using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Effects
{
    public class CastingMagicMissile : SpellEffect
    {
        public int Interval { get; set; }

        public CastingMagicMissile(Spell spell) : base(spell, Duration.Instant)
        {
            Interval = 3;
            Timer = RuneMagic.PlayerStats.MagicSkill.Level;
            if (Timer > 12)
                Timer = 12;
            Start();
        }

        public override void Update()
        {
            var texture = RuneMagic.Textures["spell_magic_missile"];
            var minDamage = 1;
            var maxDamage = 4;
            var bonusDamage = RuneMagic.PlayerStats.MagicSkill.Level;
            var area = 0;
            var speed = 5;

            if (Timer % (3 * Interval) == 0)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(texture, minDamage, maxDamage, bonusDamage, area, speed, true));
            base.Update();
        }
    }
}