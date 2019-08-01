using Godot;
using System;
using Project.Domain;
using Project.Services;
using System.Collections.Generic;

public class NewGameContainer : MarginContainer
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        GameService gsi = new GameService();
        GameState gs = gsi.LoadGame();
        Ship ship = gs.ship;
        var shipnamelabel = (Label)GetNode("VBoxContainer/HBoxContainer/VBoxContainer/sname");
        shipnamelabel.Text = gs.ship.name;

        var jobnamelabel = (Label)GetNode("VBoxContainer/Screenbox/LeftSide/JobName");
        var jobnamedesc = (Label)GetNode("VBoxContainer/Screenbox/LeftSide/building");
        JobType job = gsi.GetNextJob(gs.ship);
        if (job.idkey == 0 || job.name==null)
        {
            gsi.cleanAndSave(gs.ship);
            GetTree().ChangeScene("res://MainGame.tscn");
        }
        if (ShowJobTypes.GetJobs() != null && ShowJobTypes.GetJobs() != null)
        {
            List<JobType> JobList = gs.JobTypes;
            //Jobtype=
            BuildingType building = job.GetBuilding(job.building_id);
            string JobName = job.name + " (" + building.name + ")";
            jobnamelabel.Text = JobName;
            jobnamedesc.Text = building.description;
            GenerateCharacter cg = new GenerateCharacter();
            List<Character> Canidates = new List<Character>();
            string npath = "";
            int idx = 0;
            for (int i = 0; i <= 2; i++)
            {
                idx = i + 1;
                npath = "VBoxContainer/Screenbox/VBoxContainer/VBoxContainer/canidate" + idx + "/VBoxContainer2/";
                Canidates.Add(cg.CreateCharacter(gs.ship,job.idkey));
                var charnamelabel1 = (Label)GetNode(npath+"name1");
                string cname = Canidates[i].name;
                float age = Canidates[i].age / 48;
                int ageyrs = (int)(Math.Floor(age));
                cname = cname + ", Age " + ageyrs;
                if (Canidates[i].gender == 0)
                {
                    cname = cname + " Male";
                } else
                {
                    cname = cname + " Female";
                }
                charnamelabel1.Text = cname;

                var traitlabel1 = (Label)GetNode(npath + "traits1");
                string strtrait = "Known Traits: ";
                string pname = "";
                bool lined = false;
                foreach (var p in Canidates[i].Personality)
                {
                    if (p.discovered == true)
                    {
                        //int pid= (p.idkey + 1);
                        if (p.value < 3)
                        {
                            pname = ShowPersonalityTypes.GetPersonality(p.idkey).opposite;
                        } else
                        {
                            pname = ShowPersonalityTypes.GetPersonality(p.idkey).adjective;
                        }
                        if (strtrait.Length > 50 && lined == false)
                        {
                            strtrait = strtrait + "\n";
                            lined = true;
                        }
                        strtrait = strtrait + " " + pname + ",";
                    }
                }
                traitlabel1.Text = strtrait;

                var skilllabel1 = (Label)GetNode(npath + "skill1");
                skilllabel1.Text = "Skill Level: " + Canidates[i].job_skill;

            }
        }

        var charnumlabel = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer2/chars");
        charnumlabel.Text = "Crew Hired: " + (ship.Characters.Count-3);
        var inventorylabel1 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer/inventory1");
        var inventorylabel2 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer5/inventory2");
        var inventorylabel3 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer4/inventory3");
        var inventorylabel4 = (Label)GetNode("VBoxContainer/HBoxContainer/HBoxContainer/VBoxContainer3/inventory4");
        inventorylabel1.Text = "Raw Food: \n Clean H20: \n Fuel: \n Power: \n Spare Parts: \n Medical: \n Oxygen:";
        int total_gas = (ship.oxygen + ship.nitrogen + ship.hydrogen + ship.carbondioxide);
        double oxy_perc = Math.Round(100 * (double)((double)ship.oxygen / (double)total_gas));
        double co_perc = Math.Round(100 * (double)((double)ship.carbondioxide / (double)total_gas));
        inventorylabel2.Text = ship.food + "\n" + ship.water + "\n" + ship.fuel + "\n" + ship.power + "\n" + ship.spare_parts + "\n" + ship.medical + "\n" + oxy_perc+ "%";
        inventorylabel3.Text = "Cooked Meals: \n Dirty H20: \n Hydrogen: \n Raw Resources: \n Waste Parts: \n Organics: \n C02 : ";
        inventorylabel4.Text = ship.meals + "\n" + ship.dirty_water + "\n" + ship.hydrogen + "\n "+ ship.raw_resources + "\n" + ship.waste_parts + "\n" + ship.organics + "\n" + co_perc + "%";

    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
