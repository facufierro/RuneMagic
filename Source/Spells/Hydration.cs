﻿using SpaceCore;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Hydration : Spell
    {
        public Hydration() : base(School.Conjuration)
        {
            Description += "Water a tile at the cursor.";
            Level = 1;
        }

        public override bool Cast()
        {
            var tool = new WateringCan();
            var potency = 1 + (Player.MagicStats.ActiveSchool.Level - Level) / 4;
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, potency, Game1.player);
            return base.Cast();
        }
    }
}