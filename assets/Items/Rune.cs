using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Threading;
using RuneMagic.assets.Spells;

namespace RuneMagic.assets.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object
    {

        public int Charges { get; set; }
        public Spell Spell { get; set; }


        public Rune()
        {
        }

        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            Charges = 5;
            Spell = new Spell();
        }

        public void Activate(Item rune)
        {

            string runeName = rune.Name.Substring(8);
            switch (runeName)
            {
                case "Magic":
                    break;
                case "Translocation":
                    Spell.Translocation();
                    break;
                case "Water":
                    Spell.Water();
                    break;
                default:
                    break;

            }
            Charges--;
        }
    }
}

