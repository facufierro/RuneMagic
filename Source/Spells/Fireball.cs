using RuneMagic.Source.SpellEffects;
using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base()
        {
            School = School.Evocation;
            Description = "Shoots a fireball";
            Level = 6;
        }

        public override bool Cast()
        {
            var spellTexture = "fireball";
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill);
            var minDamage = 1;
            var maxDamage = 4;
            var Area = 4;
            var velocity = 8;
            var range = 300;
            var spread = 0;
            var isHoming = false;
            var hitSound = "explosion";

            Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, spellTexture, minDamage, maxDamage, bonusDamage, Area,
                velocity, range, spread, isHoming, hitSound));
            return true;
        }

        //public override void Update()
        //{
        //    //Implement fire damage over time
        //}
    }
}