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
        public Teleportation()
        {
            Type = SpellType.Active;
            School = MagicSchool.Alteration;
        }
        
        public override bool Cast()
        {
            if (!UsedToday)
            {
                Game1.warpFarmer("FarmHouse", 4, 3, false);
                UsedToday = true;
                return true;
            }
            else
            {
                Game1.drawObjectDialogue("I can't use this spell again today.");
                return false;
            }

        }
    }
}
