using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Teleportation : Spell
    {
        public Teleportation() : base(School.Alteration)
        {
            Description += "Teleports the caster home.";
            Level = 5;
        }

        public override bool Cast()
        {
            Game1.warpFarmer("FarmHouse", 4, 3, false);
            return base.Cast();
        }
    }
}