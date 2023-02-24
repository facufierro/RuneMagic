using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class SpellActionBar : IClickableMenu
    {
        private const int WindowWidth = 64 * 5;
        private const int WindowHeight = 64;

        public SpellActionBar()
           : base((Game1.viewport.Width - WindowWidth) / 2, (Game1.viewport.Height - WindowHeight) / 2, WindowWidth, WindowHeight)
        {
        }
    }
}