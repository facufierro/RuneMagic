using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;



namespace RuneMagic.Source
{
    public class MagicSkill : SpaceCore.Skills.Skill
    {
        /*********
        ** Accessors
        *********/
        public static readonly string MagicSkillId = "fierro.rune_magic.skill";
        public static MagicProfession Scribe;
        public static MagicProfession Lorekeeper;
        public static MagicProfession Sage;
        public static MagicProfession Runesmith;
        public static MagicProfession Runelord;
        public static MagicProfession Runemaster;


        public MagicSkill()
            : base(MagicSkillId)
        {
            Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_magic_icon.png");
            SkillsPageIcon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill_page_magic_icon.png");
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Color.DarkBlue;

            Runesmith = new MagicProfession(this, "fierro.rune_magic.runesmith")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runesmith",
                Description = "Your runes recharge 50% faster.",
            };
            Professions.Add(Runesmith);
            Scribe = new MagicProfession(this, "fierro.rune_magic.scribe")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Scribe",
                Description = "Scroll casting time is reduced by 50%.",
            };
            Professions.Add(Scribe);
            ProfessionsForLevels.Add(new ProfessionPair(5, Runesmith, Scribe));

            Runelord = new MagicProfession(this, "fierro.rune_magic.runelord")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runelord",
                Description = "When you craft a rune it has 10 charges instead of 5.",
            };
            Professions.Add(Runelord);
            Runemaster = new MagicProfession(this, "fierro.rune_magic.runemaster")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Runemaster",
                Description = "The caster reflects damage while casting runes.NOT IMPLEMENTED"
            };
            Professions.Add(Runemaster);
            ProfessionsForLevels.Add(new ProfessionPair(10, Runelord, Runemaster, Runesmith));

            Lorekeeper = new MagicProfession(this, "fierro.rune_magic.lorekeeper")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Lorekeeper",
                Description = "The caster has 20% chance to not consume a scroll when casting.NOT IMPLEMENTED"
            };
            Professions.Add(Lorekeeper);
            Sage = new MagicProfession(this, "fierro.rune_magic.sage")
            {
                Icon = RuneMagic.Instance.Helper.ModContent.Load<Texture2D>("assets/Interface/skill-icon.png"),
                Name = "Sage",
                Description = "The caster can walk while casting.NOT IMPLEMENTED",
            };
            Professions.Add(Sage);
            ProfessionsForLevels.Add(new ProfessionPair(10, Lorekeeper, Sage, Scribe));

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
            foreach (var spell in RuneMagic.Spells)
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