using StardewValley;
using StardewValley.Monsters;

namespace RuneMagic.Source.Spells
{
    public class Command : Spell
    {
        public Command() : base()
        {
            School = School.Illusion;
            Description = "Commands the target monster to die, has a chance to succeed based on luck.";
            Level = 10;
        }

        public override bool Cast()
        {
            //get the monster under the cursor
            var cursorLocation = Game1.currentCursorTile;
            var target = Game1.currentLocation.isCharacterAtTile(cursorLocation);

            var chance = 70 + 100 * Game1.player.DailyLuck;

            if (target is null)
                return false;
            //check if target is a Monster
            if (target is not Monster)
                return false;

            //pick a random number between 0 and 100 and check if it is less than the chance
            if (Game1.random.Next(0, 100) < chance)
            {
                if (target is Monster)
                    (target as Monster).Health = 0;
                return true;
            }
            else
                return false;
        }
    }
}