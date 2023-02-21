using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using static SpaceCore.Skills;

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
            Target = Game1.currentCursorTile;
            var radius = 1 + (Game1.player.GetCustomSkillLevel(Skill) - 4) / 6;
            Game1.currentLocation.explode(Target, radius, Game1.player);
            return base.Cast();
        }
    }
}