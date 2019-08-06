using Godot;
using System;
using Project.Domain;
using Project.Services;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public class MainGame : MarginContainer
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public static GameService GameService = new GameService();
    public static CharacterDecisions cds = new CharacterDecisions();
    public static UpdateBuildings ubs = new UpdateBuildings();
    public static UpdateShip uss = new UpdateShip();

    public GameState gs = GameService.LoadGame();
    [Export] public int Speed = 50;
    Vector2 loc = new Vector2();
    private Navigation2D _navigation2D;
    private Vector2[][] _paths = new Vector2[200][];
    //private List<Vector2[]> _paths = new List<Vector2[]>();

    public int numChars = GameService.LoadGame().ship.Characters.Count;
    public bool[] moveNode = new bool[GameService.LoadGame().ship.Characters.Count];
    public Timer _timer = null;
    public int ticks = 0;
    public static Random r = new Random();
    // void _InputEvent(Viewport viewport, InputEvent @event, int shape_idx)

    public void _InputEvent(Viewport viewport, InputEvent @event, int shape_idx)
    {

        GD.Print("Clicked " );
        var mouseButtonEvent = @event as InputEventMouseButton;
        if (mouseButtonEvent != null)
        {
            if (mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && mouseButtonEvent.Pressed)
            {
                GD.Print("Clicked ");
            }
        }
    }
    void updateHud()
    {
        var charnumlabel = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer2/chars");
        charnumlabel.Text = "Crew Hired: " + (gs.ship.Characters.Count - 3);
        var inventorylabel1 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer/inventory1");
        var inventorylabel2 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer5/inventory2");
        var inventorylabel3 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer4/inventory3");
        var inventorylabel4 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer3/inventory4");
        inventorylabel1.Text = "Raw Food: \n Clean H20: \n Fuel: \n Power: \n Spare Parts: \n Medical: \n Oxygen:";
        int total_gas = (gs.ship.oxygen + gs.ship.nitrogen + gs.ship.hydrogen + (int)Math.Round(gs.ship.carbondioxide));
        double oxy_perc = Math.Round(100 * (double)((double)gs.ship.oxygen / (double)total_gas));
        double co_perc = Math.Round(100 * (double)((double)gs.ship.carbondioxide / (double)total_gas));
        inventorylabel2.Text = gs.ship.food + "\n" + Math.Round(gs.ship.water)+ "\n" + gs.ship.fuel + "\n" + gs.ship.power + "\n" + gs.ship.spare_parts + "\n" + gs.ship.medical + "\n" + oxy_perc + "%";
        inventorylabel3.Text = "Cooked Meals: \n Dirty H20: \n Hydrogen: \n Raw Resources: \n Waste Parts: \n Organics: \n C02 : ";
        inventorylabel4.Text = gs.ship.meals + "\n" + Math.Round(gs.ship.dirty_water) + "\n" + gs.ship.hydrogen + "\n " + gs.ship.raw_resources + "\n" + gs.ship.waste_parts + "\n" + Math.Round(gs.ship.organics) + "\n" + co_perc + "%";
        var destination = (Label)GetNode("VBoxContainer/HBoxContainer/VBoxContainer/destination");

        //make function for data for these vars!
        string dest = "";
        if (gs.ship.to_destination == 0) dest = "Orbiting Earth, Sol System \n";
        if (gs.ship.to_destination == 1) dest = "-- -- --\n";
        if (gs.ship.to_destination == 2) dest = "Orbiting Sol, Sol System \n";
        int todays = (8000 - gs.ship.location)/24;
        if (gs.ship.to_destination == 2) dest = dest + "To Kuiper Belt: "+ todays + " Days";
        if (gs.ship.to_destination < 2) dest = dest+ "To Kuiper Belt: Infinate Days";
        destination.Text=dest;
    }
    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        updateHud();
        _navigation2D = GetNode<Navigation2D>("VBoxContainer/Screenbox/Ship/Navigation2D");
 //// generate character sprite nodes ////
        //base._Ready();
        var navnode = (Node2D)GetNode("VBoxContainer/Screenbox/Ship/Navigation2D/NavigationPolygonInstance/Node2D");
        var burnButton = (Button)GetNode("VBoxContainer/HBoxContainer/VBoxContainer2/LaunchButton");
        burnButton.Connect("pressed", this, nameof(_BurnEngines));
        var buildingsButton = (Button)GetNode("VBoxContainer/HBoxContainer/VBoxContainer2/ViewBuildings");
        buildingsButton.Connect("pressed", this, nameof(_ViewBuildings));
        Texture spritetex = ResourceLoader.Load("res://char.png") as Texture;
        if (spritetex != null)
        {
           // Sprite[] _sprite = new Sprite[numChars]; // Create a new sprite!
            Area2D[] _spParent = new Area2D[numChars]; // Create a new sprite!
            Button[] _texbut = new Button[numChars]; // Create a new sprite!
            int i = 0;
            Vector2 position = new Vector2();
            Vector2 size = new Vector2();
            size.x = 8;
            size.y = 8;
            var but = (Button)GetNode("VBoxContainer/Screenbox/Ship/Navigation2D/NavigationPolygonInstance/Node2D/CharButton");
            foreach (var c in gs.ship.Characters)
            {
                if (!c.hired) continue;
                _spParent[i] = new Area2D();
                navnode.AddChild(_spParent[i]);
                //_spParent[i].
                _texbut[i] = new Button();
               // _sprite[i] = new Sprite();
               // _texbut[i].AddChild(_sprite[i]);
                _spParent[i].AddChild(_texbut[i]); // Add it as a child of this node.
               // CollisionShape2D _c2D = new CollisionShape2D();
                //_c2D.Scale = size;
                position.x = c.pos_x;
                position.y = c.pos_y;
                // _sprite[i].SetTexture(spritetex);
                 _spParent[i].SetGlobalPosition(position);
                _spParent[i].SetPosition(position);
                _spParent[i].SetName("char_"+i);
                //GD.Print(_spParent[i].Position.x + " " + _spParent[i].Position.y);
                _texbut[i].SetName("but_" + i);
                _texbut[i].SetSize(size);
                _texbut[i].SetButtonIcon(spritetex);
                _texbut[i].Connect("pressed", this, nameof(_ClickedChar), new Godot.Collections.Array(){ i });
                //_spParent[i]._InputEvent(this,@event,i);
                moveNode[i] = true;
                i = i + 1;
            }

        }
        _timer = new Timer();
        AddChild(_timer);
        _timer.Connect("timeout", this, "onTimerTimeout");
        _timer.SetWaitTime(1);
        _timer.SetOneShot(false);
        _timer.Start();

    }
    void _BurnEngines()
    {
       gs= uss.BurnEngines(gs);
    }
    void _ViewBuildings()
    {
        var namelabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/nameBox");
        var joblabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/jobBox");
        var moodlabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/moodBox");
        var infolabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/infoBox");
        string text = "";
        foreach (var building in gs.ship.Buildings)
        {
            var btype = GameService.GetBuildingType(gs, building.building_id);
            text= btype.name + " health: " + building.health + " hygene: " + building.hygene + " \n" + text;
        }
        namelabel.Text = text;
        joblabel.Text = "";
        moodlabel.Text = "";
        infolabel.Text = "";

    }
    void _ClickedChar(int i)
    {
        //Character peep= Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(c);
        var chars = GetNode("/root/NewGameContainer/").Get("gs.ship.Characters");
        Character peep = gs.ship.Characters[i];
        GameService gsi = new GameService();
        // GD.Print("Clicked !! " + name);
        var namelabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/nameBox");
        var joblabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/jobBox");
        var moodlabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/moodBox");
        var infolabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/infoBox");
        if (namelabel == null)
        {
            // GD.Print("null");
            return;
        }
        JobType job = gsi.GetJobType(peep.job_id);
        BuildingType building = job.GetBuilding(job.building_id);
        string JobName = job.name + " (" + building.name + ")";
        namelabel.Text = peep.idkey + " " + peep.name;
        joblabel.Text = JobName;

        string mood = "\n Excercise: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Excercize)) + " Safety: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Safety))
            + " Food: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Food)) + " Water: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Water)) + " Hygene: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Hygene))
            + " Comfort: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Comfort)) + " Sleep: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Sleep)) +
            " Bio: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Bio)) + " Social: " + Math.Round(gsi.GetNeed(peep, NeedsIdx.Social));
        moodlabel.Text = mood;

        string info = "Current task: " + peep.current_task +"\n location: "+peep.pos_x+","+peep.pos_y+" destination: "+ peep.dest_x + ","+peep.dest_y;
        info = info + " \n Mood: " + peep.mood + " Health: " + cds.getHealth(peep) + "\n  \n Relationships: \n ";
        int x = 0;
        foreach (var rel in peep.Relationships)
        {
            info = info + rel.otherChar + " " + rel.value + "("+rel.lastAction+"),";
            x++;
            if (x % 2==0) info = info + " \n ";
        }
        info = info + "\n \n Personality: \n";
        info = info + "Adaptability: " + gsi.GetPersonality(peep, PersonalityIdx.Adaptability) + " Ambition: " + gsi.GetPersonality(peep, PersonalityIdx.Ambition) + " Attractiveness:" + gsi.GetPersonality(peep, PersonalityIdx.Attractiveness) + "\n";
        info = info + "Charisma: " + gsi.GetPersonality(peep, PersonalityIdx.Charisma) + " Confidence: " + gsi.GetPersonality(peep, PersonalityIdx.Confidence) + " Courage: " + gsi.GetPersonality(peep, PersonalityIdx.Courage) + "\n";
        info = info + "Creativity: " + gsi.GetPersonality(peep, PersonalityIdx.Creativity) + " Cunning: " + gsi.GetPersonality(peep, PersonalityIdx.Cunning) + " Empathy:" + gsi.GetPersonality(peep, PersonalityIdx.Empathy) + "\n";
        info = info + "Friendly: " + gsi.GetPersonality(peep, PersonalityIdx.Friendly) + " Honesty:" + gsi.GetPersonality(peep, PersonalityIdx.Honesty) + " Intelligence: " + gsi.GetPersonality(peep, PersonalityIdx.Intelligence) + "\n";
        info = info + "Leadership: " + gsi.GetPersonality(peep, PersonalityIdx.Leadership) + " Logic:" + gsi.GetPersonality(peep, PersonalityIdx.Logic) + " Observation:" + gsi.GetPersonality(peep, PersonalityIdx.Observation) + "\n";
        info = info + " Outgoing: " + gsi.GetPersonality(peep, PersonalityIdx.Outgoing) +" Patience: " + gsi.GetPersonality(peep, PersonalityIdx.Patience) +" Persuasion: " + gsi.GetPersonality(peep, PersonalityIdx.Persuasion) + "\n";
        info = info + " Productivity: " + gsi.GetPersonality(peep, PersonalityIdx.Productivity) + " Trust: " + gsi.GetPersonality(peep,PersonalityIdx.Trust) + "\n";

        infolabel.Text = info;
    }
    void onTimerTimeout()
    {
        int i = 0;
        foreach (var c in gs.ship.Characters)
        {
            Character peep = c;
            GameState prevgs = gs;
            gs = cds.SocialCheck(gs, peep);
            if (c.idkey % 5 == ticks)
            {
                gs = cds.addSocial(gs, peep);
                gs = cds.whatToDo(gs, peep);
            }
            //GD.Print(peep.current_task);
            
            i = i + 1;
        }

        if (gs.SocialQueue.Count > 0)
        {
           // GD.Print(gs.SocialQueue[0].Subject);
        }
        if (ticks == 6)
        {
            ticks = 0;
        }else
        {
            ticks = ticks + 1;
        }
        if (ticks == 6)
        {
            Modifier mod = uss.GetModifier(gs, "in_burn");
            var burnButton = (Button)GetNode("VBoxContainer/HBoxContainer/VBoxContainer2/LaunchButton");
            if (mod != null)
            { 
                gs = uss.ShipBurn(gs,mod);
                if (mod.expire <= 10 && mod.expire > 5) burnButton.Text = "Preparing for Burn in " + (mod.expire - 5);
                if (mod.expire <= 5) burnButton.Text = "--Burning-- "+ mod.expire;
            }else
            {
                burnButton.Text = "Burn";
            }
            gs = uss.TickUpdate(gs);
        }
        //foreach (var sq in gs.SocialQueue)
        //{
        //    GD.Print(sq.Subject);
        //}
        if (ticks == 1)
        {
            gs = ubs.TickUpdate(gs);
            //Building building = GameService.GetBuilding(gs, 12);
            //GD.Print("building" + building.health);
            //GD.Print("# of mods" + building.Modifiers.Count);
            //var mod = ubs.GetModifier(building, "dirty_plates");
            //if (mod != null) GD.Print("dirty plates=" + mod.value);
            //foreach (var b in gs.ship.Buildings)
            //{
            //    if (b.building_id == 12)
            //    {
            //        GD.Print(b.hygene + " hyg ");
            //        GD.Print(b.health + " health ");
            //        foreach (var m in b.Modifiers)
            //        {
            //            GD.Print(m.name + b.building_id + " " + m.value);
            //        }
            //    }
            //}
            if (gs.ship.water > 10000) gs.ship.water = 10000;
            if (gs.ship.dirty_water < 0) gs.ship.dirty_water = 0;
            updateHud();
        }
    }
    public override void _Process(float delta)
    {
        // Called every frame. Delta is time since last frame.
        // Update game logic here.

        var PathNode = (NavigationPolygonInstance)GetNode("VBoxContainer/Screenbox/Ship/Navigation2D/NavigationPolygonInstance");
        //280,340
        //if(delta % 5 == 0)
        // {
        int i = 0;
        Area2D[] _sprite = new Area2D[numChars];
        IList<Character> characters = new List<Character>();
        foreach (var peep in gs.ship.Characters)
        {
            if (!peep.hired) continue;
            Vector2 loc = new Vector2();
            _sprite[i]=(Area2D)GetNode("VBoxContainer/Screenbox/Ship/Navigation2D/NavigationPolygonInstance/Node2D/char_"+i);
            
            if(peep.dest_x != peep.pos_x && peep.dest_y != peep.pos_y)
            {
                loc.x = peep.dest_x;
                loc.y = peep.dest_y;
                MoveCharacter(Speed * delta,_sprite[i],i);
                UpdateNavigationPath(_sprite[i].Position, loc,i);
                peep.pos_x = (int) Math.Round(_sprite[i].Position.x);
                peep.pos_y = (int) Math.Round(_sprite[i].Position.y);
            }
            characters.Add(peep);
           // _sprite[i].m
            i = i + 1;
        }
        gs.ship.Characters = characters;
        Update();
    }
    private void UpdateNavigationPath(Vector2 start, Vector2 end,int i)
    {
        _paths[i] = _navigation2D.GetSimplePath(start, end);
        _paths[i] = _paths[i].Skip(1).Take(_paths[i].Length - 1).ToArray();
    }
    private void MoveCharacter(float speed,Area2D _char,int i)
        {
            var lastPosition = _char.Position;
            if (_paths[i] == null) return;
            for (var x = 0; x < _paths[i].Length; x++)
            {
                var distanceBetweenPoints = lastPosition.DistanceTo(_paths[i][x]);
                if(speed <= distanceBetweenPoints)
                {
                    _char.Position = lastPosition.LinearInterpolate(_paths[i][x], speed / distanceBetweenPoints);
                    break;
                }
                
                if (speed < 0.0) 
                {
                    _char.Position = _paths[i][0];
                    moveNode[i] = false;
                    break;
                }

                speed -= distanceBetweenPoints;
                lastPosition = _paths[i][x];
                _paths[i] = _paths[i].Skip(1).Take(_paths[i].Length - 1).ToArray();
            }
        }
}
