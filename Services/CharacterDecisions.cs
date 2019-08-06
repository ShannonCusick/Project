using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;

namespace Project.Services
{
    public class CharacterDecisions
    {
        public static GameService gsi = new GameService();
        public static UpdateBuildings ubs = new UpdateBuildings();
        public static Jobs js = new Jobs();
        public static UpdateShip uss = new UpdateShip();
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
            total_need = total_need + peep.happiness;
            return (total_need / (peep.Needs.Count+1));
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
                    prevNeed = need.value;
                }
            }
            return currentNeed;
        }
        public Character updateNeed(Character peep,int needId,float addValue)
        {
            List<CharacterNeeds> CNS = new List<CharacterNeeds>();
            foreach (var CN in peep.Needs)
            {
                var need = CN;
                if (CN.idkey == needId) need.value = need.value + addValue;
                if (need.value > 10) need.value = 10;
                CNS.Add(need);
            }
            peep.Needs = CNS;
            return peep;
        }
        public float getHealth(Character peep)
        {
            return (((peep.happiness + peep.sanity) - (peep.stress - peep.anxiety)) + peep.stamina + peep.strength + (peep.constitution * 2)) / 4;
        }
        public Character goToNeed(GameState gs, Character peep, int need)
        {
            if (need == NeedsIdx.Bio)
            {
                var bt = gsi.GetBuildingType(gs, 24);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "bio";
                    peep.current_building = 24;
                }
                else
                {
                    //some shaninagan pee pee?
                }
            }
            if (need == NeedsIdx.Excercize)
            {
                var bt = gsi.GetBuildingType(gs, 45);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "exercise";
                    peep.current_building = 45;
                }
                else
                {
                    //jog around
                }
            }
            if (need == NeedsIdx.Safety)
            {
                var bt = gsi.GetBuildingType(gs, 27);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "security";
                    peep.current_building = 27;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Food)
            {
                var bt = gsi.GetBuildingType(gs, 14);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "eat";
                    peep.current_building = 14;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Water)
            {
                var bt = gsi.GetBuildingType(gs, 14);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "drink";
                    peep.current_building = 14;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Hygene)
            {
                var bt = gsi.GetBuildingType(gs, 25);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "shower";
                    peep.current_building = 25;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Comfort)
            {
                var bt = gsi.GetBuildingType(gs,19);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "lounge";
                    peep.current_building = 19;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Sleep)
            {
                var bt = gsi.GetBuildingType(gs,22);
                if (bt != null)
                {
                    peep.dest_x = bt.def_x - 40;
                    peep.dest_y = bt.def_y - 220;
                    peep.current_task = "sleep";
                    peep.current_building = 22;
                }
                else
                {
                }
            }
            if (need == NeedsIdx.Social)
            {
                int r1 = r.Next(-3, 3);
                float outgoing = gsi.GetPersonality(peep, PersonalityIdx.Outgoing) + r1;
                bool populated = true;
                if (outgoing <= 5) populated = false;
                BuildingType bt;
                Building visit = findPopulatedSocial(gs, populated);
                if (visit != null)
                {
                    bt = gsi.GetBuildingType(gs, visit.building_id);
                    peep.current_building = visit.building_id;
                }
                else
                {
                    r1 = r.Next(16, 21);
                    if (r1 == 20) r1 = 21;
                    bt = gsi.GetBuildingType(gs, r1);
                    peep.current_building = r1;
                }
                peep.current_task = "social";
                peep.dest_x = bt.def_x - 40;
                peep.dest_y = bt.def_y - 220;
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
        public GameState GoToDorm(GameState gs, Character peep)
        {

            int col = (peep.dorm_id % 12) + 1;  //1
            int row = (int)Math.Ceiling((decimal)peep.dorm_id / 12); //3
            peep.dest_x = 700 + ((col * 16) - 8);
            peep.dest_y = 220 + ((row * 16) - 8);
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
            return gs;
        }
        public GameState whatToDo(GameState gs, Character peep)
        {
            gs.ship.oxygen = gs.ship.oxygen - 1;
            gs.ship.carbondioxide = gs.ship.carbondioxide + .06f;
            int r1 = 0;
            Modifier burning = uss.GetModifier(gs, "in_burn");
            if (burning != null && peep.job_id != 32)
            {
                peep.current_task = "securing_self";
                peep.current_building = 22;
                if (burning.value > 4) { peep.anxiety = peep.anxiety - ((1 - gsi.GetPersonality(peep, PersonalityIdx.Courage)) / 20); }
                else { peep.stress = peep.stress - ((1 - gsi.GetPersonality(peep, PersonalityIdx.Courage)) / 20); }
                gs = GoToDorm(gs, peep);
                return gs;
            }

            if ((
                peep.dest_x < (peep.pos_x -5) && peep.dest_x > (peep.pos_x +5) &&
                peep.dest_y < (peep.pos_y - 5) && peep.dest_y > (peep.pos_y + 5)
                ) && peep.dest_y != 0 && peep.dest_x != 0)
            {
                return gs;
            }
            ///////ship burn/////

            //check health
            var health = getHealth(peep);

            //check needs
            bool hasToDo = false;
            var mood = getMood(gs, peep);
            float adaptability = moodMod(mood, peep.Personality[PersonalityIdx.Adaptability].value);
            
            //update needs based on current activity
            List<CharacterNeeds> CNS = new List<CharacterNeeds>();
            bool fullfilling = false;
            foreach (var CN in peep.Needs)
            {
                var need = CN;
                float decay = 0;
                if (need.idkey == NeedsIdx.Bio)
                {
                    need.value = need.value - .01f;
                    if (peep.current_task == "bio")
                    {
                        need.value = need.value + 3;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 2);
                        gs.ship.dirty_water = gs.ship.dirty_water + .25f;
                        //gs.ship.organics = gs.ship.organics + .99f; done by sewage
                    }

                }
                if (need.idkey == NeedsIdx.Comfort)
                {
                    if(peep.current_task=="work") need.value = need.value - .005f;
                    if (peep.current_task == "lounge")
                    {
                        need.value = need.value + 1.5f;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 1);
                    }
                }
                if (need.idkey == NeedsIdx.Excercize)
                {
                    if(peep.current_task=="sleep") need.value = need.value - .005f;
                    if (peep.current_task == "exercise")
                    {
                        need.value = need.value + 1;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 1);
                        gs.ship.oxygen = gs.ship.oxygen - 1;
                    }
                }
                if (need.idkey == NeedsIdx.Food)
                {
                    need.value = need.value - .01f;
                    if (peep.current_task == "eat") {
                        need.value = need.value + 2;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 2);
                        ubs.AddModifier(gs, peep.current_building, "dirty_plates", 1, -1, true, true);
                        gs.ship.meals = gs.ship.meals - 1;
                    }
                }
                if (need.idkey == NeedsIdx.Water)
                {
                    need.value = need.value - .01f;
                    if (peep.current_task == "excercize" || peep.current_task == "eat") need.value=need.value - .025f;
                    if (peep.current_task == "drink")
                    {
                        need.value = need.value + 3;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 1);
                        gs.ship.water = gs.ship.water - .2f;
                    }
                }
                if (need.idkey == NeedsIdx.Hygene)
                {
                    if (peep.current_task == "excercize") need.value = need.value - .5f;
                    if (peep.current_task == "work") need.value = need.value - .02f;
                    if (peep.current_task == "bio") need.value = need.value - .5f;
                    if (peep.current_task == "shower")
                    {
                        need.value = need.value + 1;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, 1);
                        gs.ship.water = gs.ship.water - .25f;
                        gs.ship.dirty_water = gs.ship.dirty_water + .245f;
                    }
                }
                if (need.idkey == NeedsIdx.Safety)
                {
                    if(peep.current_task == "security") {
                        need.value = need.value + 1;
                        fullfilling = true;
                    }
                }
                if (need.idkey == NeedsIdx.Sleep)
                {
                    need.value = need.value - .01f;
                    if (peep.current_task == "sleep") {
                        need.value = need.value + .06f;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, .2f);
                    }
                }
                if (need.idkey == NeedsIdx.Social)
                {
                    need.value = need.value - .015f;
                    if (peep.current_task == "social")
                    {
                        need.value = need.value + .025f;
                        fullfilling = true;
                        gs.ship.Buildings = ubs.UpdateHygene(gs, peep.current_building, .5f);
                    }
                }

                if (need.value > 10)
                {
                    need.value = 10;
                    fullfilling = false;
                }
                CNS.Add(need);
            }
            peep.Needs = CNS;

            if (mood < (10-((adaptability + health) / 2)) + 2 && !fullfilling)
            {
                int currentNeed = getMostNeed(gs, peep);
                if (currentNeed > -1)
                {
                    peep = goToNeed(gs, peep, currentNeed);
                    hasToDo = true;
                }
            }
            if (peep.job_id == 32 && burning != null)
            {
                peep.current_task = "work";
                if (burning.value > 4){ peep.anxiety = peep.anxiety - ((1 - gsi.GetPersonality(peep, PersonalityIdx.Courage)) / 10);}
                else{peep.stress = peep.stress - ((1 - gsi.GetPersonality(peep, PersonalityIdx.Courage)) / 10); }
                peep.dest_x = 1060;
                peep.dest_y = 380;
            }
            JobType job = getJob(gs, peep.job_id);
            if (peep.current_task == "work")
            {
                Building building = gsi.GetBuilding(gs, peep.current_building);
                gs = js.performJob(gs, peep, job, building);
                if ((peep.job_id == 44 || peep.job_id == 38) && r.Next(0, 4) == 0) peep.current_task = "reset";
            }
            //go to work GameService gsi = new GameService();
            if (job != null && !hasToDo && !fullfilling && peep.current_task !="work")
            {
                BuildingType buildingtype = new BuildingType();
                if (job.idkey == 44 || job.idkey == 38)
                {
                    /////////global janitor/mechanic
                    //find lowest hygene building
                    List<Building> buildings = new List<Building>();
                    Building cbuilding= null;
                    float prevCondition = 99;
                    foreach (var build in gs.ship.Buildings)
                    {
                        if(build.hygene <= prevCondition && job.idkey==44)
                        {
                            cbuilding = build;
                            prevCondition = build.hygene;
                        }
                        if (build.health <= prevCondition && job.idkey == 38)
                        {
                            cbuilding = build;
                            prevCondition = build.health;
                        }
                    }
                    if(cbuilding != null)
                    {
                        buildingtype = job.GetBuilding(cbuilding.building_id);
                        peep.current_building = cbuilding.building_id;
                    }else
                    {
                        buildingtype = job.GetBuilding(job.building_id);
                        peep.current_building = job.building_id;
                    }
                }
                else
                {
                    buildingtype = job.GetBuilding(job.building_id);
                    peep.current_building = job.building_id;
                }
                peep.dest_x = buildingtype.def_x - 40;
                peep.dest_y = buildingtype.def_y - 220;
                peep.current_task = "work";
                hasToDo = true;
            }
            peep.mood = mood;
            IList<Character> nearbys = getNearBys(gs, peep);
            foreach (var other in nearbys)
            {
                if ((other.dest_x == peep.dest_x && other.dest_y == peep.dest_y) || (other.pos_x == peep.pos_x && other.pos_y == peep.pos_y))
                {
                    int minx = r.Next(-40, -10);
                    if(r.Next(0,2)==0) minx = r.Next(10, 40);
                    int miny = r.Next(-40, -10);
                    if (r.Next(0, 2) == 0) miny = r.Next(10, 40);
                    peep.dest_x = peep.dest_x + minx;
                    peep.dest_y = peep.dest_y + miny;
                }
            }

            ////////end update char///////
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
            return gs;
        }

        public float moodMod(float mood,float mod)
        {
            var r1 = r.Next(-2, 2);
            mood = mood + r1;
            return (mood / 10) * mod;
        }

        public Building findPopulatedSocial(GameState gs, bool populated)
        {
            Building building = null;
            int prevCount = 0;
            foreach (var b in gs.ship.Buildings)
            {
                if(b.building_id==14 || b.building_id== 16 || b.building_id == 17 || b.building_id == 18 || b.building_id == 19 || b.building_id == 21)
                {
                    //count chars//
                    int i = 0;
                    foreach (var c in gs.ship.Characters)
                    {
                        if (c.current_building == b.building_id) i = i + 1;
                    }
                    if (populated == true)
                    {
                        if (i > prevCount)
                        {
                            building = b;
                            prevCount = i;
                        }
                    }else
                    {
                        if (i <= prevCount)
                        {
                            building = b;
                            prevCount = i;
                        }
                    }
                }
            }

            return building;
        }

        public IList<Character> getNearBys(GameState gs, Character peep)
        {
            IList<Character> nearbys = new List<Character>();
            foreach (var c in gs.ship.Characters)
            {
                if (c.idkey == peep.idkey) continue;
                if(c.pos_x > (peep.pos_x - 40) && c.pos_x < (peep.pos_x + 40) &&
                   c.pos_y > (peep.pos_y - 40) && c.pos_y < (peep.pos_y + 40) )
                {
                    nearbys.Add(c);
                    continue;
                }
                if((c.dest_x < (c.pos_x - 5) && c.dest_x > (c.pos_x + 5) &&
                c.dest_y < (c.pos_y - 5) && c.dest_y > (c.pos_y + 5)) && peep.current_building==c.current_building)
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
            if (peep.current_task == "sleep") return gs;
            Character ogpeep = peep;
            IList<Character> nearbys = getNearBys(gs, peep);
            if (nearbys.Count == 0) return gs;
            float mood = getMood(gs, peep);
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

            float arrogance = (gsi.GetPersonality(peep, PersonalityIdx.Confidence) + gsi.GetPersonality(peep, PersonalityIdx.Leadership) + gsi.GetPersonality(peep, PersonalityIdx.Intelligence)) / 3;

            float r2;
            List<SocialQueue> SQS = new List<SocialQueue>();
            int i = 0;
            foreach (var other in nearbys)
            {
                CharacterRelationship CR= getRelationship(peep, other);
                //chance to ignore for a higher acquantance
                int x = i + 1;
                if (other.current_task == "sleep") continue;
                if (nearbys.Count > x)
                {
                    r1 = r.Next(1, 5);
                    if (r1 != 3) continue;
                    if (nearbys[x] != null)
                    {
                        if(getRelationship(peep, nearbys[x]) != null){
                            float val = 0;
                            if (CR != null) val = CR.value;
                            if (getRelationship(peep, nearbys[x]).value > val)
                            {
                                if (outgoing < 6 || friendly < 6) continue;
                            }

                        }
                    }
                }
                SocialQueue SQ = new SocialQueue();
                //no conditions:
                SQ.CharId = peep.idkey;
                SQ.Participater = other.idkey;
                SQ.Subject = "none";
                SQ.PowerTrait = PersonalityIdx.Attractiveness;
                SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);
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

                    //initial hello?
                    r1 = r.Next(4, 11);
                    r2 = r.Next(1, 6);


                    //arrogant?
                    if (initial <=0 && secure > r1 && friendly < r2)
                    {
                        SQ.Subject = "looked_away";
                        SQ.PowerTrait = PersonalityIdx.Attractiveness;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);
                        SQS.Add(SQ);
                        CR.lastAction = SQ.Subject;
                        peep.Relationships.Add(CR);
                        break;
                    }

                    r2 = r.Next(0, 6);
                    //shy intro?
                    if (friendly > r1 && outgoing <= r2)
                    {
                        SQ.Subject = "nod_hello";
                        SQ.PowerTrait = PersonalityIdx.Attractiveness;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Attractiveness);
                        SQS.Add(SQ);
                        CR.lastAction = SQ.Subject;
                        peep.Relationships.Add(CR);
                        break;
                    }
                    r1 = r.Next(3, 7);
                    r2 = r.Next(3, 7);
                    //Friendly + intro?
                    if (friendly > r1 && secure > r2)
                    {
                        SQ.Subject = "friendly_intro";
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Charisma);
                        SQS.Add(SQ);
                        CR.lastAction = SQ.Subject;
                        peep.Relationships.Add(CR);
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
                        CR.lastAction = SQ.Subject;
                        peep.Relationships.Add(CR);
                        break;
                    }

                    r2 = r.Next(1, 7);
                    //flirtatious intro
                    if (initial >= -1 && outgoing > 5 && other.gender != peep.gender)
                    {
                        SQ.Subject = "flirtatious_intro";
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, (PersonalityIdx.Charisma));
                        SQS.Add(SQ);
                        CR.lastAction = SQ.Subject;
                        peep.Relationships.Add(CR);
                        break;
                    }
                }
                //acquantance
                if(CR != null)
                {
                    r1 = r.Next(0, 2);
                    //if (r1 == 1) return gs;
                    if (CR.value > -10)
                    {
                        r1 = r.Next(4, 11);
                        float power = 0;
                        if(outgoing >= r1)
                        {
                            //work
                            float work_attitude = ((5 - gsi.GetPersonality(peep, (PersonalityIdx.Ambition))) * -1) + ((5 - mood) * -1);
                            if(peep.current_task=="work" && other.current_task == "work")
                            {
                                r1 = r.Next(0, 2);
                                //if (r1 != 0) return gs;
                                r1 = r.Next(0, 3);
                                if (r1 == 0)
                                {
                                    SQ.Subject = "work";
                                    SQ.PowerTrait = PersonalityIdx.Ambition;
                                    SQ.Power = work_attitude;
                                    CR.lastAction = SQ.Subject;
                                    peep = AddUpdateRelationship(peep, CR,0);
                                    SQS.Add(SQ);
                                    break;
                                }
                                if (r1 == 1)
                                {
                                    //complain? 
                                    if(work_attitude < 2)
                                    {
                                        SQ.Subject = "complain_about_work";
                                        SQ.PowerTrait = PersonalityIdx.Ambition;
                                        SQ.Power = work_attitude;
                                        CR.lastAction = SQ.Subject;
                                        peep = AddUpdateRelationship(peep, CR,0);
                                        SQS.Add(SQ);
                                        break;
                                    }
                                    else
                                    {
                                        SQ.Subject = "enthuse_about_work";
                                        SQ.PowerTrait = PersonalityIdx.Ambition;
                                        SQ.Power = work_attitude;
                                        CR.lastAction = SQ.Subject;
                                        peep = AddUpdateRelationship(peep, CR,0);
                                        SQS.Add(SQ);
                                        break;
                                    }
                                }
                                if (r1 == 2)
                                {
                                    r2 = r.Next(4, 10);
                                    if(arrogance > r2)
                                    {
                                        SQ.Subject = "offer_advice";
                                        SQ.PowerTrait = PersonalityIdx.Productivity;
                                        SQ.Power = peep.job_skill;
                                        SQS.Add(SQ);
                                        CR.lastAction = SQ.Subject;
                                        peep = AddUpdateRelationship(peep, CR,0);
                                        break;
                                    }
                                    r2 = r.Next(0, 7);
                                    if (arrogance > r2)
                                    {
                                        SQ.Subject = "ask_advice";
                                        SQ.PowerTrait = PersonalityIdx.Productivity;
                                        SQ.Power = peep.job_skill;
                                        SQS.Add(SQ);
                                        CR.lastAction = SQ.Subject;
                                        peep = AddUpdateRelationship(peep, CR,0);
                                        break;
                                    }
                                }

                             }

                        }
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
            int r1 = r.Next(-3, 3);
            var outgoing = gsi.GetPersonality(peep, PersonalityIdx.Outgoing) + r1;
            foreach (SocialQueue Q in gs.SocialQueue)
            {
                if (Q.Participater==peep.idkey)
                {
                    Character other = getCharacter(gs,Q.CharId);
                    CharacterRelationship CRi = getRelationship(peep, other);
                    //add mods...responses
                    float reaction = Q.Power;
                    string response = "";
                    SocialQueue SQ = new SocialQueue();

                    float arrogance = (gsi.GetPersonality(peep, PersonalityIdx.Confidence) + gsi.GetPersonality(peep, PersonalityIdx.Leadership) + gsi.GetPersonality(peep, PersonalityIdx.Intelligence)) / 3;
                    float mood = getMood(gs, peep);

                    if (Q.Subject == "looked_away")
                    {
                        reaction = 10 - gsi.GetPersonality(peep, PersonalityIdx.Confidence);
                    }
                    if (Q.Subject == "friendly_intro" && CRi==null)
                    {
                        reaction = ((5-Q.Power) * -1) + gsi.GetPersonality(peep, PersonalityIdx.Friendly);
                        response = "friendly_intro";
                    }
                    if (Q.Subject == "nod_hello" && CRi == null)
                    {
                        if(outgoing > 5)
                        {
                            response = "friendly_intro";
                        }else
                        {
                            response = "nod_hello";
                        }
                    }
                    if (Q.Subject == "cynical_intro" && CRi == null)
                    {
                        reaction = ((5 - Q.Power) * -1) + gsi.GetPersonality(peep, PersonalityIdx.Cunning);
                        response = "friendly_intro";
                    }
                    if (Q.Subject == "flirtatious_intro" && CRi == null)
                    {
                        reaction = gsi.GetPersonality(other, PersonalityIdx.Attractiveness) - gsi.GetPersonality(peep, PersonalityIdx.Attractiveness) + Q.Power;
                        if(reaction > 0)
                        {
                            response = "flirtatious_intro";
                        }
                        else
                        {
                            response = "friendly_intro";
                        }
                    }

                    if(response== "flirtatious_intro")
                    {
                        SQ.Subject = response;
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Charisma);
                        SQS.Add(SQ);
                    }

                    if (response == "nod_hello")
                    {
                        SQ.Subject = response;
                        SQ.PowerTrait = PersonalityIdx.Attractiveness;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Charisma);
                        SQS.Add(SQ);
                    }
                    if (response == "friendly_intro")
                    {
                        SQ.Subject = response;
                        SQ.PowerTrait = PersonalityIdx.Charisma;
                        SQ.Power = gsi.GetPersonality(peep, PersonalityIdx.Charisma);
                        SQS.Add(SQ);
                    }
                    if (Q.Subject == "work" || Q.Subject == "complain_about_work" || Q.Subject == "enthuse_about_work")
                    {
                        reaction = 5;
                        float work_attitude = ((5 - gsi.GetPersonality(peep, (PersonalityIdx.Ambition))) * -1) + ((5 - mood) * -1);
                        if (Q.Subject == "complain_about_work") reaction = 10 - gsi.GetPersonality(peep, (PersonalityIdx.Ambition));
                        if (Q.Subject == "enthuse_about_work") reaction = gsi.GetPersonality(peep, (PersonalityIdx.Ambition));
                        if (work_attitude > (Q.Power - 2) && work_attitude < (Q.Power + 2))
                        {
                            //in agreeance

                            SQ.Subject = "agree";
                            SQ.PowerTrait = PersonalityIdx.Charisma;
                            SQ.Power =5;
                            SQS.Add(SQ);
                        }
                        else
                        {
                            reaction = reaction * -1;
                            SQ.Subject = "dissagree";
                            SQ.PowerTrait = PersonalityIdx.Charisma;
                            SQ.Power = -5;
                            SQS.Add(SQ);
                        }
                    }
                    if (Q.Subject == "offer_advice")
                    {
                        if (arrogance > 5 || mood < 5)
                        {
                            //didn't want advice
                            reaction = arrogance * -1;
                        }else
                        {
                            reaction = 5-arrogance;
                        }

                        r1 = r.Next(0, 8);
                        if (gsi.GetPersonality(peep, PersonalityIdx.Friendly) >=5 && gsi.GetPersonality(peep, PersonalityIdx.Honesty) < r1)
                        {
                            SQ.Subject = "thank";
                            SQ.PowerTrait = PersonalityIdx.Charisma;
                            SQ.Power = 5;
                            SQS.Add(SQ);
                        }else
                        {
                            r1 = r.Next(1, 7);
                            if(gsi.GetPersonality(peep, PersonalityIdx.Observation) > r1 && peep.job_skill > other.job_skill)
                            {
                                if (gsi.GetPersonality(peep, PersonalityIdx.Honesty) >= 5)
                                {
                                    SQ.Subject = "dissagree";
                                    SQ.PowerTrait = PersonalityIdx.Charisma;
                                    SQ.Power = -5;
                                    SQS.Add(SQ);
                                }
                            }
                        }
                    }
                    if (Q.Subject == "ask_advice")
                    {
                        reaction = gsi.GetPersonality(peep, PersonalityIdx.Ambition);
                        if(gsi.GetPersonality(peep, PersonalityIdx.Confidence) > (10 - mood))
                        {
                            SQ.Subject = "give_advice";
                            SQ.PowerTrait = PersonalityIdx.Charisma;
                            SQ.Power = 5;
                            SQS.Add(SQ);
                        }
                    }

                    //////////enders///////////
                    if (Q.Subject == "agree" || Q.Subject== "dissagree" || Q.Subject== "thank" || Q.Subject== "give_advice")
                    {
                        reaction = Q.Power;
                    }

                    CharacterRelationship CR = new CharacterRelationship();
                    CR.idkey = peep.Relationships.Count;
                    CR.otherChar = other.idkey;
                    CR.lastAction = Q.Subject + "_response"+"_"+SQ.Subject;
                    peep = AddUpdateRelationship(peep, CR,reaction);
                    //negative-positive attitude adjustment
                    peep.anxiety = peep.anxiety + (reaction/100);
                    peep.happiness = peep.happiness + (reaction / 100);
                    peep = updateNeed(peep, NeedsIdx.Social, 1.0f);
                    
                    //move near - temp positioning
                    //peep.dest_x = gs.ship.Characters[Q.CharId].pos_x + 20;
                    //peep.dest_y = gs.ship.Characters[Q.CharId].pos_y;
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
        public Character AddUpdateRelationship(Character peep, CharacterRelationship CR,float addvalue)
        {

            IList<CharacterRelationship> CRS = new List<CharacterRelationship>();
            bool exists = false;
            foreach (var r in peep.Relationships)
            {
                CharacterRelationship rel = r;
                if(r.otherChar==CR.otherChar)
                {
                    rel.lastAction = CR.lastAction;
                    rel.value = r.value + addvalue;
                    exists = true;
                }
                CRS.Add(rel);
            }
            if (exists == false)
            {
                CR.value = addvalue;
                CRS.Add(CR);
            }
            peep.Relationships = CRS;
            return peep;
        }


    }

}