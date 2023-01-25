using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;



namespace RuneMagic.Skills
{
    public class MagicSkill : SpaceCore.Skills.Skill
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The unique ID for the magic skill.</summary>
        public static readonly string MagicSkillId = "mochman.magic_skill";

        /// <summary>The level 5 'potential' profession.</summary>
        public static MagicProfession RuneCasterI;

        /// <summary>The level 10 'prodigy' profession.</summary>
        public static MagicProfession RuneCasterII;

        /// <summary>The level 10 'memory' profession.</summary>
        public static MagicProfession RuneCasterIII;

        /// <summary>The level 5 'Mana Regen I' profession.</summary>
        public static MagicProfession ScribeI;

        /// <summary>The level 10 'Mana Regen II' profession.</summary>
        public static MagicProfession ScribeII;

        /// <summary>The level 10 'Mana Reserve' profession.</summary>
        public static MagicProfession ScribeIII;

        public MagicSkill()
            : base(MagicSkillId)
        {
            Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Textures/Interface/skill_magic_icon.png");
            SkillsPageIcon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Textures/Interface/skill_page_magic_icon.png");
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = new Microsoft.Xna.Framework.Color(75, 0, 155);

            RuneCasterI = new MagicProfession(this, "")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Textures/Interface/profession_runecaster1_icon.png"),
                Name = "Novice Rune Caster",
                Description = "You have learned the basics of casting runes. You can now cast runes with a 10% chance of failure.",
            };
            ScribeI = new MagicProfession(this, "")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Textures/Interface/profession_scribe1_icon.png"),
                Name = "Novice Scribe",
                Description = "You have learned the basics of scribing scrolls. Your scrolls have a 20% chance of not being consumed when used.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(5, RuneCasterI, ScribeI));
        }


        public override string GetName()
        {
            return "Magic";
        }

        public override List<string> GetExtraLevelUpInfo(int level)
        {
            return new()
            {
                "-1% Chance of Rune Failure",
            };
        }

        public override string GetSkillPageHoverText(int level)
        {
            return "-" + level + " Chance of Rune Failure";
        }

        public override void DoLevelPerk(int level)
        {

        }
    }
}