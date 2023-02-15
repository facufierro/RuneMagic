using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Invisibility : Spell
    {
        public Invisibility() : base()
        {
            School = School.Illusion;
            Description = "Makes the caster invisible";
            Level = 8;
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
                if (Effect.Timer <= DurationInMilliseconds)
                {
                    Game1.player.hidden.Value = true;
                    var mobs = Game1.currentLocation.characters;
                    foreach (var mob in mobs)
                    {
                        if (mob is StardewValley.Monsters.Monster monster)
                        {
                            monster.moveTowardPlayerThreshold.Value = -1;
                            monster.focusedOnFarmers = false;
                            monster.timeBeforeAIMovementAgain = 50f;
                        }
                    }
                }
                if (Effect.Timer <= 0)
                    Game1.player.hidden.Value = false;
                if (Effect.Timer < 0)
                    Effect = null;
                else
                    Effect.Timer--;
            }
        }

        //public override void Update()
        //{
        //    if (Game1.buffsDisplay.hasBuff(Id))
        //    {
        //    }
        //    else
        //    {
        //        Game1.player.hidden.Value = false;
        //    }
        //}
    }
}