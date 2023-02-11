using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Interfaces;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.SpellEffects
{
    public class LightEffect : Torch, ISpellEffect
    {
        public int Timer { get; set; } = 0;
        public int Duration { get; set; } = 10;
        public int Cooldown { get; set; }
        public string Description { get; set; }

        public LightEffect()
        {

        }
        public LightEffect(Vector2 tileLocation, int index, bool bigCraftable)
          : base(tileLocation, index)
        {
            boundingBox.Value = new Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
        }
        public void Start() { }
        public void Update()
        {
            throw new NotImplementedException();
        }
        public override void updateWhenCurrentLocation(GameTime time, GameLocation environment)
        {
            base.updateWhenCurrentLocation(time, environment);

            if (Timer > Duration * 60)
            {
                environment.objects.Remove(tileLocation.Value);
                Timer = 0;
            }
            else
            {
                Timer++;
            }
        }
        public override bool performToolAction(Tool t, GameLocation location)
        {
            //destroy the object
            location.objects.Remove(tileLocation.Value);
            return false;
        }

    }
}
