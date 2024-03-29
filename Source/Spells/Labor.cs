﻿using SpaceCore;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Labor : Spell
    {
        public Labor() : base(School.Conjuration)
        {
            Description += "Creates a hoe dirt tile at the cursor."; Level = 1;
        }

        public override bool Cast()
        {
            var target = Game1.currentCursorTile;
            var tool = new Hoe();
            var potency = 1 + (School.Level - Level) / 4;
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, potency, Game1.player);
            return base.Cast();
        }
    }
}