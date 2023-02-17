using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;

namespace RuneMagic.Source
{
    public abstract class SpellEffect
    {
        public string Name { get; set; }
        public int Timer { get; set; }
        public Duration Duration { get; set; }

        public SpellEffect(string name, Duration duration)
        {
            Name = $"Glyph of {name}";
            Duration = duration;
            switch (Duration)
            {
                case Duration.Instant: Timer = 0; break;
                case Duration.Short: Timer = (5 + Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill)) * 60; break;
                case Duration.Medium: Timer = (10 * Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill)) * 60; break;
                case Duration.Long: Timer = (30 * Game1.player.GetCustomSkillLevel(RuneMagic.PlayerStats.MagicSkill)) * 60; break;
                case Duration.Permanent: Timer = 999999999; break;
            }
        }

        public virtual void Start()
        {
            RuneMagic.PlayerStats.ActiveEffects.Add(this);
        }

        public virtual void Stop()
        {
            RuneMagic.PlayerStats.ActiveEffects.Remove(this);
        }

        public virtual void Update()
        {
            if (RuneMagic.PlayerStats.ActiveEffects.Contains(this))
            {
                if (Timer == 0)
                {
                    Stop();
                    return;
                }
                Timer--;
            }
            RuneMagic.Instance.Monitor.Log($"{Timer}");
        }
    }
}