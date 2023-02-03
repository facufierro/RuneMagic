
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
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
            Level = 1;

        }
        public override bool Cast()
        {
            var Cursor = Game1.currentCursorTile;

            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(Cursor))
            {
                Game1.currentLocation.terrainFeatures.Add(Cursor, new HoeDirt());
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
