using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class SpellActionBar
    {
        public SpellSlot[] SpellSlots { get; set; }
        public float Scale { get; set; }
        public Texture2D ArrowTexture { get; set; }
        public Vector2 Center { get; set; }

        public SpellActionBar()
        {
            ArrowTexture = RuneMagic.Textures["casting_bar_arrow"];
            Scale = 5f;
            SpellSlots = new SpellSlot[RuneMagic.PlayerStats.MemorizedSpells.Length];
        }

        public void Render(SpriteBatch b)
        {
            if (RuneMagic.Instance.Helper.Input.IsDown(SButton.Q))
            {
                var viewport = b.GraphicsDevice.Viewport;
                Center = new Vector2(viewport.Width / 2, viewport.Height / 2);
                var slotSize = new Vector2(128, 128);
                var slotPosition = new Vector2();
                var radius = 150;

                // Convert angleIncrement from radians to degrees
                var angleIncrement = 360f / SpellSlots.Length;
                var angle = -90f;

                foreach (var spell in RuneMagic.PlayerStats.MemorizedSpells)
                {
                    // Convert angle from degrees to radians
                    var angleRadians = angle * (Math.PI / 180);

                    slotPosition.X = (float)(Center.X - slotSize.X / 2 + radius * Math.Cos(angleRadians));
                    slotPosition.Y = (float)(Center.Y - slotSize.Y / 2 + radius * Math.Sin(angleRadians));
                    SpellSlot slot;
                    if (spell is not null)
                        slot = new SpellSlot(spell, new Rectangle((int)slotPosition.X, (int)slotPosition.Y, (int)slotSize.X, (int)slotSize.Y));
                    else
                        slot = new SpellSlot(null, new Rectangle((int)slotPosition.X, (int)slotPosition.Y, (int)slotSize.X, (int)slotSize.Y));

                    slot.Render(b);

                    angle += angleIncrement;
                }
            }
        }
    }
}