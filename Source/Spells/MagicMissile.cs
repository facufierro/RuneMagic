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
        public MagicMissile() : base(School.Evocation)

        {
            Name = "Magic Missile";
            Description += "Shoots a magic missile per two magic skill levels.";
            Level = 1;
        }

        public override bool Cast()
        {
            if (!Player.MagicStats.ActiveEffects.OfType<CastingMagicMissile>().Any())
            {
                Effect = new CastingMagicMissile(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}