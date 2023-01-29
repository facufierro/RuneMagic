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

        public static MagicProfession Runecaster;
        public static MagicProfession Scribe;
        public static MagicProfession Runekeeper;
        public static MagicProfession Loremaster;
        public static MagicProfession Runemaster;
        public static MagicProfession Sage;

        public MagicSkill()
            : base(MagicSkillId)
        {
            Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_magic_icon.png");
            SkillsPageIcon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_page_magic_icon.png");
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Color.DarkBlue;

            Runecaster = new MagicProfession(this, "fierro.rune_magic.runecaster")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runecaster",
                Description = "Your runes recharge 50% faster.",
            };
            Scribe = new MagicProfession(this, "fierro.rune_magic.scribe")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Scribe",
                Description = "Scroll casting time is reduced by 50%.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(5, Runecaster, Scribe));

            Runekeeper = new MagicProfession(this, "fierro.rune_magic.runekeeper")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runekeeper",
                Description = "Your runes have 10 charges instead of 5.",
            };
            Loremaster = new MagicProfession(this, "fierro.rune_magic.loremaster")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Loremaster",
                Description = "The caster has 20% chance to not consume a scroll when casting."
            };
            ProfessionsForLevels.Add(new ProfessionPair(10, Runekeeper, Loremaster));

            Runemaster = new MagicProfession(this, "fierro.rune_magic.runemaster")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runemaster",
                Description = "The caster reflects damage while casting runes."
            };
            Sage = new MagicProfession(this, "fierro.rune_magic.sage")
            {
                Icon = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Sage",
                Description = "The caster can walk while casting.",
            };
            ProfessionsForLevels.Add(new ProfessionPair(10, Runemaster, Sage));
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