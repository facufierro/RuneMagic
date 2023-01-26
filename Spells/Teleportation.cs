using RuneMagic.Famework;
using RuneMagic.Magic;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Spells
{
    public class Teleportation : Spell
    {
        private bool UsedToday = false;

        public Teleportation() : base()
        {
            Name = "Teleportation";
            School = School.Alteration;
            Description = "Teleports the caster to their home.";
            Glyph = "assets/Textures/Alteration/Teleportation.png";
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
