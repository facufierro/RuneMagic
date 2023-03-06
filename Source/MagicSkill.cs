using Microsoft.Xna.Framework;
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
            if (Experience < 100)
                Level = 0;
            else if (Experience < 300)
                Level = 1;
            else if (Experience < 600)
                Level = 2;
            else if (Experience < 1000)
                Level = 3;
            else if (Experience < 1500)
                Level = 4;
            else if (Experience < 2100)
                Level = 5;
            else if (Experience < 2800)
                Level = 6;
            else if (Experience < 3600)
                Level = 7;
            else if (Experience < 4500)
                Level = 8;
            else if (Experience < 5500)
                Level = 9;
            else if (Experience < 6600)
                Level = 10;
            else if (Experience < 7800)
                Level = 11;
            else if (Experience < 9100)
                Level = 12;
            else if (Experience < 10500)
                Level = 13;
            else if (Experience < 12000)
                Level = 14;
            else if (Experience >= 12000)
                Level = 15;
        }
    }
}