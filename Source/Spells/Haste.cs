using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Level = 4;
            Duration = Duration.Short;
        }

        public override bool Cast()
        {
            if (Effect is null)
            {
                Effect = new SpellEffect(Name, DurationInMilliseconds);
                return true;
            }
            else
                return false;
        }

        public override void Update()
        {
            if (Effect is not null)
            {
                if (Effect.Timer <= DurationInMilliseconds && Game1.player.addedSpeed < 5)
                    Game1.player.addedSpeed = 5;
                if (Effect.Timer <= 0)
                    Game1.player.addedSpeed = 0;
                if (Effect.Timer < 0)
                    Effect = null;
                else
                    Effect.Timer--;
            }
        }
    }
}