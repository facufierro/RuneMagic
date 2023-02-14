using RuneMagic.Source.Interfaces;
using StardewModdingAPI;
using StardewValley;
using System.Linq;
using System.Threading;

namespace RuneMagic.Source.Spells
{
    public class Charming : Spell
    {
        public Charming() : base()
        {
            Description = "Charms the target for a short time, when the effect ends the target will be annoyed";
            School = School.Illusion;
            Level = 1;
            Duration = 10;
        }

        public override bool Cast()
        {
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);

            if (Target is not null and NPC)
            {
                if (!Game1.buffsDisplay.hasBuff(Id))
                {
                    Buff = new Buff(Description, Duration * 1000, "Glyph of Charming", 16) { which = Id };
                    Game1.buffsDisplay.addOtherBuff(Buff);
                    Game1.player.changeFriendship(250, Target as NPC);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override void Update()
        {
            if (Buff != null)
            {
                if (Buff.millisecondsDuration == 16 && Game1.buffsDisplay.hasBuff(Id))
                {
                    Game1.player.changeFriendship(-500, Target as NPC);
                }
            }
        }
    }
}