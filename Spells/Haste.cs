using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Spells
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            Name = "Haste";
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Glyph = "assets/Textures/Alteration/Haste.png";
        }

        public override bool Cast()
        {
            //check displayed buff by source



            Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
            return true;
        }

    }
}
