using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Skills
{
    public class MagicSkill : Skill
    {
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
            Icon = RuneMagic.Textures["interface_skill_icon_magic"];
            SkillsPageIcon = RuneMagic.Textures["interface_skill_icon_magic"];
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Color.DarkBlue;

            Runesmith = new MagicProfession(this, "fierro.rune_magic.runesmith")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_runesmith"],
                Name = "Runesmith",
                Description = "Your runes recharge 50% faster.",
            };
            Professions.Add(Runesmith);
            Scribe = new MagicProfession(this, "fierro.rune_magic.scribe")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_scribe"],
                Name = "Scribe",
                Description = "Scroll casting time is reduced by 20%.",
            };
            Professions.Add(Scribe);
            ProfessionsForLevels.Add(new ProfessionPair(5, Runesmith, Scribe));

            Runelord = new MagicProfession(this, "fierro.rune_magic.runelord")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_runelord"],
                Name = "Rune Lord",
                Description = "When you craft a rune it has 10 charges instead of 5.",
            };
            Professions.Add(Runelord);
            Runemaster = new MagicProfession(this, "fierro.rune_magic.runemaster")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_runemaster"],
                Name = "Rune Master",
                Description = "The caster can spend 3 charges of a rune to make it instant cast."
            };
            Professions.Add(Runemaster);
            ProfessionsForLevels.Add(new ProfessionPair(10, Runelord, Runemaster, Runesmith));

            Lorekeeper = new MagicProfession(this, "fierro.rune_magic.lorekeeper")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_lorekeeper"],
                Name = "Lorekeeper",
                Description = "The caster has 20% chance to not consume a scroll when casting."
            };
            Professions.Add(Lorekeeper);
            Sage = new MagicProfession(this, "fierro.rune_magic.sage")
            {
                Icon = RuneMagic.Textures["interface_skill_icon_sage"],
                Name = "Sage",
                Description = "The caster can walk while casting.",
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
            List<string> info;
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
            return /*$"-{level}% Chance of Casting Failure" + Environment.NewLine +*/ $"+{level}% Casting Speed";
        }
    }
}