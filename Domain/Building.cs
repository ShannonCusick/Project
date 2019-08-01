using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    public class Building
    {
        public int idkey { get; set; }
        public int building_id { get; set; }
        public int health { get; set; }
        public int hygene { get; set; }
        public int upgrades { get; set; }
        public IList<BuildingModifiers> Modifiers { get; set; }

        public class BuildingModifiers
        {
            public int idkey { get; set; }
            public string name { get; set; }
            public float value { get; set; }
            public int expire { get; set; }
            public bool discovered { get; set; }
        }

    }
}
