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

        public Rune() : base()
        {
 
        }

        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            Charges = 5;
            InitializeSpell();
            maximumStackSize();
            canBeShipped();
            canBeGivenAsGift();

            //cast drawWhenHeld method and draw nothing on the item position
            //drawWhenHeld(Game1.spriteBatch, Game1.player.getStandingPosition(), Game1.player);
        }



        public void Activate()
        {

            if (Charges >= 0)
            {
                if (Spell != null)
                {
                    if (Spell.Cast())
                    {
                        Charges--;
                    }
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



        //override drawWhenHeld method and not draw anything
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            //do nothing
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
            return Charges;
        }

    }
}

