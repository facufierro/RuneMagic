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

        public int MaxCharges { get; set; }
        public int CurrentCharges { get; set; }
        public Spell Spell { get; set; }

        public Rune() : base()
        {

        }

        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            MaxCharges = 5;
            CurrentCharges = MaxCharges;
            InitializeSpell();
          
        }



        public void Activate()
        {
            if (CurrentCharges > 0 && Spell != null)
            {
                if (Spell.Cast())
                {
                    CurrentCharges--;
                }
            }
        }


        public void InitializeSpell()
        {
            string spellName = Name.Substring(8);
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType("RuneMagic.assets.Spells." + spellName);
            Spell = (Spell)Activator.CreateInstance(spellType);
        }

        public override bool canBeShipped()
        {
            return false;
        }
        public override bool canBeGivenAsGift()
        {
            return false;
        }

        public override int maximumStackSize()
        {
            return 1;
        }

    }
}

