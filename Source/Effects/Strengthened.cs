using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class Strengthened : SpellEffect
    {
        public Strengthened(string name) : base(name, Duration.Short)
        {
            Name = name;
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.player.attackIncreaseModifier = 5;
        }

        public override void Stop()
        {
            Game1.player.attackIncreaseModifier = 0;
            base.Stop();
        }
    }
}