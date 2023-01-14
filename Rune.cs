using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Object = StardewValley.Object;

namespace RuneMagic
{

    internal class Rune : Object
    {


        public int Charges { get; set; }
        public Spell Spell { get; set; }

        //make a constructor of rune using the constructor for object
        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            Charges = 5;
            Spell = new Spell();

            //apply the parameters to the object
            ParentSheetIndex = parentSheetIndex;
            Stack = stack;
            IsRecipe = isRecipe;

        }
    }
}
