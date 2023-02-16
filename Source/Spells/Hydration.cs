using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Hydration : Spell
    {
        public Hydration() : base()
        {
            School = School.Conjuration;
            Description = "Water a tile at the cursor.";
            Level = 3;
        }

        public override bool Cast()
        {
            var tool = new WateringCan();
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, 1, Game1.player);
            return true;
        }
    }
}