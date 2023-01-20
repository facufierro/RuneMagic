using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using RuneMagic.assets.Spells.Effects;
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
        Enchanting, //changes the properties of objects people (enchanting, curses)
        Illusion, //creates illusions (invisibility, phantoms)
    }

    [XmlType("Mods_Spell")]
    public abstract class Spell
    {
        public SpellType Type { get; set; }
        public MagicSchool School { get; set; }
        public Farmer Player { get; set; } = Game1.player;
        public Vector2 Cursor { get; set; }
        public int CurrentCooldown { get; set; }
        public int MaxCooldown { get; set; }
        public bool UsedToday { get; set; } = false;

        public abstract bool Cast();

    }
}
