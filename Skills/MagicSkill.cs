using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source;
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
        public static readonly string MagicSkillId = "fierro.rune_magic.skill";

        /// <summary>The level 5 'RuneCasterI' profession.</summary>
        public static MagicProfession RuneCasterI;

        /// <summary>The level 10 'RuneCasterII' profession.</summary>
        public static MagicProfession RuneCasterII;

        /// <summary>The level 10 'RuneCasterIII' profession.</summary>
        public static MagicProfession RuneCasterIII;

        /// <summary>The level 5 'ScribeI' profession.</summary>
        public static MagicProfession ScribeI;

        /// <summary>The level 10 'ScribeII' profession.</summary>
        public static MagicProfession ScribeII;

        /// <summary>The level 10 'ScribeIII' profession.</summary>
        public static MagicProfession ScribeIII;

        public MagicSkill()
            : base(MagicSkillId)
        {
            Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_magic_icon.png");
            SkillsPageIcon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_page_magic_icon.png");
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Color.DarkBlue;

            RuneCasterI = new MagicProfession(this, "fierro.rune_magic.runecaster_1")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/profession_runecaster1_icon.png"),
                Name = "Novice Rune Caster",
                Description = "Your runes recharge 50% faster.",
            };
            ScribeI = new MagicProfession(this, "fierro.rune_magic.scribe_1")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/profession_scribe1_icon.png"),
                Name = "Novice Scribe",
                Description = "Scroll casting time is reduced by 20%.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(5, RuneCasterI, ScribeI));

        }
        public override string GetName()
        {
            return "Magic";
        }

        public override List<string> GetExtraLevelUpInfo(int level)
        {
            List<string> info = new List<string> { $"-{level}% Chance of Casting Failure", $"+{level}% Chance of Casting Failure" };
            
            info.Concat(ModEntry.RuneMagic.PlayerStats.LearnRecipes(level));
            return info;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return $"-{level}% Chance of Casting Failure" + Environment.NewLine + $"+{level}% Chance of Casting Failure";

        }


    }
}