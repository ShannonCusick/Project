using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain
{
    public class Modifier
    {
        public int idkey { get; set; }
        public string name { get; set; }
        public float value { get; set; }
        public int expire { get; set; }
        public bool discovered { get; set; }

    }
}
