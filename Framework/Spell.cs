using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;

namespace RuneMagic.Magic
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
        public string Glyph { get; set; }

        public abstract bool Cast();

        public Spell()
        {
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
