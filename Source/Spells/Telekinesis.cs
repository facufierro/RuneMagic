
namespace RuneMagic.Source.Spells
{
    public class Telekinesis : Spell
    {
        public Telekinesis() : base()
        {
            Name = "Telekinesis";
            School = School.Alteration;
            Description = "Teleports a the caster a short distance.";
            Level = 1;
        }

        public override bool Cast()
        {
            return false;
        }
    }
}
