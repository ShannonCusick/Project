using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project.Domain
{
    public class ShowBuildingTypes
    {
        static ShowBuildingTypes()
        {

            string json = File.ReadAllText(@"data/BuildingTypes.json");
            BuildingCollection BuildingList = JsonConvert.DeserializeObject<BuildingCollection>(json);
            AllBuildings = BuildingList.Buildingtypes;
            //using (StreamReader r = new StreamReader("data/BuildingTypes.json"))
            //{
            //    string json = r.ReadToEnd();
            //    BuildingCollection BuildingList = JsonConvert.DeserializeObject<BuildingCollection>(json);
            //    AllBuildings = BuildingList.Buildingtypes;
            //}
        }
        private static List<BuildingType> allBuildings;

        public static List<BuildingType> AllBuildings
        {
            get
            {
                return allBuildings;
            }

            set
            {
                allBuildings = value;
            }
        }

        public static List<BuildingType> GetBuildings()
        {
            return AllBuildings;
        }
        public static BuildingType GetBuildingType(int id){
            BuildingType btype = new BuildingType();
            foreach(BuildingType BT in AllBuildings){
                if(BT.idkey==id){
                    return BT;
                }
            }
            return btype;
        }
    }
}
