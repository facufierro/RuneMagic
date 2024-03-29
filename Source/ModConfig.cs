﻿using Microsoft.Xna.Framework;
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
        public SButton ActionBarKey { get; set; } = SButton.Q;
        public SButton SpellBookKey { get; set; } = SButton.K;
        public int CastbarScale { get; set; } = 2;
    }
}