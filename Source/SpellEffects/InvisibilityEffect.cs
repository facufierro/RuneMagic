using RuneMagic.Source.Interfaces;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.SpellEffects
{
    public class InvisibilityEffect : ISpellEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Timer { get; set; } = 0;
        public int Cooldown { get; set; }
        public bool Active { get; set; } = false;

        public InvisibilityEffect(int duration)
        {
            Name = "Invisibility";
            Description = "Invisible.";
            Duration = duration;

        }
        public void Update()
        {
            if (RuneMagic.PlayerStats.Effects.Contains(this))
            {
                Game1.player.hidden.Value = true;
                //get all the mobs in the screen
                var mobs = Game1.currentLocation.characters;
                //loop through all the mobs
                foreach (var mob in mobs)
                {
                    //if the mob is a monster
                    if (mob is StardewValley.Monsters.Monster monster)
                    {
                        monster.moveTowardPlayerThreshold.Value = -1;
                        monster.focusedOnFarmers = false;
                        monster.timeBeforeAIMovementAgain = 50f;
                        //RuneMagic.Instance.Monitor.LogOnce($"{monster.Name}:{monster.moveTowardPlayerThreshold.Value}", LogLevel.Alert);



                    }
                }
                if (Timer >= Duration * 60)
                {
                    Game1.player.hidden.Value = false;
                    RuneMagic.PlayerStats.Effects.Remove(this);
                    Timer = 0;
                }
                else
                {
                    Timer++;
                }
            }

        }
    }
}
