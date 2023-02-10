using Microsoft.Xna.Framework;
using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public int ProjectileNumber { get; set; }
        public ISpellEffect Effect { get; set; }

        public Invisibility()
        {
            Name = "Invisibility";
            School = School.Illusion;
            Description = "Makes the caster invisible";
            CastingTime = 1.0f;
            Level = 6;
            ProjectileNumber = 0;
            Effect = new InvisibilityEffect(10);
        }

        public bool Cast()
        {
            if (!RuneMagic.PlayerStats.Effects.Contains(Effect))
            {
                RuneMagic.PlayerStats.Effects.Add(Effect);

                return true;
            }
            else
            {
                Effect.Timer = 0;
                return false;
            }



        }
    }
}
