
namespace RuneMagic.Source.Spells
{
    public class Telekinesis : Spell
    {
        public Telekinesis() : base()
        {
            Name = "Telekinesis";
            School = School.Alteration;
            Description = "You gain the ability to move or manipulate creatures or objects by thought.";
            Level = 5;
        }

        public override bool Cast()
        {
            return false;
        }
    }
}
