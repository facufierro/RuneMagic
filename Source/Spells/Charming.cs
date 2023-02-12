using RuneMagic.Source.Interfaces;
using StardewModdingAPI;
using StardewValley;
using System.Linq;
using System.Threading;

namespace RuneMagic.Source.Spells
{
    public class Charming : ISpell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }
        public int Duration { get; set; }
        public Character Target { get; set; }

        public Charming()
        {
            Id = 15065001;
            Name = "Charming";
            Description = "Charms the target for a short time, when the effect ends the target will be annoyed";
            School = School.Illusion;
            CastingTime = 1.0f;
            Level = 1;
            Duration = 10;
        }

        public bool Cast()
        {
            var cursorTile = Game1.currentCursorTile;
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == cursorTile);

            if (Target is not null and NPC)
            {
                if (!Game1.buffsDisplay.hasBuff(Id))
                {
                    Buff = new Buff("A character has been charmed by this glyph", Duration * 1000, "Glyph of Charming", 16) { which = Id };
                    Game1.buffsDisplay.addOtherBuff(Buff);
                    Game1.player.changeFriendship(250, Target as NPC);
                    return true;
                }
                else
                {
                    //Game1.addHUDMessage(new HUDMessage("I can only charm one character at a time."));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Update()
        {
            if (Buff != null)
            {
                if (Buff.millisecondsDuration == 16 && Game1.buffsDisplay.hasBuff(Id))
                {
                    RuneMagic.Instance.Monitor.Log($"{Buff.millisecondsDuration}", LogLevel.Alert);
                    Game1.player.changeFriendship(-250, Target as NPC);
                }
            }
        }
    }
}