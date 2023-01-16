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
using System.Reflection;
using System.Xml.Linq;
using System.Text;
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
            InitializeSpell();
        }

        public void Activate()
        {
            Charges--;
            if (Spell != null)
                Spell.Cast();
            else
                Console.WriteLine("Spell not found.");
        }
        public void InitializeSpell()
        {


            string spellName = Name.Substring(8);
            //get the spell class namespace
            Type spellType = Assembly.GetExecutingAssembly().GetType("RuneMagic.assets.Spells." + spellName);
            Spell = (Spell)Activator.CreateInstance(spellType);
        }

    }
}

