using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Fireball : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get; set; }

        public Fireball()
        {
            Name = "Fireball";
            School = School.Evocation;
            Description = "Shoots a fireball";
            CastingTime = 1;
            Level = 1;
        }

        public bool Cast()
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
    }
}
