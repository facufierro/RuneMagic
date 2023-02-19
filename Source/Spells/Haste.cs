using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Haste : Spell
    {
        public Haste() : base(School.Abjuration)
        {
            Description += "Increases the caster's movement speed.";
            Level = 3;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Hastened>().Any())
            {
                Effect = new Hastened(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}