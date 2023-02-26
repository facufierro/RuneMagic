using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using RuneMagic.Source;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Interface
{
    public class SpellBookMenu : IClickableMenu
    {
        private const int WindowWidth = 640;
        private const int WindowHeight = 480;
        private int RectSize = 16;
        private Point GridSize;
        private Point[,] Grid;

        private List<SpellSlot> KnownSpellSlots;
        private List<SpellSlot> MemorizedSpellSlots;

        public SpellBookMenu()
            : base((Game1.viewport.Width - WindowWidth) / 2, (Game1.viewport.Height - WindowHeight) / 2, WindowWidth, WindowHeight)
        {
            KnownSpellSlots = new List<SpellSlot>();

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

        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, xPositionOnScreen, yPositionOnScreen, WindowWidth, WindowHeight, Color.White);
            DrawSkillBar(b, RuneMagic.PlayerStats.MagicSkill);
            DrawSlots(b);

            base.draw(b);
            drawMouse(b);
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

        private void DrawSlots(SpriteBatch b)
        {
            MemorizedSpellSlots = new List<SpellSlot>();
            for (int i = 0; i < RuneMagic.PlayerStats.MemorizedSpells.Count; i++)
                MemorizedSpellSlots.Add(new SpellSlot());
            int xOffset = 2;
            int yOffset = 8;
            for (int level = 1; level <= 5; level++)
            {
                foreach (var spell in RuneMagic.PlayerStats.KnownSpells.Where(s => s.Level == level))
                {
                    var slot = new SpellSlot(spell, GridRectangle(xOffset, yOffset, 4, 4));
                    KnownSpellSlots.Add(slot);
                    slot.Render(b);
                    xOffset += 3;
                }
                xOffset = 2;
                yOffset += 3;
            }
            yOffset = 8;

            for (int i = 0; i < 15; i++)
            {
                if (i <= 4)
                    xOffset = 28;
                else if (i <= 9)
                    xOffset = 31;
                else
                    xOffset = 34;

                b.Draw(RuneMagic.Textures["spell_slot_disabled"], GridRectangle(xOffset, yOffset, 4, 4), Color.White);
                yOffset += 3;

                if (i is 4 or 9)
                    yOffset = 8;
            }
            yOffset = 8;
            foreach (var slot in MemorizedSpellSlots)
            {
                var index = MemorizedSpellSlots.IndexOf(slot);
                if (index <= 4)
                    xOffset = 28;
                else if (index <= 9)
                    xOffset = 31;
                else
                    xOffset = 34;

                if (RuneMagic.PlayerStats.MemorizedSpells[index] != null)
                {
                    MemorizedSpellSlots[index].Render(b, GridRectangle(xOffset, yOffset, 4, 4), RuneMagic.PlayerStats.MemorizedSpells[index]);
                }
                else
                {
                    MemorizedSpellSlots[index].Render(b, GridRectangle(xOffset, yOffset, 4, 4), null);
                }
                yOffset += 3;

                if (index is 4 or 9)
                    yOffset = 8;
            }
        }

        public void MemorizeSpell()
        {
            foreach (var knownSlot in KnownSpellSlots)
            {
                if (knownSlot.Bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
                {
                    foreach (var memorizedSlot in MemorizedSpellSlots)
                    {
                        if (RuneMagic.PlayerStats.MemorizedSpells.Contains(null))
                        {
                            int nullIndex = RuneMagic.PlayerStats.MemorizedSpells.FindIndex(x => x == null);
                            RuneMagic.PlayerStats.MemorizedSpells[nullIndex] = knownSlot.Spell;
                            if (RuneMagic.Config.DevMode)
                            {
                                var info = $"Trying to memorize ";
                                if (knownSlot.Spell == null)
                                    info += $"a null spell.\n";
                                else
                                    info += $"{knownSlot.Spell.Name}.\n";

                                info += $"at {nullIndex}.\n";
                                if (memorizedSlot.Spell == null)
                                    info += $"but failed.\n";
                                else
                                    info += $"and succeded. {memorizedSlot.Spell.Name} is memorized at {nullIndex}.\n";
                                RuneMagic.Instance.Monitor.Log(info);
                            }
                            return;
                        }
                    }
                }
            }
            foreach (var memorizedSlot in MemorizedSpellSlots)
            {
                if (memorizedSlot.Bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
                {
                    if (memorizedSlot.Spell != null)
                    {
                        var index = MemorizedSpellSlots.IndexOf(memorizedSlot);
                        RuneMagic.PlayerStats.MemorizedSpells[index] = null;

                        return;
                    }
                }
            }
        }
    }
}