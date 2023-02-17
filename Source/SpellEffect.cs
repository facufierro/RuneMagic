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
                case Duration.Short: Timer = 5 * 60; break;             //5 minutes ingame
                case Duration.Medium: Timer = 10 * 60; break;           //1 hour ingame
                case Duration.Long: Timer = 4 * 60 * 60; break;         //4 hours ingame
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