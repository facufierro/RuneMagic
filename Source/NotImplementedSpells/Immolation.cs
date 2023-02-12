using RuneMagic.Source.Interfaces;
using StardewValley;

namespace RuneMagic.Source.NotImplementedSpells
{
    public class Immolation : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public bool Cast()
        {
            return false;
        }

        public void Update()
        {
        }
    }
}