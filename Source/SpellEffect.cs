﻿using SpaceCore;
using StardewValley;

namespace RuneMagic.Source
{
    public abstract class SpellEffect
    {
        public string Name { get; set; }
        public Spell Spell { get; set; }
        public int Timer { get; set; }
        public Duration Duration { get; set; }

        public SpellEffect(Spell spell, Duration duration)
        {
            Name = $"Glyph of {spell.Name}";
            Duration = duration;
            Spell = spell;
            switch (Duration)
            {
                case Duration.Instant: Timer = 0; break;
                case Duration.Short: Timer = 5 + spell.School.Level * 1000; break;
                case Duration.Medium: Timer = 10 * spell.School.Level * 1000; break;
                case Duration.Long: Timer = 30 * spell.School.Level * 1000; break;
                case Duration.Permanent: Timer = 999999999; break;
            }
        }

        public virtual void Start()
        {
            Player.MagicStats.ActiveEffects.Add(this);
        }

        public virtual void End()
        {
            Player.MagicStats.ActiveEffects.Remove(this);
        }

        public virtual void Update()
        {
            if (Player.MagicStats.ActiveEffects.Contains(this))
            {
                if (Timer == 0)
                {
                    End();
                    return;
                }
                Timer--;
            }
            //RuneMagic.Instance.Monitor.Log($"{Timer}");
        }
    }
}