using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Netcode;
using RuneMagic.Source.Items;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Threading;
using xTile.Dimensions;
using static SpaceCore.Skills;

namespace RuneMagic.Source
{
    // A class for tracking and manipulating the player's magical abilities and actions in the game
    public class PlayerStats
    {
        public MagicSkill MagicSkill { get; set; } = null;

        public List<SpellEffect> ActiveEffects { get; set; }

        public List<Spell> KnownSpells { get; set; }
        public List<Spell> MemorizedSpells { get; set; }

        public float CastingTime { get; set; } = 0;
        private int _healthBeforeCasting;

        public PlayerStats()
        {
            KnownSpells = new List<Spell>();
            MemorizedSpells = new List<Spell>();

            MagicSkill = RuneMagic.MagicSkills[School.Abjuration];

            ActiveEffects = new List<SpellEffect>();
        }

        public void Update()
        {
            if (MagicSkill.Level > 14)
            {
                MagicSkill.Level = 14;
            }
            ActivateSpellCastingItem(Game1.player.CurrentItem as ISpellCastingItem);
            LearnSpells();

            //Effects
            for (int i = 0; i < ActiveEffects.Count; i++)
                ActiveEffects[i].Update();
        }

        public void ActivateSpellCastingItem(ISpellCastingItem item)
        {
            if (Game1.player.CurrentItem is not ISpellCastingItem) return;

            if (!RuneMagic.Instance.Helper.Input.IsDown(SButton.R))
            {
                CastingTime = 0;
                return;
            }

            if (CastingTime == 0)
            {
                _healthBeforeCasting = Game1.player.health;
            }

            if (Game1.player.health < _healthBeforeCasting)
            {
                CastingTime = 0;
                RuneMagic.Instance.Helper.Input.Suppress(SButton.R); return;
            }

            RuneMagic.Instance.Helper.Input.Suppress(SButton.W);
            RuneMagic.Instance.Helper.Input.Suppress(SButton.A);
            RuneMagic.Instance.Helper.Input.Suppress(SButton.S);
            RuneMagic.Instance.Helper.Input.Suppress(SButton.D);

            if (item.Spell != null && CastingTime >= Math.Floor(item.Spell.CastingTime * 60))
            {
                item.Activate(); RuneMagic.Instance.Helper.Input.Suppress(SButton.R);
                CastingTime = 0;
            }
            else { CastingTime += 1; }
        }

        public void LearnSpells()
        {
            //add null to memorized spells to allow for empty slots
            if (MemorizedSpells.Count < MagicSkill.Level + 1)
            {
                MemorizedSpells.Add(null);
            }

            // Add crafting recipes for spells that the player can learn at their level
            foreach (var spell in RuneMagic.Spells)
            {
                if (MagicSkill != null)
                {
                    if (KnownSpells.Contains(spell)) continue;
                    if (MagicSkill.Level + 1 >= 1 && spell.Level == 1)
                        KnownSpells.Add(spell);
                    if (MagicSkill.Level + 1 >= 4 && spell.Level == 2)
                        KnownSpells.Add(spell);
                    if (MagicSkill.Level + 1 >= 8 && spell.Level == 3)
                        KnownSpells.Add(spell);
                    if (MagicSkill.Level + 1 >= 12 && spell.Level == 4)
                        KnownSpells.Add(spell);
                    if (MagicSkill.Level + 1 >= 14 && spell.Level == 5)
                        KnownSpells.Add(spell);
                }
            }
        }

        public void LearnRecipes()
        {
            //foreach (var skill in RuneMagic.PlayerStats.Skills.Values)
            //{
            //    var level = skill.Level;
            //    if (level < 1) return;

            // var craftingRecipes = new string[] { "Runic Anvil", "Inscription Table", "Magic
            // Grinder", "Blank Rune", "Blank Parchment" };

            // // Add crafting recipes for the above items foreach (var recipe in craftingRecipes) {
            // if (!Game1.player.craftingRecipes.ContainsKey(recipe))
            // Game1.player.craftingRecipes.Add(recipe, 0); }

            // // Add crafting recipes for spells that the player can learn at their level foreach
            // (var spell in RuneMagic.Spells) { var spellLevel = spell.Level; if (level >=
            // spellLevel && ((spellLevel < 5 && level >= spellLevel * 2 - 1) || (spellLevel == 5 &&
            // level >= 10))) { var runeName = $"Rune of {spell.Name}"; if
            // (!Game1.player.craftingRecipes.ContainsKey(runeName))
            // Game1.player.craftingRecipes.Add(runeName, 0);

            //            var scrollName = $"{spell.Name} Scroll";
            //            if (!Game1.player.craftingRecipes.ContainsKey(scrollName))
            //                Game1.player.craftingRecipes.Add(scrollName, 0);
            //        }
            //    }
            //}
        }
    }
}