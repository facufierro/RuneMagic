using Microsoft.Xna.Framework;
using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

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
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Warded>().Any())
            {
                Effect = new Warded(Name);
                return true;
            }
            else
                return false;
        }
    }
}