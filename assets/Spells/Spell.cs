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

namespace RuneMagic.assets.Spells

{

    public enum SpellType
    {
        Active,
        Passive,
        Daily,
        Buff
    }
    public enum MagicSchool
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
        public SpellType Type { get; set; }
        public MagicSchool School { get; set; }
        public Farmer Player { get; set; } = Game1.player;
        public Vector2 Cursor { get; set; }
        public bool UsedToday { get; set; } = false;
        public int RuneTexture { get; set; }
        public Color RuneColor { get; set; }

        public abstract bool Cast();

        public Spell()
        {
            School = (MagicSchool)Enum.Parse(typeof(MagicSchool), GetType().Namespace.Split('.').Last());
            RuneTexture = Array.IndexOf(GetType().Assembly.GetTypes(), GetType()) % 5 + 1;
            switch (School)
            {
                case MagicSchool.Abjuration:
                    RuneColor = Color.Teal;
                    break;
                case MagicSchool.Conjuration:
                    RuneColor = Color.Pink;
                    break;
                case MagicSchool.Enchantment:
                    RuneColor = Color.Lime;
                    break;
                case MagicSchool.Evocation:
                    RuneColor = Color.Fuchsia;
                    break;
                case MagicSchool.Alteration:
                    RuneColor = Color.Blue;
                    break;
                case MagicSchool.Illusion:
                    RuneColor = Color.Yellow;
                    break;
            }

        }

    }
}
