using Godot;
using System;
using Project.Services;
using Project.Domain;
using Newtonsoft.Json;

public class CharButton : Button
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here

    }
    void _ClickedChar(int i, string c)
    {
        //Character peep= Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(c);
        var chars = GetNode("/root/NewGameContainer/").Get("gs.ship.Characters");
        GD.Print(chars);
        return;
        Character peep = new Character();
        GameService gsi = new GameService();
        // GD.Print("Clicked !! " + name);
        var namelabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/nameBox");
        var joblabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/jobBox");
        var moodlabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/moodBox");
        var infolabel = (Label)GetNode("/root/NewGameContainer/VBoxContainer/Screenbox/infoNode/infoBox");
        if (namelabel==null)
        {
           // GD.Print("null");
            return;
        }
        JobType job = gsi.GetJobType(peep.job_id);
        BuildingType building = job.GetBuilding(job.building_id);
        string JobName = job.name + " (" + building.name + ")";
        namelabel.Text=peep.idkey + " " +peep.name;
        joblabel.Text = JobName;

        string mood = "Excercise: " + gsi.GetNeed(peep, NeedsIdx.Excercize) + " Safety: " + gsi.GetNeed(peep, NeedsIdx.Safety)
            + " Food: " + gsi.GetNeed(peep, NeedsIdx.Food) + " Water: " + gsi.GetNeed(peep, NeedsIdx.Water) + " Hygene: " + gsi.GetNeed(peep, NeedsIdx.Hygene)
            + " Comfort: " + gsi.GetNeed(peep, NeedsIdx.Comfort) + " Sleep: " + gsi.GetNeed(peep, NeedsIdx.Sleep) +
            " Bio: " + gsi.GetNeed(peep,NeedsIdx.Bio) + " Social: " + gsi.GetNeed(peep, NeedsIdx.Social);
        moodlabel.Text = mood;

        string info = "Current task: " + peep.current_task;
        infolabel.Text = info;
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
