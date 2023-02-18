using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Effects;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class MagicMissile : Spell
    {
        public MagicMissile() : base()
        {
            Name = "Magic Missile";
            School = School.Evocation;
            Description = "Shoots a magic missile per two magic skill levels.";
            Level = 1;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<CastingMagicMissile>().Any())
            {
                Effect = new CastingMagicMissile(Name);
                return true;
            }
            else
                return false;
        }
    }
}