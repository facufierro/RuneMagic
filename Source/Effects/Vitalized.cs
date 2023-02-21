using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class Vitalized : SpellEffect
    {
        public Vitalized(Spell spell) : base(spell, Duration.Long)
        {
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.player.stamina += 50;
            Game1.player.health += 50;
        }

        public override void End()
        {
            Game1.player.stamina -= 50;
            Game1.player.health -= 50;
            base.End();
        }
    }
}