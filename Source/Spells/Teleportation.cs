

using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Teleportation : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }

        public Teleportation() : base()
        {
            Name = "Teleportation";
            School = School.Alteration;
            Description = "Teleports the caster to their home.";
            Level = 7;
            CastingTime = 5;
        }

        public bool Cast()
        {
            Game1.warpFarmer("FarmHouse", 4, 3, false);
            return true;


        }
    }
}
