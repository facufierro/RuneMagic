﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RuneMagic.Source
{
    public class MagicSkill
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; } = 0;
        public Texture2D Icon { get; set; }
        public Tuple<Color, Color> Colors { get; set; }

        public MagicSkill(School school)
        {
            Name = school.ToString();
            School = school;
            Icon = RuneMagic.Textures[$"icon_{school.ToString().ToLower()}"];
            Experience = 0;
            Level = 0;
            switch (school)
            {
                case School.Abjuration:
                    Description = "";
                    Colors = new(new Color(200, 200, 200), new Color(175, 175, 175));
                    break;

                case School.Alteration:
                    Description = "";
                    Colors = new(new Color(0, 0, 200), new Color(0, 0, 175));
                    break;

                case School.Conjuration:
                    Description = "";
                    Colors = new(new Color(200, 0, 200), new Color(200, 0, 175));
                    break;

                case School.Evocation:
                    Description = "";
                    Colors = new(new Color(200, 0, 0), new Color(175, 0, 0));
                    break;
            }
        }

        public void SetLevel()
        {
            if (Level > 15)
            {
                Level = 15;
            }
            Level = (int)Math.Floor(Math.Pow(Experience / 100, 1.2));
        }
    }
}