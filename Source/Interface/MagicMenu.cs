using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Items;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interface
{
    public class MagicMenu : IClickableMenu
    {
        public const int WindowWidth = 640;
        public const int WindowHeight = 480;
        public int RectSize = 16;
        public Point GridSize;
        public Point[,] Grid;
        public SpellBook SpellBook;

        public MagicMenu(SpellBook spellBook)
            : base((Game1.viewport.Width - WindowWidth) / 2, (Game1.viewport.Height - WindowHeight) / 2, WindowWidth, WindowHeight, true)
        {
            SpellBook = spellBook;
            SetGrid();
        }

        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, xPositionOnScreen, yPositionOnScreen, WindowWidth, WindowHeight, Color.White);
            DrawSkillBar(b, RuneMagic.PlayerStats.MagicSkill);
            base.draw(b);
        }

        public Rectangle GridRectangle(int x, int y, int width, int height)
        {
            return new Rectangle(Grid[x, y], new Point(width * RectSize, height * RectSize));
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
    }
}