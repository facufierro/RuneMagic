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
        public CastingMagicMissile(string name) : base(name, Duration.Instant)
        {
            Name = name;
            Timer = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill);
            if (Timer > 12)
                Timer = 12;
            Start();
        }

        public override void Update()
        {
            var texture = RuneMagic.Textures["spell_magic_missile"];
            var minDamage = 1;
            var maxDamage = 4;
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill);
            var speed = 5;

            if (Timer % 3 == 0)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(texture, minDamage, maxDamage, bonusDamage, speed, true));
            base.Update();
        }
    }
}