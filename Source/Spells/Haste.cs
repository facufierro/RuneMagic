
using SpaceCore;
using StardewValley;
using StardewValley.Menus;
using System.Linq;
using System.Threading;

namespace RuneMagic.Source.Spells
{
    public class Haste : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
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
            var buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 10, "Glyph of Haste", "Glyph of Haste");
            //if the player already has the buff, refresh it
            if (Game1.buffsDisplay.otherBuffs.Any(b => b.which == buff.which))
            {
                Game1.buffsDisplay.otherBuffs.First(b => b.which == buff.which).millisecondsDuration = buff.millisecondsDuration;
                return true;
            }
            else
            {
                Game1.buffsDisplay.addOtherBuff(buff);
                return true;
            }
        }
    }
}
