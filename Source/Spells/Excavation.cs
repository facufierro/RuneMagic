using RuneMagic.Source.Effects;
using StardewValley;
using StardewValley.Locations;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Excavation : Spell
    {
        public Excavation() : base(School.Conjuration)
        {
            Description += "The caster digs a hole at a target location."; Level = 5;
        }

        public override bool Cast()
        {
            if (!Player.MagicStats.ActiveEffects.OfType<ExcavationEffect>().Any())
            {
                Effect = new ExcavationEffect(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}