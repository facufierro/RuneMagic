﻿using StardewValley.TerrainFeatures;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RuneMagic.assets.Spells
{
    public class Hydration : Spell
    {
        public Hydration()
        {
        }
        public override void Cast()
        {
            if (Game1.currentLocation.terrainFeatures.ContainsKey(Cursor) && Game1.currentLocation.terrainFeatures[Cursor] is HoeDirt)
            {
                HoeDirt dirt = (HoeDirt)Game1.currentLocation.terrainFeatures[Cursor];
                dirt.state.Value = HoeDirt.watered;

            }
        }
    }
}
