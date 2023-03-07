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

        public RuneCraftingMenu(SpellBook spellBook)
            : base(spellBook)
        {
            Rune = new MagicButton();
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            DrawKnownSlots(b);
            DrawRune(b);
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
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            foreach (var knownSlot in SpellBook.KnownSpellSlots)
            {
                if (knownSlot.Bounds.Contains(x, y))
                {
                    RuneMagic.Instance.Monitor.Log($"{knownSlot.Spell.Name}");

                    Rune.Spell = knownSlot.Spell;
                }
            }
            if (Rune.Bounds.Contains(x, y))
            {
                Game1.playSound("hammer");
                Game1.player.addItemToInventory(new Rune(RuneMagic.JsonAssetsApi.GetObjectId($"Rune of {Rune.Spell.Name}"), 1));
            }
        }
    }
}