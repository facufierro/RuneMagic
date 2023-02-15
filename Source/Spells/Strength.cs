using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Strength : Spell
    {
        public Strength() : base()
        {
            School = School.Enchantment;
            Description = "Increases the caster's attack damage.";
            Level = 5;
            Duration = Duration.Medium;
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
                if (Effect.Timer <= DurationInMilliseconds && Game1.player.attackIncreaseModifier < 5)
                    Game1.player.attackIncreaseModifier = 5;
                if (Effect.Timer <= 0)
                    Game1.player.attackIncreaseModifier = 0;
                if (Effect.Timer < 0)
                    Effect = null;
                else
                    Effect.Timer--;

                RuneMagic.Instance.Monitor.Log(Game1.player.addedSpeed.ToString());
            }
        }
    }
}