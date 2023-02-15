using StardewValley;
using StardewValley.TerrainFeatures;

namespace RuneMagic.Source.Spells
{
    public class Hydration : Spell
    {
        public Hydration() : base()
        {
            School = School.Conjuration;
            Description = "Water a tile at the cursor.";
            Level = 2;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            if (Game1.currentLocation.terrainFeatures[Target] is HoeDirt dirt)
            {
                dirt.state.Value = HoeDirt.watered;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}