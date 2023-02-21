using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace RuneMagic.Source.Effects
{
    public class Cleansed : SpellEffect
    {
        public Cleansed(Spell spell) : base(spell, Duration.Short)
        {
            Start();
        }

        public override void Update()
        {
            base.Update();
            Game1.buffsDisplay.clearAllBuffs();
        }
    }
}