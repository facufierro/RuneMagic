using Microsoft.Xna.Framework;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Displacement : Spell
    {
        public Displacement() : base(School.Alteration)
        {
            Description += "Teleports a the caster to a target location."; Level = 3;
        }

        public override bool Cast()
        {
            var target = Game1.currentCursorTile;
            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(target))
            {
                Game1.player.Position = new Vector2(target.X * Game1.tileSize, target.Y * Game1.tileSize);
                return base.Cast();
            }
            else
            {
                return false;
            }
        }
    }
}