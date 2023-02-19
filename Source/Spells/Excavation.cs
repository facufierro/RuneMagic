using StardewValley;
using StardewValley.Locations;

namespace RuneMagic.Source.Spells
{
    public class Excavation : Spell
    {
        public Excavation() : base(School.Conjuration)
        {
            Description += "The caster digs a hole at a target location.";
            Level = 5;
        }

        public override bool Cast()
        {
            if (Game1.currentLocation is MineShaft)
            {
                Game1.nextMineLevel();
                return base.Cast();
            }
            else
                return false;
        }
    }
}