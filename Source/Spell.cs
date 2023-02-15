using Microsoft.Xna.Framework;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using System.Collections.Generic;

namespace RuneMagic.Source
{
    public class Spell
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public School School { get; set; }
        public dynamic Target { get; set; }
        public SpellEffect Effect { get; set; }

        public int Level { get; set; } = 1;
        public float CastingTime { get; set; } = 1;
        public Duration Duration { get; set; } = Duration.Instant;

        private static readonly Dictionary<School, List<Color>> s_schoolColors = new()
        {
            { School.Abjuration, new List<Color> { Color.Gray, Color.DarkGray } },
            { School.Alteration, new List<Color> { new Color(0, 0, 200), new Color(0, 0, 175) } },
            { School.Conjuration, new List<Color> { Color.Orange, Color.DarkOrange } },
            { School.Evocation, new List<Color> { new Color(200, 0, 200), new Color(175, 0, 175) } },
            { School.Enchantment, new List<Color> { new Color(0, 200, 0), new Color(0, 175, 0) } },
            { School.Illusion, new List<Color> { new Color(200, 0, 0), new Color(175, 0, 0) } },
        };

        public Spell()
        {
            Name = GetType().Name;
            CastingTime = 1 + (Level / 10f) * 1.5f;
        }

        public virtual bool Cast()
        {
            return false;
        }

        public virtual void Update()
        { }

        public List<Color> GetColor()
        {
            if (s_schoolColors.TryGetValue(School, out List<Color> colors))
            {
                return colors;
            }
            else
            {
                return new List<Color> { Color.Red, Color.White };
            }
        }

        public int DurationInMilliseconds => Duration switch
        {
            Duration.Instant => 0,
            Duration.Short => 10 * 60,
            Duration.Medium => 60 * 60,
            Duration.Long => 5 * 60 * 60,
            Duration.Permanent => 999999999,
            _ => 0
        };
    }
}