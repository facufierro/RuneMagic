

using JsonAssets.Data;
using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using SpaceCore;
using StardewValley;
using System;

namespace RuneMagic.Source.Spells
{
    public class MagicMissile : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MagicMissile() : base()
        {

            Name = "Magic Missile";
            School = School.Evocation;
            Description = "Shoots a magic missile";
            CastingTime = 1;
            Level = 1;


        }
        public bool Cast()
        {
            var projectileNumber = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill) / 2;
            var bonusDamage = Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill);
            var minDamage = 1;
            var maxDamage = 4;
            for (int i = 0; i < projectileNumber; i++)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, "magic_missile", minDamage, maxDamage, bonusDamage, 8, 400, 3, true, ""));
            return true;
        }
    }
}
