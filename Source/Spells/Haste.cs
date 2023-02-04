
using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Haste : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public int ProjectileNumber { get; set; }
        public Haste() : base()
        {
            Name = "Haste";
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            CastingTime = 1;
            Level = 3;
        }

        public bool Cast()
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
