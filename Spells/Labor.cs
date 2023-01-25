using StardewValley.TerrainFeatures;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Spells
{
    public class Labor : Spell
    {
        public Labor() : base()
        {
            Name = "Labor";
            School = School.Conjuration;
            Description = "Creates a hoe dirt tile at the cursor.";
            Glyph = "assets/Textures/Alteration/Labor.png";
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
