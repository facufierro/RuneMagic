
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
            Level = 2;
        }

        public override bool Cast()
        {
            //check displayed buff by source



            Game1.buffsDisplay.addOtherBuff(new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 10, "Glyph of Haste", "Glyph of Haste"));
            return true;
        }

    }
}
