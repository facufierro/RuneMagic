using StardewValley.TerrainFeatures;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Labor : Spell
    {
        public Labor()
        {
            Type = SpellType.Active;
            School = MagicSchool.Conjuration;
        }
        public override bool Cast()
        {
            Cursor = Game1.currentCursorTile;
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
