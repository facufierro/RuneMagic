using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Healing : Spell
    {
        public Healing() : base(School.Abjuration)
        {
            Description += "Restores the caster's health.";
            Level = 1;
        }

        public override bool Cast()
        {
            if (Game1.player.health >= Game1.player.maxHealth)
                return false;
            var heal = Game1.player.GetCustomSkillLevel(Skill) * 10;
            if (heal > Game1.player.maxHealth - Game1.player.health)
                heal = Game1.player.maxHealth - Game1.player.health;
            Game1.player.health += heal;
            return base.Cast();
        }
    }
}