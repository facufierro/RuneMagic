using Force.DeepCloner;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Skills
{
    public class MagicSkill : Skill
    {
        public static readonly string MagicSkillId = "fierro.rune_magic.magic_skill";
        public string Name { get; set; }
        public static List<MagicProfession> MagicProfessions { get; set; }
        public List<Color> Colors { get; set; }
        public string Description { get; set; }
        public List<Texture2D> Icons { get; set; }

        public MagicSkill(string id, List<Color> colors, List<Texture2D> icons)
            : base(id)
        {
            Name = GetType().Name.Replace("Skill", " Magic");
            Colors = colors;
            Icons = icons;
            Icon = Icons[1];
            SkillsPageIcon = Icons[0];
            ExperienceCurve = new[] { 100, 380, 770, 1300, 2150, 3300, 4800, 6900, 10000, 15000 };
            ExperienceBarColor = Colors[1];
        }

        public override string GetName()
        {
            return Name;
        }

        public override List<string> GetExtraLevelUpInfo(int level)
        {
            var info = new List<string>();
            var spellInfo = "";
            info.Add($"Your Spells in this School are now more powerful.");

            foreach (var spell in RuneMagic.Spells)
            {
                if (level == 1 && spell.Level == 1 && spell.Skill == this) { spellInfo += $"{spell.Name} \n"; }
                if (level == 3 && spell.Level == 2 && spell.Skill == this) { spellInfo += $"{spell.Name} \n"; }
                if (level == 5 && spell.Level == 3 && spell.Skill == this) { spellInfo += $"{spell.Name} \n"; }
                if (level == 7 && spell.Level == 4 && spell.Skill == this) { spellInfo += $"{spell.Name} \n"; }
                if (level == 10 && spell.Level == 5 && spell.Skill == this) { spellInfo += $"{spell.Name} \n"; }
            }
            if (spellInfo != "")
            {
                info.Add($"You have learned the following Spells:");
                info.Add($"{spellInfo}");
            }

            return info;
        }

        public override string GetSkillPageHoverText(int level)
        {
            return $"+{level} {Name.Replace(" Skill", "")} Spell Power";
        }
    }
}