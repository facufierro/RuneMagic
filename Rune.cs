using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Object = StardewValley.Object;
using System.Xml.Serialization;

namespace RuneMagic
{
    [XmlType("Mods_Rune")]
    public class Rune : Object
    {

        public int Charges { get; set; }
        public Spell Spell { get; set; }


        public Rune()
        {
        }

        public Rune(Item item)
        {
            Charges = 5;
            Spell = new Spell();

            ParentSheetIndex = item.ParentSheetIndex;
            Stack = item.Stack;
        }
    }
}
