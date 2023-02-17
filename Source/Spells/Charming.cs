using RuneMagic.Source.Effects;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Charming : Spell
    {
        public Charming() : base()
        {
            Description = "Charms the target for a short time, when the effect ends the target will be annoyed";
            School = School.Illusion;
            Level = 2;
        }

        public override bool Cast()
        {
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Charmed>().Any())
            {
                Effect = new Charmed(Name, Target);
                return true;
            }
            else
                return false;
        }
    }
}