using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project.Domain
{
    public class ShowCharacterNames
    {
         static ShowCharacterNames()
        {

            string json = File.ReadAllText(@"data/CharacterNames.json");
            CharacterNamesCollection NamesList = JsonConvert.DeserializeObject<CharacterNamesCollection>(json);
            AllNames= NamesList.Names;
        }
        private static List<CharacterNames> allNames;

        public static List<CharacterNames> AllNames
        {
            get
            {
                return allNames;
            }

            set
            {
                allNames = value;
            }
        }
        public static Random r = new Random();

        public static string GetName(int type, int gender)
        {
            string name = "noname";
            if (type == 1)
            {
                if (gender == 1)
                {
                    int t= r.Next(1, 1054);
                    name = AllNames[t].first_name;
                }
                else
                {
                    int t = r.Next(1056, 1845);
                    name = AllNames[t].first_name;
                }
            }else
            {
                int t = r.Next(1847, 6604);
                name = AllNames[t].last_name;
            }
            return name;
        }
    }
}
