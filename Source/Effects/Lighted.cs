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

        public Lighted(Spell spell, Vector2 target) : base(spell, Duration.Medium)
        {
            Target = target;
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.currentLocation.objects.Add(Target, new Torch(Target, 1));
        }

        public override void End()
        {
            Game1.currentLocation.objects.Remove(Target);
            base.End();
        }
    }
}