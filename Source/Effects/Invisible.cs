using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Effects
{
    public class Invisible : SpellEffect
    {
        public Invisible(string name) : base(name, Duration.Short)
        {
            Name = name;
            Start();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            Game1.player.hidden.Value = false;
            base.Stop();
        }

        public override void Update()
        {
            base.Update();
            Game1.player.hidden.Value = true;
            var mobs = Game1.currentLocation.characters;
            foreach (var mob in mobs)
            {
                if (mob is StardewValley.Monsters.Monster monster)
                {
                    monster.moveTowardPlayerThreshold.Value = -1;
                    monster.focusedOnFarmers = false;
                    monster.timeBeforeAIMovementAgain = 50f;
                }
            }
        }
    }
}