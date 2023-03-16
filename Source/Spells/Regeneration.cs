using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Regeneration : Spell
    {
        public Regeneration() : base(School.Abjuration)
        {
            Description += "Slowly regenerates the caster's Stamina."; Level = 4;
        }

        public override bool Cast()
        {
            if (!Player.MagicStats.ActiveEffects.OfType<Regenerating>().Any())
            {
                Effect = new Regenerating(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}