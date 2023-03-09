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
    public class SpellBookMenu : MagicMenu
    {
        public SpellBookMenu(SpellBook spellBook)
            : base(spellBook)
        {
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            DrawKnownSlots(b);

            DrawMemorizedSlots(b);
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

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            //if the current location is home
            if (Game1.currentLocation.Name == "FarmHouse" || Game1.currentLocation.Name == "WizardHouse")
            {
                foreach (var knownSlot in SpellBook.KnownSpellSlots)
                {
                    if (knownSlot.Bounds.Contains(x, y))
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
                    if (memorizedSlot.Bounds.Contains(x, y))
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