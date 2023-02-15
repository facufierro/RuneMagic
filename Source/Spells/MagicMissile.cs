using RuneMagic.Source.SpellEffects;
using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class MagicMissile : Spell
    {
        public MagicMissile() : base()
        {
            Name = "Magic Missile";
            School = School.Evocation;
            Description = "Shoots a magic missile";
            Level = 1;
        }

        public override bool Cast()
        {
            var projectileNumber = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill) / 2;
            var spellTexture = "magic_missile";
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill);
            var minDamage = 1;
            var maxDamage = 4;
            var Area = 0;
            var velocity = 7;
            var range = 400;
            var spread = 2;
            var isHoming = false;
            var hitSound = "flameSpellHit";

            for (int i = 0; i < projectileNumber; i++)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, spellTexture, minDamage, maxDamage, bonusDamage, Area,
     velocity, range, spread, isHoming, hitSound));
            return true;
        }
    }
}