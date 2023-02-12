using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Warding : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public Warding()
        {
            Name = "Warding";
            Description = "Protects the caster from damage for a short period of time.";
            School = School.Abjuration;
            CastingTime = 1.0f;
            Level = 1;
        }

        public bool Cast()
        {
            return false;
        }

        public void Update()
        { }
    }
}