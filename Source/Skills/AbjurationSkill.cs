using Force.DeepCloner;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Skills
{
    public class AbjurationSkill : MagicSkill
    {
        public static readonly string AbjurationSkillId = $"{MagicSkillId}_abjuration";

        public AbjurationSkill(List<Color> colors)
            : base(AbjurationSkillId, colors, new() { RuneMagic.Textures["interface_skill_icon_abjuration"], RuneMagic.Textures["interface_page_icon_abjuration"] })
        {
            Colors = colors;

            MagicProfessions = new List<MagicProfession>
            {
                new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_0")
                {
                    Icon = Icons[1],
                    Name = "Purifier",
                    Description = "For every harmful condition removed with the Cleanse spell, gain Health and Stamina.",
                },
                 new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_1")
                {
                    Icon = Icons[1],
                    Name = "Guardian",
                    Description = "Your Shield spells are more effective.",
                },
                  new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_2")
                {
                    Icon = Icons[1],
                    Name = "White Wizard",
                    Description = "Your Healing spells are more effective, and leave a regeneration effect for a short period of time.",
                },
                   new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_3")
                {
                    Icon = Icons[1],
                    Name = "Buff Specialist",
                    Description = "Your Buff spells last longer",
                },
                    new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_4")
                {
                    Icon = Icons[1],
                    Name = "Wardweaver",
                    Description = "You can create a Ward that makes you immune to damage for 5 hits. It last the entire day.",
                },
                     new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_5")
                {
                    Icon = Icons[1],
                    Name = "Battle Mage",
                    Description = "For each enemy banished using the Banish spell, you get a cumulative attack buff.",
                },
            };

            foreach (var profession in MagicProfessions)
            {
                Professions.Add(profession);
            }
            ProfessionsForLevels.Add(new ProfessionPair(5, MagicProfessions[0], MagicProfessions[1]));
            ProfessionsForLevels.Add(new ProfessionPair(10, MagicProfessions[2], MagicProfessions[3]));
            ProfessionsForLevels.Add(new ProfessionPair(10, MagicProfessions[4], MagicProfessions[5]));
        }
    }
}