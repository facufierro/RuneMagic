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
    public class AlterationSkill : MagicSkill
    {
        public static readonly string AlterationSkillId = $"{MagicSkillId}_alteration";

        public AlterationSkill(List<Color> colors)
                    : base(AlterationSkillId, colors, new() { RuneMagic.Textures["interface_skill_icon_alteration"], RuneMagic.Textures["interface_page_icon_alteration"] })
        {
            Colors = colors;
            MagicProfessions = new List<MagicProfession>
            {
                new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_0")
                {
                    Icon = Icon,
                    Name = "Transmuter",
                    Description = "When you use the Transmutation spell you gain more money.",
                },
                 new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_1")
                {
                    Icon = Icon,
                    Name = "Illusionist",
                    Description = "Your Illusion spells last longer.",
                },
                  new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_2")
                {
                    Icon = Icon,
                    Name = "Dimensional Traveler",
                    Description = "Once a day you can create a magic anchor and use it to teleport back to that location.",
                },
                   new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_3")
                {
                    Icon = Icon,
                    Name = "Chronomancer",
                    Description = "Once a day you can skip or back up time.",
                },
                    new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_4")
                {
                    Icon = Icon,
                    Name = "Alchemist",
                    Description = "You can coat your weapon with poison.",
                },
                     new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_5")
                {
                    Icon = Icon,
                    Name = "Shadow",
                    Description = "Your Invisibility spells last longer.",
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