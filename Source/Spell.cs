using Microsoft.Xna.Framework;
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
            switch (School)
            {
                case School.Abjuration:
                    return new List<Color> { new Color(200, 200, 200), new Color(175, 175, 175) };

                case School.Alteration:
                    return new List<Color> { new Color(0, 0, 200), new Color(0, 0, 175) };

                case School.Conjuration:
                    return new List<Color> { Color.Orange, Color.DarkOrange };

                case School.Evocation:
                    return new List<Color> { new Color(200, 0, 200), new Color(175, 0, 175) };

                case School.Enchantment:
                    return new List<Color> { new Color(0, 200, 0), new Color(0, 175, 0) };

                case School.Illusion:
                    return new List<Color> { new Color(200, 0, 0), new Color(175, 0, 0) };

                default:
                    return new List<Color> { Color.White, Color.Black };
            }
        }

        public int DurationInMilliseconds => Duration switch
        {
            Duration.Instant => 0,
            Duration.Short => 5 * 60,
            Duration.Medium => 60 * 60,
            Duration.Long => 4 * 60 * 60,
            Duration.Permanent => 999999999,
            _ => 0
        };
    }
}