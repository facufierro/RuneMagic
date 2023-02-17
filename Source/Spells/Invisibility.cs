using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : Spell
    {
        public Invisibility() : base()
        {
            School = School.Illusion;
            Description = "Makes the caster invisible";
            Level = 8;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Invisible>().Any())
            {
                Effect = new Invisible(Name);
                return true;
            }
            else
                return false;
        }
    }
}