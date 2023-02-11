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
    public class Warding : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get; set; }

        public Warding()
        {
            Name = "Warding";
            Description = "Protects the caster from damage for a short period of time.";
            School = School.Abjuration;
            CastingTime = 1.0f;
            Level = 1;
            Effect = new WardingEffect(10);
        }

        public bool Cast()
        {
            if (!RuneMagic.PlayerStats.Effects.Contains(Effect))
            {
                Effect.Start();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
