using Microsoft.Xna.Framework;
using RuneMagic.Source.Interfaces;
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
            Buff = new Buff(Buff.yobaBlessing)
            {
                millisecondsDuration = Duration * 1000,
                description = Description,
                source = Name,
                displaySource = Name,
                glow = Color.Transparent,
                sheetIndex = 16,
            };
            Game1.buffsDisplay.addOtherBuff(Buff);
            return true;
        }
    }
}