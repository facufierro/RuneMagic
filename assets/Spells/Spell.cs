using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;

namespace RuneMagic.assets.Spells

{

    public enum SpellType
    {
        Active,
        Passive,
        Daily,
        Timed
    }

    public class Spell
    {
        public SpellType Type { get; set; }

        public Spell()
        {

        }
        public void Magic()
        {
            Type = SpellType.Active;
        }
        public bool Translocation()
        {
            Type = SpellType.Active;
            //get cursor position
            Vector2 target = Game1.currentCursorTile;

            //teleport 3 tiles from the player in the direction of the target
            Vector2 direction = target - Game1.player.getTileLocation();
            direction.Normalize();
            Vector2 destination = Game1.player.getTileLocation() + direction * 5;

            //check if the destination is a valid tile
            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(destination))
            {
                Game1.player.Position = destination * Game1.tileSize;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Water()
        {
            //get tile at cursor position
            Vector2 target = Game1.currentCursorTile;
            //check if it can be watered
            if (Game1.currentLocation.terrainFeatures.ContainsKey(target) && Game1.currentLocation.terrainFeatures[target] is HoeDirt)
            {
                //water the tile
                HoeDirt dirt = (HoeDirt)Game1.currentLocation.terrainFeatures[target];
                dirt.state.Value = HoeDirt.watered;

            }

        }
    }
}
