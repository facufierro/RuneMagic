using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Vitality : Spell
    {
        public Vitality() : base()
        {
            School = School.Enchantment;
            Description = "Increases the casters Health and Stamina for a long period of time.";
            Level = 8;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Vitalized>().Any())
            {
                Effect = new Vitalized(Name);
                return true;
            }
            else
                return false;
        }
    }
}