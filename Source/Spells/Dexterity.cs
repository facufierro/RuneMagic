using RuneMagic.Source.Interfaces;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Dexterity : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public Dexterity()
        {
            Name = "Dexterity";
            School = School.Alteration;
            Description = "Increases your dexterity.";
            Level = 1;
            CastingTime = 1;
        }

        public bool Cast()
        { return false; }

        public void Update()
        { }
    }
}