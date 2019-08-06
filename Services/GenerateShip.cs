using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Domain;

namespace Project.Services
{
    class GenerateShip
    {
        public Ship CreateShip(string name)
        {
            Ship ship = new Ship();
            ship.name = name;
            ship.location = 0;
            ship.destination = 1;
            ship.to_destination = 0;
            ship.food = 500;
            ship.meals = 100;
            ship.water = 10000;
            ship.fuel = 8000;
            ship.power = 1000;
            ship.spare_parts = 1000;
            ship.waste_parts = 0;
            ship.medical = 1000;
            ship.raw_resources = 1000;
            ship.organics = 50;
            ship.dirty_water = 0;
            ship.oxygen = 50000;
            ship.hydrogen = 0;
            ship.carbondioxide = 0;
            ship.nitrogen = 150000;
            int i = 0;
            ship.Buildings = new List<Building>();
            foreach (var b in ShowBuildingTypes.GetBuildings())
            {
                var newbuild = new Building();
                newbuild.idkey = i;
                newbuild.building_id = b.idkey;
                newbuild.health = 100;
                newbuild.hygene = 100;
                newbuild.upgrades = 0;
                newbuild.Modifiers = new List<Modifier>();
                ship.Buildings.Add(newbuild);
                i = i + 1;
            }
            ship.Modifiers = new List<Modifier>();
            ship.Characters = new List<Character>();
            ship.Laws = new List<Ship.Law>();


            return ship;
        }
    }
}
