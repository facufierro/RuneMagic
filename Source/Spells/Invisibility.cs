using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using StardewValley;
using System;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : Spell
    {
        public Invisibility()
        {
            Name = "Invisibility";
            School = School.Illusion;
            Description = "Makes the caster invisible";
            Level = 6;
        }

        public override bool Cast()
        {
            if (!Game1.buffsDisplay.hasBuff(Id))
            {
                Buff = new(Id) { which = Id, millisecondsDuration = Duration * 1000 };
                Game1.buffsDisplay.addOtherBuff(Buff);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update()
        {
            if (Game1.buffsDisplay.hasBuff(Id))
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
                    }
                }
            }
            else
            {
                Game1.player.hidden.Value = false;
            }
        }
    }
}