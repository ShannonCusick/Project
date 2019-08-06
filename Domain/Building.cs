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
        public float health { get; set; }
        public float hygene { get; set; }
        public int upgrades { get; set; }
        public IList<Modifier> Modifiers { get; set; }
        

    }
}
