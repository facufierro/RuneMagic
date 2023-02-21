using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class Warded : SpellEffect
    {
        public Warded(Spell spell) : base(spell, Duration.Short)
        {
            Start();
        }

        public override void Start()
        {
            Game1.buffsDisplay.addOtherBuff(new Buff(21)
            {
                which = 15065,
                millisecondsDuration = 999999,
                sheetIndex = 29,
                glow = Color.Gray
            });
            base.Start();
        }

        public override void End()
        {
            Game1.buffsDisplay.removeOtherBuff(15065);
            base.End();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}