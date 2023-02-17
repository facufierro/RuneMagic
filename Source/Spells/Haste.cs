using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Level = 4;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Hastened>().Any())
            {
                Effect = new Hastened(Name);
                return true;
            }
            else
                return false;
        }
    }
}