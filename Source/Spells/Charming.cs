using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Charming : Spell
    {
        public Charming() : base()
        {
            Description = "Charms the target for a short time, when the effect ends the target will be annoyed";
            School = School.Illusion;
            Level = 2;
            Duration = Duration.Medium;
        }

        public override bool Cast()
        {
            Target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);

            if (Target is not null and NPC)
            {
                Effect ??= new SpellEffect(Name, DurationInMilliseconds);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update()
        {
            if (Effect is not null)
            {
                if (Effect.Timer == DurationInMilliseconds)
                    Game1.player.changeFriendship(250, Target);
                if (Effect.Timer == 0)
                {
                    Game1.player.changeFriendship(-250, Target);
                    Effect = null;
                }
                else
                    Effect.Timer--;
            }
        }
    }
}