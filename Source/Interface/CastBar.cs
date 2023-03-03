using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class CastBar
    {
        private Texture2D Frame;
        private Texture2D Background;
        private Color BarColor;

        private float Scale;

        public CastBar()
        {
            Frame = RuneMagic.Textures["castbar_frame"];
            Background = RuneMagic.Textures["castbar_background"];
            BarColor = new Color(new Vector4(0, 0, 200, 0.8f));
            Scale = 3f;
            //create a new rectangle at the center of the screen
        }

        public void Render(SpriteBatch spriteBatch, ISpellCastingItem spellCastingItem)
        {
            if (spellCastingItem.Spell == null)
                return;
            var castingTime = spellCastingItem.Spell.CastingTime;
            if (RuneMagic.PlayerStats.CastingTime > 0)
            {
                var x = (int)(spriteBatch.GraphicsDevice.Viewport.Width / 2 - (64 * Scale) / 2);
                var y = (spriteBatch.GraphicsDevice.Viewport.Height / 2 + 32);
                var width = (int)(64 * Scale);
                var height = (int)(64 * Scale);
                spriteBatch.Draw(Background, new Rectangle(x, y, width, height), Color.White);

                x = spriteBatch.GraphicsDevice.Viewport.Width / 2 - 88;
                y = spriteBatch.GraphicsDevice.Viewport.Height / 4 * 3 + 22;
                width = (int)(RuneMagic.PlayerStats.CastingTime / (castingTime * 60) * 58);
                spriteBatch.Draw(Game1.staminaRect, new Rectangle(x, y, (int)(width * Scale), 13), BarColor);

                x = (int)(spriteBatch.GraphicsDevice.Viewport.Width / 2 - (64 * Scale) / 2);
                y = (spriteBatch.GraphicsDevice.Viewport.Height / 2 + 32);
                width = (int)(64 * Scale);
                height = (int)(64 * Scale);
                spriteBatch.Draw(Frame, new Rectangle(x, y, width, height), Color.White);
            }
        }
    }
}