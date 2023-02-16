using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace RuneMagic.Source.Spells
{
    public class Breaking : Spell
    {
        public Breaking() : base()
        {
            School = School.Alteration;
            Description = "Breaks debris on the target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            //set Target to the debris under muse cursor
            var pickaxe = new Pickaxe();
            var target = Game1.currentLocation.getObjectAtTile((int)Game1.currentCursorTile.X, (int)Game1.currentCursorTile.Y);
            if (target == null)
                return false;
            pickaxe.DoFunction(Game1.currentLocation, (int)target.TileLocation.X * Game1.tileSize, (int)target.TileLocation.Y * Game1.tileSize, 1, Game1.player);
            return true;
        }
    }
}