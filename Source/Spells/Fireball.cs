using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base(School.Evocation)
        {
            Description += "Shoots a fire ball, damaging monsters in a big area around the target location";
            Level = 4;
        }

        public override bool Cast()
        {
            var texture = RuneMagic.Textures["spell_fireball"];
            var minDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkills[School.Evocation]);
            var maxDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkills[School.Evocation]) * 3;
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkills[School.Evocation]);
            var area = 1;
            var speed = 5;

            Game1.currentLocation.projectiles.Add(new SpellProjectile(texture, minDamage, maxDamage, bonusDamage, area, speed, false));
            return base.Cast();
        }
    }
}