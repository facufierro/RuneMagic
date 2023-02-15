namespace RuneMagic.Source.Spells
{
    public class Fear : Spell
    {
        public Fear() : base()
        {
            School = School.Illusion;
            Description = "You cause a creature you can see within range to become frightened for the duration. If the creature ends its turn out of line of sight of you, the creature can make a Wisdom saving throw. On a successful save, the spell ends for that creature.";
            Level = 5;
        }
    }
}