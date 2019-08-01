using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project.Domain
{
    public class ShowPersonalityTypes
    {
        static ShowPersonalityTypes()
        {

            string json = File.ReadAllText(@"data/PersonalityTypes.json");
            PersonalityCollection PersonalityList = JsonConvert.DeserializeObject<PersonalityCollection>(json);
            AllPersonalitys = PersonalityList.Personalitytypes;
        }
        private static List<PersonalityType> allPersonalitys;

        public static List<PersonalityType> AllPersonalitys
        {
            get
            {
                return allPersonalitys;
            }

            set
            {
                allPersonalitys = value;
            }
        }

        public static List<PersonalityType> GetPersonalitys()
        {
            return AllPersonalitys;
        }
        public static PersonalityType GetPersonality(int idkey)
        {
            return AllPersonalitys[idkey];
        }
    }
}
