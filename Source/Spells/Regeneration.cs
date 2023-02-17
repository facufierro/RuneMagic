using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Regeneration : Spell
    {
        public Regeneration() : base()
        {
            School = School.Enchantment;
            Description = "Slowly regenerates the caster's Stamina.";
            Level = 4;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Regenerating>().Any())
            {
                Effect = new Regenerating(Name);
                return true;
            }
            else
                return false;
        }
    }
}