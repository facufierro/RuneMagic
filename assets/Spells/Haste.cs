using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Haste : Spell
    {
        public Haste()

        {
            //target is the player


        }

        public override void Cast()
        {
            Game1.player.addedSpeed = 5;
        }

    }
}
