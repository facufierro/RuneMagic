
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewValley;
using System.Collections.Generic;
using xTile.Dimensions;
using xTile.Tiles;

namespace RuneMagic.Source.Spells
{
    public class Displacement : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Displacement() : base()
        {
            Name = "Displacement";
            School = School.Alteration;
            Description = "Teleports a the caster to a target location.";
            CastingTime = 1;
            Level = 4;
        }

        public bool Cast()
        {
            var cursorTile = Game1.currentCursorTile;
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
