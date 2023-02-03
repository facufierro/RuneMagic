

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RuneMagic.Source
{
    public enum School
    {
        Abjuration, //protects against magic (warding, protection)
        Alteration, //alters reality (teleportation, tranformation of objects)
        Conjuration, //creates objects from nothing (summons, portals)
        Evocation, //alters the threads of magic itself (fire, lightning, arcane )
        Enchantment, //changes the properties of objects people (enchanting, curses)
        Illusion, //creates illusions (invisibility, phantoms)
    }

    [XmlType("Mods_Spell")]
    public abstract class Spell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public float IsUsable { get; set; }
        public float Circle { get; set; }
        public int Level { get; set; }
        public Vector2 Target { get; set; }

        public abstract bool Cast();

        public Spell()
        {

            CastingTime = 1f;
            Circle = 1;

        }

        public List<Color> GetColor()
        {
            List<Color> colors = new List<Color>();
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
