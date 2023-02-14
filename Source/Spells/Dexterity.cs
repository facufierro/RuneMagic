using RuneMagic.Source.Interfaces;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Dexterity : Spell
    {
        public Dexterity()
        {
            Name = "Dexterity";
            School = School.Alteration;
            Description = "Increases your dexterity.";
            Level = 1;
            CastingTime = 1;
        }

        public override bool Cast()
        {
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);

            if (Target is not null and NPC)
            {
                if (!Game1.buffsDisplay.hasBuff(Id))
                {
                    Buff = new Buff("You feel fingers move faster.", Duration * 1000, $"Glyph of {Name}", 16) { which = Id };
                    Game1.buffsDisplay.addOtherBuff(Buff);
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
                }
            }
        }
    }
}