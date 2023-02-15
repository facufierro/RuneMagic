using StardewValley;
using StardewValley.Locations;

namespace RuneMagic.Source.Spells
{
    public class Excavation : Spell
    {
        public Excavation() : base()
        {
            School = School.Evocation;
            Description = "The caster digs a hole at a target location.";
            Level = 7;
        }

        public override bool Cast()
        {
            if (Game1.currentLocation is MineShaft)
            {
                Game1.nextMineLevel();

                return true;
            }
            else
                return false;
        }
    }
}