using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Translocation : Spell
    {
        public Translocation() { }

        public override void Cast()
        {
            Cursor = Game1.currentCursorTile;
            Vector2 direction = Cursor - Game1.player.getTileLocation();
            direction.Normalize();
            Vector2 destination = Game1.player.getTileLocation() + direction * 5;

            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(destination))
            {
                Game1.player.Position = destination * Game1.tileSize;
            }
        }

    }
}
