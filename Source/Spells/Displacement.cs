
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewValley;
using System.Collections.Generic;
using xTile.Dimensions;
using xTile.Tiles;

namespace RuneMagic.Source.Spells
{
    public class Displacement : Spell
    {

        public Displacement() : base()
        {
            Name = "Displacement";
            School = School.Alteration;
            Description = "Teleports a the caster to a target location.";
            Level = 4;
        }

        public override bool Cast()
        {
            //get cursor tile position
            var cursorTile = Game1.currentCursorTile;
            //teleport player to cursor tile if it is walkable
            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(cursorTile))
            {
                RuneMagic.Farmer.Position = new Vector2(cursorTile.X * Game1.tileSize, cursorTile.Y * Game1.tileSize);


                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
