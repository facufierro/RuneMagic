using RuneMagic.Famework;
using RuneMagic.Magic;
using StardewModdingAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace RuneMagic.Framework
{
    public abstract class MagicItem : StardewValley.Object
    {
        public float GlobalCooldown { get; set; }
        public float GlobalCooldownMax { get; set; }
        public Spell Spell { get; set; }


        public MagicItem()
        {

        }
        public MagicItem(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {

        }

        public abstract void InitializeSpell();
        public abstract void Use();
        public abstract bool Fizzle();
        public void UpdateCooldown()
        {

            if (GlobalCooldown > 0)
            {
                GlobalCooldown -= 0.01f;
            }
        }

    }
}
