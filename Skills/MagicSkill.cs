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

        /// <summary>The level 5 'RunicAffinity' profession.</summary>
        public static MagicProfession RunicAffinity;

        /// <summary>The level 10 'RunicManipulation' profession.</summary>
        public static MagicProfession RunicManipulation;

        /// <summary>The level 10 'RuneMastery' profession.</summary>
        public static MagicProfession RuneMastery;

        /// <summary>The level 5 'ArcaneScribing' profession.</summary>
        public static MagicProfession ArcaneScribing;

        /// <summary>The level 10 'ScrollAugmentation' profession.</summary>
        public static MagicProfession ScrollAugmentation;

        /// <summary>The level 10 'ScrollMastery' profession.</summary>
        public static MagicProfession ScrollMastery;

        public MagicSkill()
            : base(MagicSkillId)
        {
            Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_magic_icon.png");
            SkillsPageIcon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_page_magic_icon.png");
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Color.DarkBlue;

            RunicAffinity = new MagicProfession(this, "fierro.rune_magic.runic_affinity")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/runic_affinity-icon.png"),
                Name = "Runic Affinity",
                Description = "Your runes recharge 50% faster.",
            };
            ArcaneScribing = new MagicProfession(this, "fierro.rune_magic.arcane_scribing")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/arcane_scribing-icon.png"),
                Name = "Arcane Scribing",
                Description = "Scroll casting time is reduced by 20%.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(5, RunicAffinity, ArcaneScribing));

            RunicManipulation = new MagicProfession(this, "fierro.rune_magic.runic_manipulation")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/runic_manipulation-icon.png"),
                Name = "RunicManipulation",
                Description = "Your runes recharge 50% faster.",
            };
            ScrollAugmentation = new MagicProfession(this, "fierro.rune_magic.scroll_augmentation")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/scroll_augmentation-icon.png"),
                Name = "Novice Scribe",
                Description = "Scroll casting time is reduced by 20%.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(10, RunicManipulation, ArcaneScribing));

            RuneMastery = new MagicProfession(this, "fierro.rune_magic.rune_mastery")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/rune_mastery-icon.png"),
                Name = "RunicManipulation",
                Description = "Your runes recharge 50% faster.",
            };
            ScrollMastery = new MagicProfession(this, "fierro.rune_magic.scroll_mastery")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/scroll_mastery-icon.png"),
                Name = "Novice Scribe",
                Description = "Scroll casting time is reduced by 20%.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(10, RunicManipulation, ArcaneScribing));
        }
        public override string GetName()
        {
            return "Magic";
        }

        public override List<string> GetExtraLevelUpInfo(int level)
        {

            List<string> info = new List<string>();
            if (level == 1)
                info = new List<string>(){
                $"The wizard has taught you the basics of magic." };
            else
                info = new List<string>(){
                $"You now have {level}% less chance of Casting Failure",
                $"and {level}% Casting Speed"};

            info.Add($"You have gained access to the following spells:");

            string spellList = "";
            foreach (var spell in ModEntry.RuneMagic.SpellList)
            {
                if (spell.Level == level)
                {
                    //set spellList to spellList + spell.Name  and add a comma if it's not the last spell in the list
                    spellList = spellList + spell.Name + (spellList == "" ? "" : ", ");


                }
            }
            //remove the last comma from spellList
            spellList = spellList.TrimEnd(new char[] { ',', ' ' });
            info.Add($" {spellList}");
            return info;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return $"-{level}% Chance of Casting Failure" + Environment.NewLine + $"+{level}% Casting Speed";

        }


    }
}