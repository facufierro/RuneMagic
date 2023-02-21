using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source
{
    public class ModConfig
    {
        public bool DevMode { get; set; } = false;
        public SButton CastKey { get; set; } = SButton.R;
        //public SButton RunemasterKey { get; set; } = SButton.Q;
    }
}