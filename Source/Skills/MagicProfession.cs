using static SpaceCore.Skills;

namespace RuneMagic.Source.Skills
{
    public class MagicProfession : Skill.Profession
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public MagicProfession(Skill skill, string Id)
            : base(skill, Id) { }

        public override string GetName()
        {
            return Name;
        }

        public override string GetDescription()
        {
            return Description;
        }
    }
}