using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using RuneMagic.Source;
using RuneMagic.Source.Items;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Interface
{
    public class ScrollScribingMenu : IClickableMenu
    {
        private const int WindowWidth = 640;
        private const int WindowHeight = 480;
        private int RectSize = 16;
        private Point GridSize;
        private Point[,] Grid;
        private SpellBook SpellBook;

        public ScrollScribingMenu(SpellBook spellBook)
            : base((Game1.viewport.Width - WindowWidth) / 2, (Game1.viewport.Height - WindowHeight) / 2, WindowWidth, WindowHeight, true)
        {
            SpellBook = spellBook;
            SetGrid();
        }

        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, xPositionOnScreen, yPositionOnScreen, WindowWidth, WindowHeight, Color.White);
            DrawSkillBar(b, RuneMagic.PlayerStats.MagicSkill);
            DrawKnownSlots(b);
            DrawMemorizedSlots(b);
            base.draw(b);
            drawMouse(b);
        }

        private void SetGrid()
        {
            GridSize = new Point(WindowWidth / RectSize, WindowHeight / RectSize);

            Grid = new Point[GridSize.X, GridSize.Y];
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    Grid[x, y] = new Point(xPositionOnScreen + (x * RectSize), yPositionOnScreen + (y * RectSize));
                }
            }
        }

        private Rectangle GridRectangle(int x, int y, int width, int height)
        {
            return new Rectangle(Grid[x, y], new Point(width * RectSize, height * RectSize));
        }

        private void DrawSkillBar(SpriteBatch b, MagicSkill skill)
        {
            //draw the skill icon at rect[1,1] of rectSize*3

            b.Draw(skill.Icon, GridRectangle(1, 1, 3, 3), Color.White);
            var xOffset = 4;

            for (int i = 0; i < 15; i++)
            {
                Texture2D texture;
                if (i == 4 || i == 9 || i == 14)
                {
                    if (i >= skill.Level)
                        texture = RuneMagic.Textures["icon_profession_empty"];
                    else
                        texture = RuneMagic.Textures["icon_profession_filled"];
                    xOffset--;
                    b.Draw(texture, GridRectangle(xOffset + (i * 2), 1, 6, 3), Color.White);

                    xOffset += 2;
                }
                else
                {
                    if (i >= skill.Level)
                        texture = RuneMagic.Textures["icon_level_empty"];
                    else
                        texture = RuneMagic.Textures["icon_level_filled"];
                    b.Draw(texture, GridRectangle(xOffset + (i * 2), 1, 3, 3), Color.White);
                }
            }
        }

        private void DrawKnownSlots(SpriteBatch b)
        {
            SpellBook.KnownSpellSlots.Clear();

            int xOffset = 2;
            int yOffset = 8;
            for (int level = 1; level <= 5; level++)
            {
                foreach (var spell in RuneMagic.PlayerStats.KnownSpells.Where(s => s.Level == level))
                {
                    var slot = new MagicButton(spell, GridRectangle(xOffset, yOffset, 4, 4));
                    SpellBook.KnownSpellSlots.Add(slot);
                    slot.Render(b);
                    xOffset += 3;
                }
                xOffset = 2;
                yOffset += 3;
            }
        }

        private void DrawMemorizedSlots(SpriteBatch b)
        {
            //var memorizedSpells = RuneMagic.PlayerStats.MemorizedSpells;
            int yOffset = 8;
            int xOffset = 28;
            foreach (var slot in SpellBook.MemorizedSpellSlots)
            {
                var index = SpellBook.MemorizedSpellSlots.IndexOf(slot);
                if (index < 5)
                    xOffset = 28;
                else if (index < 10)
                    xOffset = 31;
                else if (index < 15)
                    xOffset = 34;

                if (index >= RuneMagic.PlayerStats.MemorizedSpells.Count)
                {
                    slot.Active = false;
                }

                slot.Bounds = GridRectangle(xOffset, yOffset, 4, 4);
                slot.Render(b);
                yOffset += 3;

                if (index == 4 || index == 9)
                    yOffset = 8;
            }
        }

        public void MemorizeSpell()
        {
            //if the current location is home
            if (Game1.currentLocation.Name == "FarmHouse")
            {
                foreach (var knownSlot in SpellBook.KnownSpellSlots)
                {
                    if (knownSlot.Bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        foreach (var memorizedSlot in SpellBook.MemorizedSpellSlots)
                        {
                            if (RuneMagic.PlayerStats.MemorizedSpells.Contains(null))
                            {
                                int nullIndex = RuneMagic.PlayerStats.MemorizedSpells.FindIndex(x => x == null);
                                RuneMagic.PlayerStats.MemorizedSpells[nullIndex] = knownSlot.Spell;
                                SpellBook.MemorizedSpellSlots[nullIndex].Spell = knownSlot.Spell;
                                //RuneMagic.Instance.Monitor.Log($"{memorizedSlot.Spell.Name}");

                                return;
                            }
                        }
                    }
                }
                foreach (var memorizedSlot in SpellBook.MemorizedSpellSlots)
                {
                    if (memorizedSlot.Bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        if (memorizedSlot.Spell != null)
                        {
                            var index = SpellBook.MemorizedSpellSlots.IndexOf(memorizedSlot);
                            RuneMagic.PlayerStats.MemorizedSpells[index] = null;
                            memorizedSlot.Spell = null;
                            memorizedSlot.Selected = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}