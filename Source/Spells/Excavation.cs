using RuneMagic.Source.Interfaces;
using StardewValley;
using StardewValley.Locations;

namespace RuneMagic.Source.Spells
{
    public class Excavation : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public Excavation() : base()
        {
            Name = "Excavation";
            School = School.Evocation;
            Description = "The caster digs a hole at a target location.";
            Level = 7;
            CastingTime = 2;
        }

        public bool Cast()
        {
            if (Game1.currentLocation is MineShaft)
            {
                var currentMineLevel = Game1.mine.mineLevel;
                //warp to currentMineLevel + 1
                Game1.nextMineLevel();

                return true;
            }
            else
            {
                //say "You can only cast this spell in a mine."
                var message = "You can only cast this spell in a mine.";
                Game1.addHUDMessage(new HUDMessage(message, 2));

                return false;
            }
        }

        public void Update()
        { }
    }
}