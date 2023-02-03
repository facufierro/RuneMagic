

using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Teleportation : Spell
    {
        private bool UsedToday = false;

        public Teleportation() : base()
        {
            Name = "Teleportation";
            School = School.Alteration;
            Description = "Teleports the caster to their home.";
            Level = 7;
        }

        public override bool Cast()
        {
            if (!UsedToday)
            {
                Game1.warpFarmer("FarmHouse", 4, 3, false);
                UsedToday = true;
                return true;
            }
            else
            {
                Game1.drawObjectDialogue("I can't use this spell again today.");
                return false;
            }

        }
    }
}
