using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace RuneMagic.Source.Effects
{
    public class Hastened : SpellEffect
    {
        public Hastened(string name) : base(name, Duration.Short)
        {
            Name = name;
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.player.addedSpeed = 5;
        }

        public override void Stop()
        {
            Game1.player.addedSpeed = 0;
            base.Stop();
        }
    }
}