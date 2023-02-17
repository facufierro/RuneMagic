using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace RuneMagic.Source.Effects
{
    public class Lighted : SpellEffect
    {
        private Vector2 Target;

        public Lighted(string name, Vector2 target) : base(name, Duration.Short)
        {
            Name = name;
            Target = target;
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.currentLocation.objects.Add(Target, new Torch(Target, 1));
        }

        public override void Stop()
        {
            Game1.currentLocation.objects.Remove(Target);
            base.Stop();
        }
    }
}