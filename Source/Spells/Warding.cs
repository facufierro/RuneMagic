using Microsoft.Xna.Framework;
using RuneMagic.Source.SpellEffects;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Warding : Spell
    {
        public Warding()
        {
            Name = "Warding";
            Description = "Protects the caster from damage for a short period of time.";
            School = School.Abjuration;
            CastingTime = 1.0f;
            Level = 1;
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