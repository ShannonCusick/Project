using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Domain;

namespace Project.Services
{
    public class GenerateCharacter
    {
        public static Random r = new Random();

        public Character CreateCharacter(Ship ship,int job_id, int mother_id = 0, int father_id = 0, string last_name = null)
        {
            Character NewChar = new Character();

            int gender = r.Next(1, 100);
            if (gender < 50)
            { 
                NewChar.gender = 0;
            }else
            {
                NewChar.gender = 1;
            }
            if (last_name == null)
            {
                last_name = ShowCharacterNames.GetName(0, NewChar.gender);
            }
            NewChar.idkey = ship.Characters.Count;
            string first_name = ShowCharacterNames.GetName(1, NewChar.gender);
            NewChar.name = first_name + " " + last_name;
            NewChar.age = r.Next(864, 1728);
            NewChar.spouse_id = 0;
            NewChar.mother_id = mother_id;
            NewChar.father_id = father_id;
            NewChar.job_id = job_id;
            NewChar.job_skill = r.Next(1,7);
            NewChar.dorm_id = 0;
            NewChar.sleep_shift = 0;
            NewChar.stress = (float)5;
            NewChar.happiness = (float)5;
            NewChar.anxiety = (float)5;
            int val = r.Next(3, 7);
            NewChar.strength = (float)val;
            val = r.Next(3, 7);
            NewChar.reflexes = (float)val;
            val = r.Next(3, 8);
            NewChar.constitution = (float)val;
            NewChar.stamina = (float)5;
            NewChar.sanity = (float)5;
            NewChar.mood= (float)5;
            NewChar.Personality = new List<CharacterPersonality>();
            foreach (var p in ShowPersonalityTypes.GetPersonalitys())
            {
                var newper = new CharacterPersonality();
                val = r.Next(0, 10);
                if(val > 8)
                {
                    val= r.Next(0, 10);
                }
                if (val < 2)
                {
                    val = r.Next(0, 10);
                }
                newper.idkey =(p.idkey - 1);
                newper.value = (float)val;
                newper.value_mods = (float)0;
                if(val >  8 || val < 2)
                {
                    newper.discovered = true;
                }else
                {
                    newper.discovered = false;
                }
                NewChar.Personality.Add(newper);
            }
            NewChar.Needs= new List<CharacterNeeds>();
            for (int i = 0; i < 9; i++)
            {
                var newneed = new CharacterNeeds();
                newneed.idkey = i;
                newneed.value = (float)r.Next(5, 10);
                newneed.value_mods = (float)0;
                NewChar.Needs.Add(newneed);
            }
            NewChar.Wants = new List<CharacterWants>();
            NewChar.Modifiers = new List<CharacterModifiers>();
            NewChar.Memories = new List<CharacterMemories>();
            NewChar.Relationships = new List<CharacterRelationship>();
            NewChar.pos_x = 0;
            NewChar.pos_y = 0;
            NewChar.current_task = "dorm";
            NewChar.dest_x = 0;
            NewChar.dest_y = 0;
            NewChar.hired = false;
            NewChar.dead = false;

            ship.Characters.Add(NewChar);
            GameService gs = new GameService();
            gs.SaveGame(ship);
            return NewChar;
        }
    }
}
