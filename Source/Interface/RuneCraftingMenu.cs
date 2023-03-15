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
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;
using static SpaceCore.Skills;

namespace RuneMagic.Source.Interface
{
    public class RuneCraftingMenu : MagicMenu
    {
        private MagicButton Rune;
        private Rune RuneItem;

        public RuneCraftingMenu(SpellBook spellBook)
            : base(spellBook)
        {
            Rune = new();
            RuneItem = new();
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            DrawKnownSlots(b);
            DrawRune(b);
            DrawTooltip(b);
            drawMouse(b);
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

        private void DrawRune(SpriteBatch b)
        {
            Rune.Bounds = GridRectangle(29, 14, 5, 5);
            b.Draw(RuneMagic.Textures["runic_anvil"], GridRectangle(26, 12, 12, 12), Color.White);
            Rune.Render(b, RuneMagic.Textures["blank_rune"]);
            if (Rune.Spell != null)
            {
                var x = 29;
                var y = 25;
                Color color;
                b.Draw(RuneMagic.Textures["magic_dust"], GridRectangle(x, y, 3, 3), Color.White);
                if (RuneItem.IngredientsMet())
                    color = Color.White;
                else
                    color = Color.Red;
                b.DrawString(Game1.smallFont, $"x {RuneItem.Ingredients[1].Item2}", Grid[x + 3, y + 1].ToVector2(), color);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            foreach (var knownSlot in SpellBook.KnownSpellSlots)
            {
                if (knownSlot.Bounds.Contains(x, y))
                {
                    //RuneMagic.Instance.Monitor.Log($"{knownSlot.Spell.Name}");

                    Rune.Spell = knownSlot.Spell;
                    RuneItem.Spell = knownSlot.Spell;
                    RuneItem.Ingredients = new List<(int, int)>
                    {
                       ( RuneMagic.JsonAssetsApi.GetObjectId("Blank Rune"), 1 ),
                       ( RuneMagic.JsonAssetsApi.GetObjectId("Magic Dust"), RuneItem.Spell.Level * 5 )
                    };
                }
            }
            if (Rune.Bounds.Contains(x, y))
            {
                if (RuneItem.IngredientsMet() && Rune.Spell != null)
                    if (Game1.player.addItemToInventoryBool(new Rune(RuneMagic.JsonAssetsApi.GetObjectId($"Rune of {Rune.Spell.Name}"), 1)))
                    {
                        Game1.player.removeItemsFromInventory(RuneItem.Ingredients[0].Item1, RuneItem.Ingredients[0].Item2);
                        Game1.player.removeItemsFromInventory(RuneItem.Ingredients[1].Item1, RuneItem.Ingredients[1].Item2);
                        Game1.playSound("hammer");
                        Rune.Spell = null;
                    }
            }
        }
    }
}