using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project.Domain
{
    public static class ShowShip
    {
        static ShowShip()
        {

            string json = File.ReadAllText(@"savegame/game.json");
            Ship ship = JsonConvert.DeserializeObject<Ship>(json);
        }
        private static Ship ship;
        
        public static Ship GetShip()
        {
            return ship;
        }
    }
}
