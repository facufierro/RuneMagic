using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Charming : Spell
    {
        public Charming() : base(School.Alteration)
        {
            Description += "Charms the target for a period time."; Level = 2;
        }

        public override bool Cast()
        {
            var target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);
            if (!Player.MagicStats.ActiveEffects.OfType<Charmed>().Any())
            {
                Effect = new Charmed(this, target);
                return base.Cast();
            }
            else
                return false;
        }
    }
}