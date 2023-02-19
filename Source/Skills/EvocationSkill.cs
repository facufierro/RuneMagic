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
    public class EvocationSkill : MagicSkill
    {
        public static readonly string EvocationSkillId = $"{MagicSkillId}_evocation";

        public EvocationSkill(List<Color> colors)
                    : base(EvocationSkillId, colors, new() { RuneMagic.Textures["interface_skill_icon_evocation"], RuneMagic.Textures["interface_page_icon_evocation"] })
        {
            Colors = colors;

            MagicProfessions = new List<MagicProfession>
            {
                new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_0")
                {
                    Icon = Icon,
                    Name = "Arcanist",
                    Description = "Your non elemental damaging spells are more effective.",
                },
                 new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_1")
                {
                    Icon = Icon,
                    Name = "Elementalist",
                    Description = "Your elemental damaging spells are more effective.",
                },
                  new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_2")
                {
                    Icon = Icon,
                    Name = "Channeler",
                    Description = "When you cast a non elemental damaging spell you have a chance of casting it twice.",
                },
                   new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_3")
                {
                    Icon = Icon,
                    Name = "Blaster",
                    Description = "Your non elemental damaging spells have bigger radius.",
                },
                    new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_4")
                {
                    Icon = Icon,
                    Name = "Pyromancer",
                    Description = "Your Fire spells are more effective.",
                },
                     new MagicProfession(this, $"fierro.rune_magic.{GetType().Name.Replace("Skill","").ToLower()}_profession_5")
                {
                    Icon = Icon,
                    Name = "Cryomancer",
                    Description = "Your Ice spells are more effective.",
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