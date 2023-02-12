using Microsoft.Xna.Framework;
using RuneMagic.Source.Interfaces;
using StardewValley;

namespace RuneMagic.Source.SpellEffects
{
    public class WardingEffect : ISpellEffect
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int BuffId { get; set; }
        public Buff Buff { get; set; }

        public WardingEffect(int duration)
        {
            Name = "Glyph of Warding";
            Description = "You are protected from harm.";
            Duration = duration;
            BuffId = 90;
        }
        public void Start()
        {
            RuneMagic.PlayerStats.Effects.Add(this);
            Buff = new Buff(Buff.yobaBlessing)
            {
                millisecondsDuration = Duration * 1000,
                description = Description,
                source = Name,
                displaySource = Name,
                glow = Color.Transparent,
                sheetIndex = 16,
            };
            Game1.buffsDisplay.addOtherBuff(Buff);


        }
        public void Update()
        {

        }
    }
}

