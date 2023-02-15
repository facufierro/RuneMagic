using Microsoft.Xna.Framework;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Blasting : Spell
    {
        public Blasting() : base()
        {
            Name = "Blasting";
            School = School.Evocation;
            Description = "Creates an explosion at a target location.";
            Level = 1;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            Game1.currentLocation.explode(Target, 1, Game1.player);
            return true;
        }
    }
}