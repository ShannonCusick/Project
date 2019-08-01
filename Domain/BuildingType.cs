using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    //Building types
    public class BuildingType
    {
        //[JsonProperty(PropertyName = "firstname")]
        public int idkey { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int capacity { get; set; }
        public string shapename { get; set; }
        public int power_required { get; set; }
        public int space_req { get; set; }
        public int def_x { get; set; }
        public int def_y { get; set; }

        //  public void DataLoad()
        //    {
        //     }
    }



}
