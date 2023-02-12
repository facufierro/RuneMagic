using RuneMagic.Source.Interfaces;
using StardewValley;

namespace RuneMagic.Source.SpellEffects
{
    public class CharmingEffect : ISpellEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Timer { get; set; }
        public int Cooldown { get; set; }
        public ISpellEffect Effect { get; set; }
        public Character Target { get; set; }

        public CharmingEffect(int duration)
        {
            Name = "Charming";
            Description = "Charm a target";
            Duration = duration;
            Timer = 0;


        }
        public void Start() { }

        public void Update()
        {
            if (RuneMagic.PlayerStats.Effects.Contains(this))
            {
                if (Timer >= Duration * 60)
                {
                    Game1.player.changeFriendship(-500, Target as NPC);
                    RuneMagic.PlayerStats.Effects.Remove(this);
                    Timer = 0;
                }
                else
                {
                    Timer++;
                }

            }
        }
    }
}
