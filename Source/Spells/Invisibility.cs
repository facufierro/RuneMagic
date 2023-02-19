using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : Spell
    {
        public Invisibility() : base(School.Alteration)
        {
            Description += "Makes the caster invisible";
            Level = 4;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Invisible>().Any())
            {
                Effect = new Invisible(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}