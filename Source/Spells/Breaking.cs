﻿using Microsoft.Xna.Framework;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Breaking : Spell
    {
        public Breaking() : base(School.Conjuration)
        {
            Description += "Conjures a Pickaxe at the target location."; Level = 2;
        }

        public override bool Cast()
        {
            var target = Game1.currentCursorTile;
            var tool = new Pickaxe();
            var potency = 1 + (School.Level - Level) / 4;
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, potency, Game1.player);
            return base.Cast();
        }
    }
}