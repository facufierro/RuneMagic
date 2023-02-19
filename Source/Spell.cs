using Microsoft.Xna.Framework;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static SpaceCore.Skills;

namespace RuneMagic.Source
{
    public class Spell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MagicSkill Skill { get; set; }
        public School School { get; set; }
        public dynamic Target { get; set; }
        public SpellEffect Effect { get; set; }
        public int Level { get; set; } = 1;
        public float CastingTime { get; set; } = 1;

        public Spell(School school)
        {
            Name = GetType().Name;
            CastingTime = 1 + (Level / 10f) * 1.5f;
            School = school;
            Skill = RuneMagic.PlayerStats.MagicSkills[School];
            if (GetType().GetMethod(nameof(Cast)).DeclaringType == typeof(Spell))
            {
                Description = "This spell is not yet implemented.\n".ToUpper();
            }
            else
            {
                Description = "";
            }
        }

        public virtual bool Cast()
        {
            Game1.player.AddCustomSkillExperience(Skill, 5);
            return true;
        }
    }
}