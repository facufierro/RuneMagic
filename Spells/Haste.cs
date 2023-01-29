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
    public class Haste : Spell
    {
        public Haste() : base()
        {
            Name = "Haste";
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Level = 1;
        }

        public override bool Cast()
        {
            //check displayed buff by source



            Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
            return true;
        }

    }
}
