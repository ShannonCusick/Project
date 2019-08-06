using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    [Serializable]
    public class Ship
    {
        public string name { get; set; }
        public int phase { get; set; }
        public int location { get; set; }
        public int destination { get; set; }
        public int to_destination { get; set; }
        public int food { get; set; }
        public int meals { get; set; }
        public float water { get; set; }
        public int fuel { get; set; }
        public int power { get; set; }
        public int spare_parts { get; set; }
        public int waste_parts { get; set; }
        public int medical { get; set; }
        public int raw_resources { get; set; }
        public float organics { get; set; }
        public float dirty_water { get; set; }
        public int oxygen { get; set; }
        public int nitrogen { get; set; }
        public int hydrogen { get; set; }
        public float carbondioxide { get; set; }
        public IList<Modifier> Modifiers { get; set; }
        public IList<Building> Buildings { get; set; }
        public IList<Character> Characters { get; set; }
        public IList<Law> Laws { get; set; }

        
        public class Law
        {
            public int idkey { get; set; }
            public string name { get; set; }
        }
    }
}
