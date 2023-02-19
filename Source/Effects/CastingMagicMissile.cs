using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class CastingMagicMissile : SpellEffect
    {
        public CastingMagicMissile(Spell spell) : base(spell, Duration.Instant)
        {
            Timer = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkills[School.Evocation]);
            if (Timer > 12)
                Timer = 12;
            Start();
        }

        public override void Update()
        {
            var texture = RuneMagic.Textures["spell_magic_missile"];
            var minDamage = 1;
            var maxDamage = 4;
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkills[School.Evocation]);
            var speed = 5;

            if (Timer % 3 == 0)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(texture, minDamage, maxDamage, bonusDamage, speed, true));
            base.Update();
        }
    }
}