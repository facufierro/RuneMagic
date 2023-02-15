using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Teleportation : Spell
    {
        public Teleportation() : base()
        {
            School = School.Alteration;
            Description = "Teleports the caster to their home.";
            Level = 8;
        }

        public override bool Cast()
        {
            Game1.warpFarmer("FarmHouse", 4, 3, false);
            return true;
        }
    }
}