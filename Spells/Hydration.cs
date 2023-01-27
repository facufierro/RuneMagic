using StardewValley.TerrainFeatures;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RuneMagic.Magic;
using RuneMagic.Famework;

namespace RuneMagic.Spells
{
    public class Hydration : Spell
    {
        public Hydration() : base()
        {
            Name = "Hydration";
            School = School.Conjuration;
            Description = "Water a tile at the cursor.";
            Level = 4;
        }
        public override bool Cast()
        {
            var Cursor = Game1.currentCursorTile;
            if (Game1.currentLocation.terrainFeatures.ContainsKey(Cursor) && Game1.currentLocation.terrainFeatures[Cursor] is HoeDirt)
            {
                HoeDirt dirt = (HoeDirt)Game1.currentLocation.terrainFeatures[Cursor];
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
