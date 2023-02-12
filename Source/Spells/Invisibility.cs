using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using StardewValley;
using System;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public void Update()
        { }

        public Invisibility()
        {
            Name = "Invisibility";
            School = School.Illusion;
            Description = "Makes the caster invisible";
            CastingTime = 1.0f;
            Level = 6;
        }

        public bool Cast()
        {
            //if (!RuneMagic.PlayerStats.Effects.Contains(Effect))
            //{
            //    RuneMagic.PlayerStats.Effects.Add(Effect);

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
    }
}