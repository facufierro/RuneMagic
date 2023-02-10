using RuneMagic.Source.Interfaces;
using RuneMagic.Source.SpellEffects;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Charming : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get; set; }

        public Charming()
        {
            Name = "Charming";
            Description = "Charms the target for a short time, when the effect ends the target will be annoyed";
            School = School.Illusion;
            CastingTime = 1.0f;
            Level = 1;
            Effect = new CharmingEffect(60);

        }
        public bool Cast()
        {

            //get cursor tile
            var cursorTile = Game1.currentCursorTile;
            //check if there is a character there
            var character = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == cursorTile);

            if (character is not null and NPC)
            {
                (Effect as CharmingEffect).Target = character;
                if (!RuneMagic.PlayerStats.Effects.Contains(Effect))
                {
                    Game1.player.changeFriendship(250, character);
                    RuneMagic.PlayerStats.Effects.Add(Effect);
                    return true;
                }
                else
                {
                    //writh as HUDMessage "I don't think is going to work with more than one at a time"



                    return false;
                }



            }
            else
            {
                return false;


            }
        }
    }
}
