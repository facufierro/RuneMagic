using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source
{
    public class RuneMagic
    {
        public PlayerStats PlayerStats { get; set; }
        public Farmer Farmer { get; set; } = Game1.player;

        public RuneMagic()
        {
            PlayerStats = new PlayerStats();
        }
    }
}
