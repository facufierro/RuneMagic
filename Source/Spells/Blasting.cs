using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Blasting : Spell
    {
        public Blasting() : base(School.Evocation)
        {
            Description += "Creates an explosion at a target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            var target = Game1.currentCursorTile;
            var radius = 1 + (School.Level - 4) / 6;
            Game1.currentLocation.explode(target, radius, Game1.player);
            return base.Cast();
        }
    }
}