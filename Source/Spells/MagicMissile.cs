﻿

using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class MagicMissile : Spell
    {
        public int ProjectileNumber { get; set; }

        public MagicMissile() : base()
        {

            Name = "Magic Missile";
            School = School.Evocation;
            Description = "Shoots a magic missile";
            Level = 1;
            ProjectileNumber = 1;

        }
        public override bool Cast()
        {
            for (int i = 0; i < ProjectileNumber; i++)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, "magic_missile", 1, 4, 1, 8, 400, 3, true, ""));
            return true;
        }
    }
}