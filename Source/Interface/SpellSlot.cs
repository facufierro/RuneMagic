using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Interface
{
    public class SpellSlot
    {
        public Spell Spell { get; set; }
        public Texture2D Icon { get; set; }
        public Texture2D Texture { get; set; }
        public bool Selected { get; set; } = false;
        public Rectangle Rectangle { get; set; }
        public Color Color { get; set; }

        public SpellSlot(Spell spell, Rectangle rectangle)
        {
            Spell = spell;
            Rectangle = rectangle;
            Color = Color.White;
        }

        public override string ToString()
        {
            return $"Spell: {Spell}, Coordenates: {Rectangle.X}, {Rectangle.Y}, Size: {Rectangle.Size.X}, {Rectangle.Size.Y}";
        }

        public void Render(SpriteBatch b)
        {
            if (Spell != null)
            {
                Icon = Spell.Icon;
                Texture = RuneMagic.Textures["spell_slot_filled"];
                b.Draw(Texture, Rectangle, Color);
                b.Draw(Icon, new Rectangle(Rectangle.X + 5, Rectangle.Y + 5, Rectangle.Width - 10, Rectangle.Height - 10), Color.White);
            }
            else
            {
                Icon = null;
                Texture = RuneMagic.Textures["spell_slot_empty"];
                b.Draw(Texture, Rectangle, Color);
            }
        }
    }
}