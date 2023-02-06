using RuneMagic.Source.Interfaces;

namespace RuneMagic.Source.NotImplementedSpells
{
    public class Telekinesis : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public int ProjectileNumber { get; set; }
        public ISpellEffect Effect { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Telekinesis() : base()
        {
            Name = "Telekinesis";
            School = School.Alteration;
            Description = "You gain the ability to move or manipulate creatures or objects by thought.";
            Level = 5;
        }


        public bool Cast()
        {
            return false;
        }
    }
}
