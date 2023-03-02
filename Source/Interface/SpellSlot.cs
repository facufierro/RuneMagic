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
        public bool Active { get; set; } = true;
        public Rectangle Bounds { get; set; }
        public Color Color { get; set; }

        public SpellSlot()
        { Color = Color.White; }

        public SpellSlot(Spell spell) : this()
        {
            Spell = spell;
        }

        public SpellSlot(Spell spell, Rectangle rectangle) : this(spell)
        {
            Spell = spell;
            Bounds = rectangle;
        }

        public void Render(SpriteBatch b)
        {
            if (Active)
            {
                if (Spell != null)
                {
                    Icon = Spell.Icon;
                    if (Selected)
                        Texture = RuneMagic.Textures["spell_slot_empty"];
                    else
                        Texture = RuneMagic.Textures["spell_slot_filled"];
                    b.Draw(Texture, Bounds, Color);
                    b.Draw(Icon, new Rectangle(Bounds.X + 5, Bounds.Y + 5, Bounds.Width - 10, Bounds.Height - 10), Color.White);
                }
                else
                {
                    Icon = null;
                    Texture = RuneMagic.Textures["spell_slot_empty"];
                    b.Draw(Texture, Bounds, Color);
                }
            }
            else
            {
                Texture = RuneMagic.Textures["spell_slot_disabled"];
                b.Draw(Texture, Bounds, Color);
            }
        }

        public void Render(SpriteBatch b, Rectangle bounds)
        {
            Bounds = bounds;
            if (Active)
            {
                if (Spell != null)
                {
                    Icon = Spell.Icon;
                    if (Selected)
                        Texture = RuneMagic.Textures["spell_slot_empty"];
                    else
                        Texture = RuneMagic.Textures["spell_slot_filled"];
                    b.Draw(Texture, Bounds, Color);
                    b.Draw(Icon, new Rectangle(Bounds.X + 5, Bounds.Y + 5, Bounds.Width - 10, Bounds.Height - 10), Color.White);
                }
                else
                {
                    Icon = null;
                    Texture = RuneMagic.Textures["spell_slot_empty"];
                    b.Draw(Texture, Bounds, Color);
                }
            }
            else
            {
                Texture = RuneMagic.Textures["spell_slot_disabled"];
                b.Draw(Texture, Bounds, Color);
            }
        }

        public override string ToString()
        {
            return $"Spell: {Spell}, Coordenates: {Bounds.X}, {Bounds.Y}, Size: {Bounds.Size.X}, {Bounds.Size.Y}";
        }
    }
}