using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Blasting : Spell
    {
        public Blasting() : base()
        {
            School = School.Evocation;
            Description = "Creates an explosion at a target location.";
            Level = 5;
        }

        public override bool Cast()
        {
            Target = Game1.currentCursorTile;
            var radius = 1 + (Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill) - 4) / 6;
            Game1.currentLocation.explode(Target, radius, Game1.player);
            RuneMagic.Instance.Monitor.Log(radius.ToString());
            return true;
        }
    }
}