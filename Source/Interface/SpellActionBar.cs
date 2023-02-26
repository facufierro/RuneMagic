using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RuneMagic.Source.Items;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class SpellActionBar
    {
        public List<SpellSlot> SpellSlots { get; set; }

        public float Scale { get; set; }
        public Texture2D ArrowTexture { get; set; }
        public Vector2 Center { get; set; }

        public SpellActionBar()
        {
            ArrowTexture = RuneMagic.Textures["casting_bar_arrow"];
            Scale = 5f;

            SpellSlots = new List<SpellSlot>();
            for (int i = 0; i < RuneMagic.PlayerStats.MemorizedSpells.Count; i++)
            {
                SpellSlots.Add(new SpellSlot());
            }
        }

        public void Render(SpriteBatch b)
        {
            if (RuneMagic.Instance.Helper.Input.IsDown(SButton.Q) && Game1.player.CurrentItem is SpellBook)
            {
                var memorizedSpells = RuneMagic.PlayerStats.MemorizedSpells;
                var viewport = b.GraphicsDevice.Viewport;
                Center = new Vector2(viewport.Width / 2, viewport.Height / 2);
                var slotSize = new Vector2(128, 128);
                var slotPosition = new Vector2();
                var radius = 150;

                // Convert angleIncrement from radians to degrees
                var angleIncrement = 360f / memorizedSpells.Count;
                var angle = -90f;

                for (int i = 0; i < memorizedSpells.Count; i++)
                {
                    var angleRadians = angle * (Math.PI / 180);
                    slotPosition.X = (float)(Center.X - slotSize.X / 2 + radius * Math.Cos(angleRadians));
                    slotPosition.Y = (float)(Center.Y - slotSize.Y / 2 + radius * Math.Sin(angleRadians));

                    if (memorizedSpells[i] != null)
                        SpellSlots[i].Spell = RuneMagic.PlayerStats.MemorizedSpells[i];
                    else
                        SpellSlots[i].Spell = null;

                    SpellSlots[i].Bounds = new Rectangle((int)slotPosition.X, (int)slotPosition.Y, (int)slotSize.X, (int)slotSize.Y);

                    SpellSlots[i].Render(b);
                    angle += angleIncrement;
                }

                //make the cursor disappear while selecting
                Game1.mouseCursorTransparency = 0;

                if (Game1.player.CurrentItem is SpellBook spellBook)
                {
                    //make the cursor disappear

                    // Find the slot closest to the mouse position
                    var closestSlot = SpellSlots.OrderBy(slot =>
                    {
                        var center = new Vector2(slot.Bounds.Center.X, slot.Bounds.Center.Y);
                        return Vector2.Distance(center, new Vector2(Game1.getMouseX(), Game1.getMouseY()));
                    }).First();

                    // Deselect all non-selected slots
                    foreach (var slot in SpellSlots)
                    {
                        if (slot != closestSlot)
                            slot.Selected = false;
                    }

                    // Select the closest slot and initialize the spell
                    closestSlot.Selected = true;
                    //make closest slot rectangle bigger
                    if (closestSlot.Spell != null)
                    {
                        var lastSize = closestSlot.Bounds;
                        var newSize = new Rectangle(lastSize.X - 20, lastSize.Y - 20, lastSize.Width + 40, lastSize.Height + 40);
                        closestSlot.Render(b, newSize);
                        spellBook.SelectedSlot = closestSlot;
                        spellBook.InitializeSpell();
                    }
                }
            }
        }
    }
}