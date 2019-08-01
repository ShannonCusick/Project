using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;

namespace Project.Services
{
    public class CharacterDecisions
    {
        public GameService gsi = new GameService();
        public static Random r = new Random();

        public Character getCharacter(GameState gs, int CharId)
        {
            foreach (var peep in gs.ship.Characters)
            {
                if (peep.idkey == CharId) return peep;
            }
            return null;
        }

        public float getMood(GameState gs, Character peep)
        {
            float total_need = 0;
            foreach (var need in peep.Needs)
            {
                total_need = total_need + need.value;
            }
            return (total_need / peep.Needs.Count);
        }

        public int getMostNeed(GameState gs, Character peep)
        {
            int currentNeed = -1;
            float prevNeed = 10;
            foreach (var need in peep.Needs)
            {
                if (need.value < prevNeed)
                {
                    currentNeed = need.idkey;
                }
                prevNeed = need.value;
            }
            return currentNeed;
        }
        public float getHealth(GameState gs, Character peep)
        {
            return (((peep.happiness + peep.sanity) - (peep.stress - peep.anxiety)) + peep.stamina + peep.strength + (peep.constitution * 2)) / 4;
        }
        public Character goToNeed(GameState gs, Character peep, int need)
        {
            if (need == NeedsIdx.Bio)
            {
                var bt = gs.BuildingTypes[23];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "bio";
                }
                else
                {
                    //some shaninagan pee pee?
                }
            }
            if (need == NeedsIdx.Excercize)
            {
                var bt = gs.BuildingTypes[45];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "exercise";
                }
                else
                {
                    //jog around
                }
            }
            if (need == NeedsIdx.Safety)
            {
                var bt = gs.BuildingTypes[27];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "security";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Food)
            {
                var bt = gs.BuildingTypes[14];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "eat";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Water)
            {
                var bt = gs.BuildingTypes[14];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "drink";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Hygene)
            {
                var bt = gs.BuildingTypes[25];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "shower";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Comfort)
            {
                var bt = gs.BuildingTypes[19];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "lounge";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Sleep)
            {
                var bt = gs.BuildingTypes[22];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "sleep";
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Social)
            {
                var bt = gs.BuildingTypes[14];
                if (bt != null)
                {
                    peep.dest_x = bt.def_x;
                    peep.dest_y = bt.def_y;
                    peep.current_task = "social";
                }
                else
                {
                }
            }

            return peep;
        }
        public JobType getJob(GameState gs, int job_id)
        {
            foreach (JobType job in gs.JobTypes)
            {
                if (job_id == job.idkey)
                {
                    return job;
                }
            }
            return null;
        }
        public GameState whatToDo(GameState gs, Character peep)
        {
            if (peep.dest_x != peep.pos_x && peep.dest_y != peep.pos_y && peep.dest_y != 0 && peep.dest_x != 0)
            {
                return gs;
            }
            var health = getHealth(gs, peep);
            //check health

            //check needs
            bool hasToDo = false;
            var mood = getMood(gs, peep);
            float adaptability = moodMod(mood, peep.Personality[PersonalityIdx.Adaptability].value);
            if (mood < ((adaptability + health) / 2))
            {
                int currentNeed = getMostNeed(gs, peep);
                if (currentNeed > -1)
                {
                    peep = goToNeed(gs, peep, currentNeed);
                    hasToDo = true;
                }
            }
            //go to work GameService gsi = new GameService();
            JobType job = getJob(gs, peep.job_id);
            if (job != null && !hasToDo)
            {
                BuildingType building = job.GetBuilding(job.building_id);
                peep.dest_x = building.def_x - 40;
                peep.dest_y = building.def_y - 220;
                peep.current_task = "work";
                hasToDo = true;
            }
            peep.mood = mood;

            if (hasToDo)
            {
                IList<Character> characters = new List<Character>();
                foreach (var c in gs.ship.Characters)
                {
                    Character addchar = c;
                    if (c.idkey == peep.idkey)
                    {
                        addchar = peep;
                    }
                    characters.Add(addchar);
                }
                gs.ship.Characters = characters;

            }

            return gs;
        }
        public float moodMod(float mood,float mod)
        {
            var r1 = r.Next(-2, 2);
            mood = mood + r1;
            return (mood / 10) * mod;
        }

        public IList<Character> getNearBys(GameState gs, Character peep)
        {
            IList<Character> nearbys = new List<Character>();
            foreach (var c in gs.ship.Characters)
            {
                if(c.pos_x > (peep.pos_x - 40) && c.pos_x > (peep.pos_x + 40) &&
                   c.pos_y > (peep.pos_y - 40) && c.pos_y > (peep.pos_y + 40) )
                {
                    nearbys.Add(c);
                }

            }
            return nearbys;
         }
        public CharacterRelationship getRelationship(Character c, Character other)
        {
            CharacterRelationship CR = null;
            foreach (var rel in c.Relationships)
            {
                if (rel.otherChar == other.idkey)
                {
                    return rel;
                }
            }
            return CR;
        }
        public GameState addSocial(GameState gs, Character peep)
        {
            Character ogpeep = peep;
            IList<Character> nearbys = getNearBys(gs, peep);
            if (nearbys.Count == 0) return gs;


            var r1 = r.Next(2, 11);

            //social at all?
            if (gsi.GetPersonality(peep, PersonalityIdx.Outgoing) < r1) return gs;

            r1 = r.Next(-2, 2);
            float secure = (gsi.GetPersonality(peep, PersonalityIdx.Confidence) + gsi.GetPersonality(peep, PersonalityIdx.Courage)) / 2;
            secure = moodMod(peep.mood, secure) + r1;

            float friendly = (gsi.GetPersonality(peep, PersonalityIdx.Friendly) + gsi.GetPersonality(peep, PersonalityIdx.Empathy)) / 2;
            friendly = moodMod(peep.mood, friendly) + r1;

            r1 = r.Next(-2, 2);
            float optimist = (gsi.GetPersonality(peep, PersonalityIdx.Trust) + gsi.GetPersonality(peep, PersonalityIdx.Confidence) + gsi.GetPersonality(peep, PersonalityIdx.Courage) + gsi.GetPersonality(peep, PersonalityIdx.Adaptability)) / 4;
            optimist = moodMod(peep.mood, optimist) + r1;

            r1 = r.Next(-2, 2);
            float outgoing= moodMod(peep.mood, gsi.GetPersonality(peep, PersonalityIdx.Outgoing)) + r1;


            r1 = r.Next(-2, 2);
            //not effected by mood or inversly effected
            float ammusing = (gsi.GetPersonality(peep, PersonalityIdx.Cunning) + (gsi.GetPersonality(peep, PersonalityIdx.Charisma))) / 2;
            optimist = ammusing + r1;

            float r2;
            List<SocialQueue> SQS = new List<SocialQueue>();
            int i = 0;
            foreach (var other in nearbys)
            {
                CharacterRelationship CR= getRelationship(peep, other);
                //chance to ignore for a higher acquantance
                if(nearbys.Count > i+1)
                {
                    r1 = r.Next(1, 4);
                    if (r1 == 3) continue;
                    if (nearbys[i+1] != null)
                    {
                        if (getRelationship(peep,nearbys[i + 1]).value > CR.value)
                        {
                            if (outgoing < 6 || friendly < 6) continue;
                        }
                    }
                }
                ////////unmet char:
                if (CR == null)
                {
                    CR = new CharacterRelationship();
                    CR.idkey = peep.Relationships.Count;
                    CR.otherChar = other.idkey;
                    //establish judging by cover
                    float initial = 0;
                    if (secure  < peep.mood)
                    {
                        initial = (gsi.GetPersonality(other, PersonalityIdx.Attractiveness) - gsi.GetPersonality(peep, PersonalityIdx.Attractiveness));
                    }else
                    {
                        initial = gsi.GetPersonality(other, PersonalityIdx.Attractiveness);
                    }
                    CR.value = initial;
                    peep.Relationships.Add(CR);

                    //initial hello?
                    r1 = r.Next(4, 10);
                    r2 = r.Next(1, 6);
                    SocialQueue SQ = new SocialQueue();
                    //no conditions:
                    SQ.CharId=peep.idkey;
                    SQ.Participater = other.idkey;
                    SQ.Subject = "none";
                    SQ.PowerTrait = PersonalityIdx.Attractiveness;
                    SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);


                    //arrogant?
                    if (initial <=0 && secure > r1 && friendly < r2)
                    {
                        SQ.Subject = "looked_away";
                        SQ.PowerTrait = PersonalityIdx.Attractiveness;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);
                        SQS.Add(SQ);
                        break;
                    }

                    r2 = r.Next(1, 6);
                    //shy intro?
                    if (friendly > r1 && outgoing < r2)
                    {
                        SQ.Subject = "nod_hello";
                        SQ.PowerTrait = PersonalityIdx.Attractiveness;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);
                        SQS.Add(SQ);
                        break;
                    }
                    r1 = r.Next(3, 10);
                    r2 = r.Next(3, 8);
                    //Friendly + intro?
                    if (friendly > r1 && secure > r2)
                    {
                        SQ.Subject = "friendly_intro";
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Charisma);
                        SQS.Add(SQ);
                        break;
                    }

                    r1 = r.Next(5, 10);
                    //Cynical + intro?
                    if (ammusing  > r1)
                    {
                        SQ.Subject = "cynical_intro";
                        SQ.PowerTrait = PersonalityIdx.Cunning;
                        SQ.Power = gsi.GetPersonality(peep, (PersonalityIdx.Charisma)) + gsi.GetPersonality(peep, +PersonalityIdx.Cunning);
                        SQS.Add(SQ);
                        break;
                    }

                    r2 = r.Next(1, 7);
                    //flirtatious intro
                    if (initial >= -1 && outgoing > 5)
                    {
                        SQ.Subject = "flirtatious_intro";
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, (PersonalityIdx.Charisma));
                        SQS.Add(SQ);
                        break;
                    }
                }
                i++;
            }
            if(ogpeep != peep)
            {
                IList<Character> characters = new List<Character>();
                foreach (var c in gs.ship.Characters)
                {
                    Character addchar = c;
                    if (c.idkey == peep.idkey)
                    {
                        addchar = peep;
                    }
                    characters.Add(addchar);
                }
                gs.ship.Characters = characters;

            }
            gs.SocialQueue.AddRange(SQS);
            return gs;
        }
        public GameState SocialCheck(GameState gs, Character peep)
        {
            List<SocialQueue> SQS = new List<SocialQueue>();
            bool changedone = false;
            foreach (SocialQueue Q in gs.SocialQueue)
            {
                if (Q.Participater==peep.idkey)
                {
                    Character other = getCharacter(gs,Q.CharId);
                    CharacterRelationship CR = getRelationship(peep, other);
                    if (CR == null)
                    {
                        CR = new CharacterRelationship();
                        CR.idkey = peep.Relationships.Count;
                        CR.otherChar = other.idkey;
                        CR.value = 0;
                        peep.Relationships.Add(CR);
                    }
                    //move near - temp positioning
                    peep.dest_x = gs.ship.Characters[Q.CharId].pos_x + 20;
                    peep.dest_y = gs.ship.Characters[Q.CharId].pos_y;
                    //answer-- if near?
                    
                    //remove/respond

                    changedone = true;
                }
                else
                {
                    SQS.Add(Q);
                }
            }
            if (changedone)
            {
                IList<Character> characters = new List<Character>();
                foreach (var c in gs.ship.Characters)
                {
                    Character addchar = c;
                    if (c.idkey == peep.idkey)
                    {
                        addchar = peep;
                    }
                    characters.Add(addchar);
                }
                gs.ship.Characters = characters;

            }
            gs.SocialQueue = SQS;

            return gs;

        }


    }

}