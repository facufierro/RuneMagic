using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Strength : Spell
    {
        public Strength() : base()
        {
            School = School.Enchantment;
            Description = "Increases the caster's attack damage.";
            Level = 5;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Strengthened>().Any())
            {
                Effect = new Strengthened(Name);
                return true;
            }
            else
                return false;
        }
    }
}