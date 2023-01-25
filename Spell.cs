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

namespace RuneMagic
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

        public Color GetColor()
        {
            switch (School)
            {
                case School.Abjuration:
                    return Color.LightGray;
                case School.Alteration:
                    return Color.Blue;
                case School.Conjuration:
                    return Color.Orange;
                case School.Evocation:
                    return Color.Purple;
                case School.Enchantment:
                    return Color.Green;
                case School.Illusion:
                    return Color.Yellow;
                default:
                    return Color.White;
            }
        }

    }
}
