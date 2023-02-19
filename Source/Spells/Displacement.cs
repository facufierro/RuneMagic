using Microsoft.Xna.Framework;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Displacement : Spell
    {
        public Displacement() : base(School.Alteration)
        {
            Description += "Teleports a the caster to a target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(Target))
            {
                Game1.player.Position = new Vector2(Target.X * Game1.tileSize, Target.Y * Game1.tileSize);
                return base.Cast();
            }
            else
            {
                return false;
            }
        }
    }
}