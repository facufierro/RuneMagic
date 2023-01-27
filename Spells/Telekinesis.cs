using Microsoft.Xna.Framework;
using RuneMagic.Famework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Spells
{
    public class Telekinesis : Spell
    {
        public Telekinesis() : base()
        {
            Name = "Telekinesis";
            School = School.Alteration;
            Description = "Teleports a the caster a short distance.";
            Level = 2;
        }

        public override bool Cast()
        {
            var cursor = Game1.currentCursorTile;
            Vector2 direction = cursor - Game1.player.getTileLocation();
            direction.Normalize();
            Vector2 destination = Game1.player.getTileLocation() + direction * 5;

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
    }
}
