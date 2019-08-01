using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;
using Newtonsoft.Json;

namespace Project.Services
{
    public class GameService
    {
        private bool HasShipJob(Ship ship, int job_id)
        {
            bool jobfilled = false;
            foreach (var c in ship.Characters)
            {
                if (c.job_id == job_id)
                {
                    jobfilled = true;
                }
            }
            return jobfilled;
        }
        public JobType GetJobType(int jobId)
        {
            JobType job = new JobType();
            foreach (var j in ShowJobTypes.GetJobs())
            {
                if (j.idkey == jobId)
                {
                    return j;
                }
            }
            return job;
        }
        public JobType GetNextJob(Ship ship)
        {
            JobType job = new JobType();
            foreach (var j in ShowJobTypes.GetJobs())
            {
                if (HasShipJob(ship,j.idkey) == false)
                {
                    return j;
                }
            }

            return job;

        }
        public bool HireCharacter(int charidx)
        {
            GameState gs = LoadGame();
            int ix = 0;
            int i = 0;
            List<Character> Characters = new List<Character>();
            foreach (var c in gs.ship.Characters)
            {
                if (c.hired == false)
                {
                    ix = ix + 1;
                    if(ix != charidx)
                    {
                       // ship.Characters.Remove(c);
                    }else
                    {
                        i = i + 1;
                        c.hired = true;
                        c.dorm_id = i;
                        //36
                        int col = (c.dorm_id % 12) +1;  //1
                        int row = (int)Math.Ceiling((decimal)c.dorm_id / 12); //3
                        c.pos_x = 744 + ((col * 16) - 8);
                        c.pos_y = 448 + ((row * 16) - 8);
                        c.dest_x = c.pos_x;
                        c.dest_y = c.pos_y;
                        Characters.Add(c);
                    }

                }else
                {
                    i = i + 1;
                    Characters.Add(c);
                }
            }
            gs.ship.Characters = Characters;
            SaveGame(gs.ship);
            return true;
        }
        public void cleanAndSave(Ship ship)
        {
            IList<Character> characters = new List<Character>();
            int i = 0;
            foreach (var c in ship.Characters)
            {
                if (c.hired)
                {
                    c.idkey = i;
                    characters.Add(c);
                    i++;
                }
            }
            ship.Characters = characters;
            SaveGame(ship);
        }
        public bool SaveGame(Ship ship)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(@"savegame/game.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, ship);
            }
            return true;

        }
        public GameState LoadGame()
        {
            GameState gs = new GameState();
            string json = File.ReadAllText(@"savegame/game.json");
            Ship ship = JsonConvert.DeserializeObject<Ship>(json);
            gs.ship=ship;
            gs.BuildingTypes=ShowBuildingTypes.GetBuildings();
            gs.JobTypes=ShowJobTypes.GetJobs();
            gs.PersonalityTypes=ShowPersonalityTypes.GetPersonalitys();
            gs.SocialQueue=new List<SocialQueue>();
            return gs;

        }
        public bool NewGame(string name = "The Intogon")
        {
            GenerateShip cg = new GenerateShip();
            Ship ship = cg.CreateShip(name);
            if(ship != null)
            {
                SaveGame(ship);
                return true;
            }else
            {
                return false;
            }
        }

        public float GetNeed(Character c, int id)
        {
            return (c.Needs[id].value + c.Needs[id].value_mods);
        }

        public float GetPersonality(Character c, int id)
        {
            return (c.Personality[id].value + c.Personality[id].value_mods);
        }

    }
}
