using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells.Enchantment
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            Type = SpellType.Buff;
            School = MagicSchool.Enchantment;
        }

        public override bool Cast()
        {
            //check displayed buff by source



            Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
            return true;
        }

    }
}
