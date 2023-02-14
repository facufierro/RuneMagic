using RuneMagic.Source.Interfaces;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Translocation : Spell
    {
        public Translocation() : base()
        {
            Name = "Translocation";
            School = School.Evocation;
            Description = "The caster changes position with a target living creature.";
            Level = 2;
        }

        public override bool Cast()
        {
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);
            if (Target != null)
            {
                (Game1.player.Position, Target.Position) = (Target.Position, Game1.player.Position);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}