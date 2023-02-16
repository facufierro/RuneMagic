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
            School = School.Conjuration;
            Description = "Breaks debris on the target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            var tool = new Pickaxe();
            tool.DoFunction(Game1.currentLocation, (int)Game1.currentCursorTile.X * Game1.tileSize, (int)Game1.currentCursorTile.Y * Game1.tileSize, 1, Game1.player);
            return true;
        }
    }
}