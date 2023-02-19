using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class Regenerating : SpellEffect
    {
        public Regenerating(Spell spell) : base(spell, Duration.Medium)
        {
            Start();
        }

        public override void Update()
        {
            base.Update();
            if (Game1.player.stamina < Game1.player.MaxStamina)
                Game1.player.stamina += 0.01f;
        }
    }
}