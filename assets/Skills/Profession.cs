namespace RuneMagic.assets.Skills
{
    public class MagicProfession : SpaceCore.Skills.Skill.Profession
    {
        /*********
        ** Accessors
        *********/
        public string Name { get; set; }
        public string Description { get; set; }


        /*********
        ** Public methods
        *********/
        public MagicProfession(MagicSkill skill, string Id)
            : base(skill, Id) { }

        public override string GetName()
        {
            return this.Name;
        }

        public override string GetDescription()
        {
            return this.Description;
        }
    }
}