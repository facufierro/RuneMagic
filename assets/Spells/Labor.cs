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
        public override void Cast()
        {
            //get tile at cursor location
            Cursor = Game1.currentCursorTile;
            //if tile is tillable
            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(Cursor))
            {
                //till tile
                Game1.currentLocation.terrainFeatures.Add(Cursor, new HoeDirt());
            }

        }
    }
}
