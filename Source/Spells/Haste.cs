
using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            Name = "Haste";
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Level = 3;
        }

        public override bool Cast()
        {

            if (Game1.buffsDisplay.hasBuff(2))
            {
                return false;
            }
            else
            {
                Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
                return true;
            }

        }

    }
}
