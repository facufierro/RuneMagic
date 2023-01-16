using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Teleportation : Spell
    {
        public Teleportation() { }

        public override void Cast()
        {
            if (!UsedToday)
            {
                Game1.warpFarmer("FarmHouse", 4, 2, false);
                UsedToday = true;
            }
            else
            {
                Game1.drawObjectDialogue("I can't use this spell again today.");
            }

        }
    }
}
