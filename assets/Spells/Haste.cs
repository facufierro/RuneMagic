using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Haste : Spell
    {
        public Haste()
        {
            Type = SpellType.Buff;
            School = MagicSchool.Enchanting;
        }

        public override void Cast()
        {
            Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
        }

    }
}
