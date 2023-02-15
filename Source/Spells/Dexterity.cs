using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Dexterity : Spell
    {
        public Dexterity() : base()
        {
            School = School.Enchantment;
            Description = "Increases the caster's casting speed.";
            Level = 6;
        }

        public override bool Cast()
        {
            Target = Game1.player;

            return false;
        }
    }
}