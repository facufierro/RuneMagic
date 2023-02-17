using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Healing : Spell
    {
        public Healing() : base()
        {
            School = School.Abjuration;
            Description = "";
            Level = 2;
        }

        public override bool Cast()
        {
            if (Game1.player.health >= Game1.player.maxHealth)
                return false;
            var heal = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill) * 10;
            if (heal > Game1.player.maxHealth - Game1.player.health)
                heal = Game1.player.maxHealth - Game1.player.health;
            Game1.player.health += heal;
            return true;
        }
    }
}