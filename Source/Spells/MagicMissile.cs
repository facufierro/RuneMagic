

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
        public int ProjectileNumber { get; set; }

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
            ProjectileNumber = RuneMagic.Farmer.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill) / 2;
            for (int i = 0; i < ProjectileNumber; i++)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, "magic_missile", 1, 4, 1, 8, 400, 3, true, ""));
            return true;
        }
    }
}
