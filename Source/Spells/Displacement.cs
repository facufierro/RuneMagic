
using Microsoft.Xna.Framework;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Displacement : Spell
    {

        public Displacement() : base()
        {
            Name = "Displacement";
            School = School.Alteration;
            Description = "Teleports a the caster a short distance.";
            Level = 3;
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
