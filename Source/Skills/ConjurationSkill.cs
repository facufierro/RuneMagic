using Force.DeepCloner;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Skills
{
    public class ConjurationSkill : MagicSkill
    {
        public static readonly string ConjurationSkillId = $"{MagicSkillId}_conjuration";

        public ConjurationSkill(List<Color> colors)
                    : base(ConjurationSkillId, colors, new() { RuneMagic.Textures["interface_skill_icon_conjuration"], RuneMagic.Textures["interface_page_icon_conjuration"] })
        {
            Colors = colors;
            MagicProfessions = new List<MagicProfession>
            {
                new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_0")
                {
                    Icon = Icon,
                    Name = "Summoner",
                    Description = "Your Summons are more powerful",
                },
                 new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_1")
                {
                    Icon = Icon,
                    Name = "Artificer",
                    Description = "Your conjured tools are more powerful.",
                },
                  new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_2")
                {
                    Icon = Icon,
                    Name = "Golemancer",
                    Description = "You can create a Golem that follows you around and helps you",
                },
                   new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_3")
                {
                    Icon = Icon,
                    Name = "Necromancer",
                    Description = "You can have more Summons at one time.",
                },
                    new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_4")
                {
                    Icon = Icon,
                    Name = "Materializer",
                    Description = "You can create ",
                },
                     new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_5")
                {
                    Icon = Icon,
                    Name = "Artisan",
                    Description = "Once a day you can create a copy of an Item. It will always be of High Quality.",
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