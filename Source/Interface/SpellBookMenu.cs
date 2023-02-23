using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using RuneMagic.Source;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Interface
{
    internal class SpellBookMenu : IClickableMenu
    {
        private const int WindowWidth = 640;
        private const int WindowHeight = 480;
        private int RectSize = 16;
        private Point GridSize;
        private Point[,] Grid;

        private List<Rectangle> KnownSpellSlots;
        private List<Rectangle> MemorizedSpellSlots;

        public SpellBookMenu()
            : base((Game1.viewport.Width - WindowWidth) / 2, (Game1.viewport.Height - WindowHeight) / 2, WindowWidth, WindowHeight)
        {
            GridSize = new Point(WindowWidth / RectSize, WindowHeight / RectSize);

            KnownSpellSlots = new List<Rectangle>();
            MemorizedSpellSlots = new List<Rectangle>();

            Grid = new Point[GridSize.X, GridSize.Y];
            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    Grid[x, y] = new Point(xPositionOnScreen + (x * RectSize), yPositionOnScreen + (y * RectSize));
                }
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);
            //check for left click
        }

        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, xPositionOnScreen, yPositionOnScreen, WindowWidth, WindowHeight, Color.White);
            DrawSkillBar(b, RuneMagic.PlayerStats.ActiveSkill);
            DrawKnownSpellSlots(b);
            DrawMemorizedSpellSlots(b);
            if (RuneMagic.Instance.Helper.Input.IsDown(SButton.MouseLeft))
            {
                Point mousePos = new(Game1.getMouseX(), Game1.getMouseY());
                for (int i = 0; i < KnownSpellSlots.Count; i++)
                {
                    int index = i;
                    if (KnownSpellSlots[i].Contains(mousePos))
                    {
                        // Check if spell is already memorized
                        if (!RuneMagic.PlayerStats.MemorizedSpells.Contains(RuneMagic.PlayerStats.KnownSpells[i]))
                        {
                            // If the number of memorized spells is at the maximum, remove the first
                            // (oldest) spell
                            if (RuneMagic.PlayerStats.MemorizedSpells.Count >= 10)
                            {
                                if (index > 10)
                                    index = 0;
                                RuneMagic.PlayerStats.MemorizedSpells.RemoveAt(index);
                            }
                            // Add new spell to the end of the list
                            RuneMagic.PlayerStats.MemorizedSpells.Add(RuneMagic.Spells[i]);
                        }
                        break;
                    }
                }
                RuneMagic.Instance.Monitor.Log($"Memorized Spells");
                foreach (var spell in RuneMagic.PlayerStats.MemorizedSpells)
                    RuneMagic.Instance.Monitor.Log($"-{spell.Name}");
            }
            base.draw(b);
            drawMouse(b);
        }

        private Rectangle GridRectangle(int x, int y, int width, int height)
        {
            return new Rectangle(Grid[x, y], new Point(width * RectSize, height * RectSize));
        }

        private void DrawSkillBar(SpriteBatch b, Skill skill)
        {
            //draw the skill icon at rect[1,1] of rectSize*3

            b.Draw(skill.Icon, GridRectangle(1, 1, 3, 3), Color.White);
            var xOffset = 4;

            for (int i = 0; i < 15; i++)
            {
                Texture2D texture;
                if (i == 4 || i == 9 || i == 14)
                {
                    if (i > skill.Level)
                        texture = RuneMagic.Textures["icon_profession_empty"];
                    else
                        texture = RuneMagic.Textures["icon_profession_filled"];
                    xOffset--;
                    b.Draw(texture, GridRectangle(xOffset + (i * 2), 1, 6, 3), Color.White);

                    xOffset += 2;
                }
                else
                {
                    if (i > skill.Level)
                        texture = RuneMagic.Textures["icon_level_empty"];
                    else
                        texture = RuneMagic.Textures["icon_level_filled"];
                    b.Draw(texture, GridRectangle(xOffset + (i * 2), 1, 3, 3), Color.White);
                }
            }
        }

        private void DrawKnownSpellSlots(SpriteBatch b)
        {
            int xOffset = 2;
            int yOffset = 8;

            // Draw the spell icons for each level
            for (int level = 1; level <= 5; level++)
            {
                // Draw the spell icons for the current level
                if (RuneMagic.PlayerStats.KnownSpells.Count > 0)
                    foreach (var spell in RuneMagic.PlayerStats.KnownSpells.Where(s => s.Level == level))
                    {
                        b.Draw(RuneMagic.Textures["icon_spell_slot_known"], GridRectangle(xOffset, yOffset, 4, 4), Color.White);
                        b.Draw(spell.Icon, GridRectangle(xOffset, yOffset, 4, 4), Color.White);
                        KnownSpellSlots.Add(GridRectangle(xOffset, yOffset, 4, 4));
                        xOffset += 3;
                    }
                xOffset = 2;
                yOffset += 3;
            }
        }

        private void DrawMemorizedSpellSlots(SpriteBatch b)
        {
            int yOffset = 8;
            int xOffset = 30;
            // Draw the memorized spell slots in two columns of 5

            foreach (var spell in RuneMagic.PlayerStats.MemorizedSpells)
            {
                if (RuneMagic.PlayerStats.MemorizedSpells.IndexOf(spell) == 5)
                {
                    yOffset = 8;
                    xOffset += 3;
                }
                b.Draw(RuneMagic.Textures["icon_spell_slot_memorized"], GridRectangle(xOffset, yOffset, 4, 4), Color.White);
                b.Draw(spell.Icon, GridRectangle(xOffset, yOffset, 4, 4), Color.White);
                MemorizedSpellSlots.Add(GridRectangle(xOffset, yOffset, 4, 4));
                yOffset += 3;
            }
        }
    }
}