

namespace RuneMagic.Source.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base()
        {
            Name = "Fireball";
            School = School.Evocation;
            Description = "Shoots a fireball.";
            Level = 1;

        }

        public override bool Cast()
        {
            return false;
        }
    }
}
