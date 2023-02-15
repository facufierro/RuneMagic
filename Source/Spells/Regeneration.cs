using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Regeneration : Spell
    {
        public Regeneration() : base()
        {
            School = School.Enchantment;
            Description = "Slowly regenerates the caster's health.";
            Level = 4;
            Duration = Duration.Long;
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
                if (Effect.Timer <= DurationInMilliseconds && Game1.player.Stamina < Game1.player.MaxStamina)
                    Game1.player.Stamina += 0.01f;
                if (Effect.Timer < 0)
                    Effect = null;
                else
                    Effect.Timer--;
            }
        }
    }
}