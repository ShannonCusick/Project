using System;
using System.Collections.Generic;
using Project.Domain;
using System.IO;


namespace Project.Services
{
    public class UpdateShip
    {
        public IList<Modifier> AddModifier(GameState gs, string modifier, float value, int expires, bool discovered, bool AddValue)
        {
            var found = false;

            List<Modifier> mods = new List<Modifier>();
            foreach (var mod in gs.ship.Modifiers)
            {
                if (mod.name == modifier)
                {
                    //update current
                    if (AddValue == false)
                    {
                        mod.value = value;
                    }
                    else
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
            return mods;
        }
        public IList<Modifier> UpdateModifiers(GameState gs)
        {
            //add string modifier if you want to remove mod directly.
            List<Modifier> mods = new List<Modifier>();
            foreach (var mod in gs.ship.Modifiers)
            {
                if (mod.expire > 0) mod.expire = mod.expire - 1;
                if (mod.expire != 0)
                {
                    mods.Add(mod);
                }
            }
            return mods;
        }
        public Modifier GetModifier(GameState gs, string modifier)
        {
            Modifier m = null;
            foreach (var mod in gs.ship.Modifiers)
            {
                if (mod.name == modifier) return mod;
            }
            return m;
        }

        public GameState BurnEngines(GameState gs)
        {
            if (gs.ship.to_destination == 0)
            {
                gs.ship.to_destination = 1;
                gs.ship.Modifiers = AddModifier(gs, "in_burn", 2, 9, true, false);
            }
            return gs;
        }

        public GameState ShipBurn(GameState gs,Modifier mod)
        {
            //5 tic of burn, after 3 waiting
            if(mod.expire <= 5)
            {
                int fuelcost = 125;
                if (gs.ship.fuel >= 0)
                {
                    gs.ship.fuel = gs.ship.fuel - fuelcost;
                    UpdateBuildings ubs = new UpdateBuildings();
                    ubs.AddModifier(gs, 8, "health", -15, 1, true, false);
                }
            }
            if (mod.expire <=1)
            {
                gs.ship.to_destination = gs.ship.to_destination + 1;
            }
            return gs;
        }
        public GameState TickUpdate(GameState gs)
        {
            gs.ship.Modifiers = UpdateModifiers(gs);
            if (gs.ship.to_destination != 0)
            {
                gs.ship.location = gs.ship.location + 1;
            }
            return gs;
        }

    }
}
