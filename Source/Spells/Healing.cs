using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Healing : Spell
    {
        public Healing() : base()
        {
            School = School.Abjuration;
            Description = "";
            Level = 2;
        }

        public override bool Cast()
        {
            if (Game1.player.health >= Game1.player.maxHealth)
                return false;

            Game1.player.health += 10;
            return true;
        }
    }
}