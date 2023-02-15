using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Blasting : Spell
    {
        public Blasting() : base()
        {
            School = School.Evocation;
            Description = "Creates an explosion at a target location.";
            Level = 5;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            Game1.currentLocation.explode(Target, 2, Game1.player);
            return true;
        }
    }
}