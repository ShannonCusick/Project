using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;


namespace Project.Services
{
    public class Jobs
    {
        public static Random r = new Random();

        public GameState performJob(GameState gs, Character peep, JobType job,Building building)
        {
            //Do not put Character personal updates here!
            GameService gsi = new GameService();
            CharacterDecisions cds = new CharacterDecisions();
            UpdateBuildings ubs = new UpdateBuildings();


            //repairs/maintenance
            //observation test
            var r1 =r.Next(-2, 11);
            var r2 = r.Next(-3, 3);
            var mood= cds.getMood(gs, peep);
            var observation = cds.moodMod(mood, peep.Personality[PersonalityIdx.Observation].value)+r2;
            r2 = r.Next(-2, 5);
            var productivity = (cds.moodMod(mood, peep.Personality[PersonalityIdx.Productivity].value) + r2);
            var fproductivity = (productivity) / 10;
            bool didMainJob = false;
            int job_id = job.idkey;
            //fproductivity = fproductivity + .55f;
            if (observation > r1)
            {
                building.health = building.health + fproductivity;
            }
            ///////cleaning

            if (job.job_id == 7)
            {
                building.hygene = building.hygene + productivity;
            }

            //////////////job type specific direct production///////////
            var output = building.health / 100;
            float prod = 0;
            Modifier mod = null;
            //Farmer
            if (job_id == 4)
            {
                mod = ubs.GetModifier(building, "hydro_production");
                if(mod != null)
                {
                    output = output + mod.value;
                }
                //harvest crop
                mod = ubs.GetModifier(building, "planted_crops");
                if (mod != null)
                {
                    if (mod.value > 1)
                    {
                        prod = output * fproductivity;
                        gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "planted_crops", -1, -1, true, true);
                        gs.ship.food = gs.ship.food + (int)Math.Ceiling(prod);
                        didMainJob = true;
                    }
                }
                //plant crop
                if (didMainJob == false)
                {
                    mod = ubs.GetModifier(building, "soil");
                    if (mod != null)
                    {
                        if (mod.value > 2)
                        {
                            gs.ship.water = gs.ship.water - 1;
                            gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "planted_crops", 1, -1, true, true);
                            gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "soil", -2, -1, true, true);
                        }
                    }
                }
            }
            //Cafeteria Cook
            if (job_id == 14)
            {
                if(gs.ship.meals < (gs.ship.Characters.Count * 2))
                {
                    mod = ubs.GetModifier(building, "dirty_plates");
                    if (mod != null)
                    {
                        if (mod.value < 100 && building.hygene > 55)
                        {
                            int meal = (int)Math.Ceiling(fproductivity);
                            gs.ship.food = gs.ship.food - meal;
                            gs.ship.meals = gs.ship.meals + meal;
                            gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "dirty_plates", meal * 2, -1, true, true);
                            didMainJob = true;
                        }
                        if(building.hygene <= 55)
                        {
                            building.hygene = building.hygene + productivity;
                            didMainJob = true;
                        }
                    }
                }
                if (didMainJob == false) job_id = 16;
            }
            if (job_id == 16)
            {
                if (building.hygene > 35 && gs.ship.water > 0) { 
                    int cleanplates = (int)Math.Ceiling(fproductivity) * -1;
                    gs.ship.water = gs.ship.water - .25f;
                    gs.ship.dirty_water = gs.ship.dirty_water + .225f;
                    gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "dirty_plates", cleanplates, -1, true, true);
                 }else {
                    building.hygene = building.hygene + productivity;
                }
            }

            if (job_id == 24)
            {
                //barista //make drinks
                mod = ubs.GetModifier(building, "drinks");
                bool makedrinks = false;
                if (mod == null)
                {
                    makedrinks = true;
                }else
                {
                    if (mod.value < 10) makedrinks = true;
                }
                if (makedrinks == true)
                {
                    if(gs.ship.water > 1 && gs.ship.food > 0)
                    gs.ship.water = gs.ship.water - .15f;
                    gs.ship.food = gs.ship.food - 1;
                    gs.ship.Buildings = ubs.AddModifier(gs, building.building_id, "drinks", 5, -1, true, true);
                }

            }
            if (job_id == 38)
            {
                //global mechanic
                building.health = building.health + productivity;

            }


                if (building.health > 100) building.health = 100;
           if (building.hygene > 100) building.hygene = 100;
            ////////end updates///////
            List<Building> buildings = new List<Building>();
            foreach (var build in gs.ship.Buildings)
            {
                var b = build;
                if (b.idkey == building.idkey)
                {
                    b = building;
                }
                buildings.Add(b);
            }
            gs.ship.Buildings = buildings;
            //IList<Character> characters = new List<Character>();
            //foreach (var c in gs.ship.Characters)
            //{
            //    Character addchar = c;
            //    if (c.idkey == peep.idkey)
            //    {
            //        addchar = peep;
            //    }
            //    characters.Add(addchar);
            //}
            //gs.ship.Characters = characters;
            return gs;
        }

    }
}
