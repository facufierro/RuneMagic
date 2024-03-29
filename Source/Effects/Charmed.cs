﻿using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace RuneMagic.Source.Effects
{
    public class Charmed : SpellEffect
    {
        private NPC Target;

        public Charmed(Spell spell, NPC target) : base(spell, Duration.Medium)
        {
            Target = target;
            Start();
        }

        public override void Start()
        {
            base.Start();
            Game1.player.changeFriendship(250, Target);
        }

        public override void End()
        {
            Game1.player.changeFriendship(-250, Target);
            base.End();
        }
    }
}