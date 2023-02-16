using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Labor : Spell
    {
        public Labor() : base()
        {
            School = School.Conjuration;
            Description = "Creates a hoe dirt tile at the cursor.";
            Level = 2;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            var tool = new Hoe();
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, 1, Game1.player);
            return true;
        }
    }
}