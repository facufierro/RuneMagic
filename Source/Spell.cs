using Microsoft.Xna.Framework;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using System.Collections.Generic;

namespace RuneMagic.Source
{
    public class Spell
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public School School { get; set; }
        public dynamic Target { get; set; }
        public Buff Buff { get; set; }

        public int Level { get; set; } = 1;
        public float CastingTime { get; set; } = 1;
        public int Duration { get; set; } = 10;

        private static int s_nextId = 0;

        public Spell()
        {
            Id = int.Parse($"15065{s_nextId}");
            s_nextId++;
            Name = GetType().Name;
            CastingTime = 1 + (Level / 10f) * 1.5f;
        }

        public virtual bool Cast()
        { return false; }

        public virtual void Update()
        {
        }

        public List<Color> GetColor()
        {
            List<Color> colors = new();
            switch (School)
            {
                case School.Abjuration:
                    colors.Add(Color.Gray);
                    colors.Add(Color.DarkGray);
                    return colors;

                case School.Alteration:
                    colors.Add(new Color(0, 0, 200));
                    colors.Add(new Color(0, 0, 175));
                    return colors;

                case School.Conjuration:
                    colors.Add(Color.Orange);
                    colors.Add(Color.DarkOrange);
                    return colors;

                case School.Evocation:
                    colors.Add(new Color(200, 0, 200));
                    colors.Add(new Color(175, 0, 175));
                    return colors;

                case School.Enchantment:
                    colors.Add(new Color(0, 200, 0));
                    colors.Add(new Color(0, 175, 0));
                    return colors;

                case School.Illusion:
                    colors.Add(new Color(200, 0, 0));
                    colors.Add(new Color(175, 0, 0));
                    return colors;

                default:
                    colors.Add(Color.Red);
                    colors.Add(Color.White);
                    return colors;
            }
        }
    }
}