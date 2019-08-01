using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    public class PersonalityIdx
    {
        public const int Leadership = 0;
        public const int Intelligence = 1;
        public const int Charisma = 2;
        public const int Attractiveness = 3;
        public const int Productivity = 4;
        public const int Creativity = 5;
        public const int Confidence = 6;
        public const int Outgoing = 7;
        public const int Persuasion = 8;
        public const int Empathy = 9;
        public const int Adaptability = 10;
        public const int Patience = 11;
        public const int Courage = 12;
        public const int Observation = 13;
        public const int Trust = 14;
        public const int Ambition = 15;
        public const int Logic = 16;
        public const int Honesty = 17;
        public const int Friendly =18;
        public const int Cunning = 19;
        public const int Other = 20;
    }
    public class NeedsIdx
    {
        public const int Excercize = 0;
        public const int Safety = 1;
        public const int Food = 2;
        public const int Water = 3;
        public const int Hygene = 4;
        public const int Comfort = 5;
        public const int Sleep = 6;
        public const int Bio = 7;
        public const int Social = 8;

    }
  public class Character
    {
        public int idkey { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int gender { get; set; }
        public int spouse_id { get; set; }
        public int mother_id { get; set; }
        public int father_id { get; set; }
        public int job_id { get; set; }
        public int job_skill { get; set; }
        public int dorm_id { get; set; }
        public int sleep_shift { get; set; }
        //mood
        public float mood { get; set; }
        public float stress { get; set; }
        public float happiness { get; set; }
        public float anxiety { get; set; }
        public float strength { get; set; }
        public float reflexes { get; set; }
        public float constitution { get; set; }
        public float stamina { get; set; }
        public float sanity { get; set; }
        //
        public IList<CharacterPersonality> Personality{ get; set; }
        public IList<CharacterNeeds> Needs{ get; set; }
        public IList<CharacterWants> Wants { get; set; }
        public IList<CharacterModifiers> Modifiers { get; set; }
        public IList<CharacterMemories> Memories { get; set; }
        public IList<CharacterRelationship> Relationships { get; set; }
        //
        public int pos_x { get; set; }
        public int pos_y { get; set; }
        public string current_task { get; set; }
        public int dest_x { get; set; }
        public int dest_y { get; set; }
        public bool hired { get; set; }
        public bool dead { get; set; }
    }
    public class CharacterPersonality
    {
        public int idkey { get; set; }
        public float value { get; set; }
        public float value_mods { get; set; }
        public bool discovered { get; set; }
    }
    public class CharacterNeeds
    {
        public int idkey { get; set; }
        public float value { get; set; }
        public float value_mods { get; set; }
    }
    public class CharacterWants
    {
        public int idkey { get; set; }
        public string name { get; set; }
        public float value { get; set; }
    }
    public class CharacterModifiers
    {
        public int idkey { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public int expire { get; set; }
        public bool discovered { get; set; }
    }
    public class CharacterMemories
    {
        public int idkey { get; set; }
        public string name { get; set; }
        public string phase { get; set; }
        public string location { get; set; }
        public string building { get; set; }
        public string otherChar { get; set; }
        public string value { get; set; }
    }
    public class CharacterRelationship
    {
        public int idkey { get; set; }
        public int otherChar { get; set; }
        public float value { get; set; }
    }

}
