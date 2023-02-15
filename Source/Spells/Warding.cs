using Microsoft.Xna.Framework;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Warding : Spell
    {
        public Warding() : base()
        {
            Description = "Protects the caster from damage for a short period of time.";
            School = School.Abjuration;
            CastingTime = 1.0f;
            Level = 7;
        }

        public override bool Cast()
        {
            var buff = new Buff(Buff.yobaBlessing)
            {
                millisecondsDuration = 5 * 1000,
                glow = Color.Transparent,
                sheetIndex = 9000,
            };
            Game1.buffsDisplay.addOtherBuff(buff);
            return true;
        }
    }
}