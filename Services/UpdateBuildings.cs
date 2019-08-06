using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;


namespace Project.Services
{
    public class UpdateBuildings
    {

        public List<Building> AddModifier(GameState gs, int buildingId, string modifier,float value, int expires, bool discovered, bool AddValue)
        {
            List<Building> buildings = new List<Building>();
            var found = false;
            foreach (var building in gs.ship.Buildings)
            {
                var b = building;
                if (b.building_id == buildingId)
                {
                    List<Modifier> mods = new List<Modifier>();
                    foreach (var mod in b.Modifiers)
                    {
                        if (mod.name == modifier)
                        {
                            //update current
                            if (AddValue == false)
                            {
                                mod.value = value;
                            }else
                            {
                               mod.value = mod.value + value;
                            }
                            if (mod.value < 0) mod.value = 0;
                            mod.expire = expires;
                            found = true;
                        }
                        mods.Add(mod);
                    }
                    if (found == false)
                    {
                        //or add
                        Modifier mod = new Modifier();
                        mod.name = modifier;
                        mod.value = value;
                        mod.expire = expires;
                        mod.discovered = discovered;
                        mods.Add(mod);
                    }
                    b.Modifiers = mods;

                }
                buildings.Add(b);
            }
            return buildings;
        }

        public List<Building> UpdateModifiers(GameState gs)
        {
            //add string modifier if you want to remove mod directly.
            List<Building> buildings = new List<Building>();
            foreach (var building in gs.ship.Buildings)
            {
                var b = building;
                List<Modifier> mods = new List<Modifier>();
                foreach (var mod in b.Modifiers)
                {
                    if (mod.expire == 0) continue;
                    if(mod.expire > 0 ) mod.expire = mod.expire - 1;
                    if (mod.expire != 0)
                    {
                        mods.Add(mod);
                    }
                    if (mod.name == "health") b.health = b.health + mod.value;
                }
                b.Modifiers = mods;
                buildings.Add(b);
            }
            return buildings;
        }
        public Modifier GetModifier(Building building, string modifier)
        {
            Modifier m = null;
            foreach (var mod in building.Modifiers)
            {
                if (mod.name == modifier) return mod;
            }
            return m;
        }

        public GameState TickUpdate(GameState gs)
        {
            gs.ship.Buildings = UpdateModifiers(gs);
            List<Building> buildings = new List<Building>();
            foreach (var building in gs.ship.Buildings)
            {
                var b = building;
                var output = building.health / 100;
                //var bt = gsi.GetBuildingType(gs,building.building_id);
                b.health = b.health - .25f;
                float prod = 0;

                //output here///
                if (b.building_id == 1) gs.ship.oxygen = (int)Math.Round(gs.ship.oxygen + (output * 100));
                if (b.building_id == 2) gs.ship.carbondioxide = gs.ship.carbondioxide - output;
                if (b.building_id == 3)
                {
                    //hydroponics
                    if(gs.ship.dirty_water > 0)
                    {
                        gs.ship.carbondioxide = gs.ship.carbondioxide - output;
                        gs.ship.oxygen = gs.ship.oxygen + (int)Math.Round(output * 50);
                        gs.ship.dirty_water = gs.ship.dirty_water + .98f;
                        gs.ship.Buildings = AddModifier(gs, 5, "hydro_production", output, 2, true, false);
                    }
                }
                if (b.building_id == 4)
                {
                    //composter
                    if(gs.ship.organics > 1)
                    {
                        prod = output;
                        gs.ship.organics = gs.ship.organics - (prod *1.1f);
                    }
                    gs.ship.Buildings = AddModifier(gs, 5, "soil", prod, -1, true,true);
                }
                if (b.building_id == 5)
                {
                    //greenhouse
                    gs.ship.water = gs.ship.water - 1;
                    gs.ship.dirty_water = gs.ship.dirty_water + .75f;
                    gs.ship.oxygen = gs.ship.oxygen + (int)Math.Round(output * 10);
                }
                if (b.building_id == 6)
                {
                    //sewage
                    if(gs.ship.dirty_water > 100)
                    {
                        prod = 1 + output;
                        gs.ship.dirty_water = gs.ship.dirty_water - (prod * .75f);
                        gs.ship.organics = gs.ship.organics + (prod * .25f);
                        gs.ship.Buildings = AddModifier(gs, 12, "pending_water", prod, -1, true, true);
                    }
                }
                if (b.building_id == 7)
                {
                    //thrusters
                }
                if (b.building_id == 8)
                {
                    //Engine block
                }
                if (b.building_id == 9)
                {
                    //Auxiliary Thrusters
                }
                if (b.building_id == 10)
                {
                    //Engine Cooling System
                }
                if (b.building_id == 11)
                {
                    //Power Station
                    int max_power = (int)Math.Round(b.health * 10);
                    if (gs.ship.power > max_power) gs.ship.power = max_power;
                }
                if (b.building_id == 12)
                {
                    //Water Recycling

                    Modifier mod = GetModifier(building, "pending_water");
                    if (mod != null)
                    {
                        if(mod.value > 1)
                        {
                            prod = output;
                            //if (prod > 1) prod = 1;
                            gs.ship.dirty_water = gs.ship.dirty_water - 1;
                            gs.ship.water = gs.ship.water + prod;
                            gs.ship.organics = gs.ship.organics + prod * .25f;
                            gs.ship.Buildings = AddModifier(gs, 12, "pending_water", prod * -1, -1, true, true);
                        }
                    }
                }
                if (b.building_id == 13)
                {
                    //external Water Harvester
                    gs.ship.water = gs.ship.water + output;
                }
                if (b.building_id == 14)
                {
                    //Cafeteria
                    //gs.ship.Buildings = AddModifier(gs, 14, "dirty_plates", 1, -1, true, true);
                }
                if (b.building_id == 15)
                {
                    //restuauturant
                }
                if (b.building_id == 16)
                {
                    //music club
                }
                if (b.building_id == 17)
                {
                    //Arcade
                }
                if (b.building_id == 18)
                {
                    //Game room
                }
                if (b.building_id == 19)
                {
                    //lounge
                }
                if (b.building_id == 20)
                {
                    //movie theater
                }
                if (b.building_id == 21)
                {
                    //bar
                }
                if (b.building_id == 22)
                {
                    //dorm
                }
                if (b.building_id == 47)
                {
                    //Battery

                }


                buildings.Add(b);
            }
            gs.ship.Buildings = buildings;
            return gs;
        }

        public List<Building> UpdateHygene(GameState gs, int buildingId, float value)
        {

            List<Building> buildings = new List<Building>();
            foreach (var building in gs.ship.Buildings)
            {
                var b = building;
                if(b.building_id==buildingId) b.hygene = b.hygene - value;
                buildings.Add(b);
            }

            return buildings;
        }

    }
}
