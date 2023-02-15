using StardewValley;
using StardewValley.TerrainFeatures;

namespace RuneMagic.Source.Spells
{
    public class Labor : Spell
    {
        public Labor() : base()
        {
            Name = "Labor";
            School = School.Conjuration;
            Description = "Creates a hoe dirt tile at the cursor.";
            Level = 2;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;

            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(Target))
            {
                Game1.currentLocation.terrainFeatures.Add(Target, new HoeDirt());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}