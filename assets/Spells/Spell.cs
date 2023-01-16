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

    [XmlType("Mods_Spell")]
    public abstract class Spell
    {
        public SpellType Type { get; set; }
        public Farmer Player { get; set; } = Game1.player;
        public Vector2 Cursor { get; set; }
        public int CurrentCooldown { get; set; }
        public int MaxCooldown { get; set; }
        public bool UsedToday { get; set; } = false;

        public abstract void Cast();

    }
}
