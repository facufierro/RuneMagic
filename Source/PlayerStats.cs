using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Netcode;
using RuneMagic.Source.Interface;
using RuneMagic.Source.Items;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using xTile.Dimensions;
using static SpaceCore.Skills;

namespace RuneMagic.Source
{
    // A class for tracking and manipulating the player's magical abilities and actions in the game
    public class PlayerStats
    {
        public MagicSkill MagicSkill { get; set; } = null;
        public bool RuneCarving { get; set; } = false;
        public bool ScrollScribing { get; set; } = false;
        public bool MagicLearned { get; set; } = false;
        public CastBar CastBar { get; set; }
        public List<SpellEffect> ActiveEffects { get; set; }

        public List<Spell> KnownSpells { get; set; }
        public List<Spell> MemorizedSpells { get; set; }

        public float CastingTime { get; set; } = 0;
        private int _healthBeforeCasting;
        private int _previousMagicSkillLevel = 0;

        public PlayerStats()
        {
            KnownSpells = new List<Spell>();
            MemorizedSpells = new List<Spell>();

            MagicSkill = RuneMagic.MagicSkills[School.Abjuration];

            ActiveEffects = new List<SpellEffect>();
            CastBar = new CastBar();
        }

        public void Update()
        {
            MagicSkill.SetLevel();

            ActivateSpellCastingItem(Game1.player.CurrentItem as ISpellCastingItem);
            LearnSpells();

            //Effects
            for (int i = 0; i < ActiveEffects.Count; i++)
                ActiveEffects[i].Update();
            if (_previousMagicSkillLevel != MagicSkill.Level)
                //if the player has a SpellBook in the inventory
                if (Game1.player.Items.Any(item => item is SpellBook))
                {
                    //get the first SpellBook in the inventory
                    var spellBook = Game1.player.Items.First(item => item is SpellBook) as SpellBook;
                    //update the spell slots
                    spellBook.UpdateSpellSlots();
                }
            _previousMagicSkillLevel = MagicSkill.Level;
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
            if (RuneMagic.PlayerStats.ScrollScribing)
            {
                if (!Game1.player.craftingRecipes.ContainsKey("Inscription Table"))
                    Game1.player.craftingRecipes.Add("Inscription Table", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Magic Grinder"))
                    Game1.player.craftingRecipes.Add("Magic Grinder", 0);
                foreach (var spell in RuneMagic.PlayerStats.KnownSpells)
                {
                    if (!Game1.player.craftingRecipes.ContainsKey($"{spell.Name} Scroll"))
                        Game1.player.craftingRecipes.Add($"{spell.Name} Scroll", 0);
                }
            }
            if (RuneMagic.PlayerStats.RuneCarving)
            {
                if (!Game1.player.craftingRecipes.ContainsKey("Runic Anvil"))
                    Game1.player.craftingRecipes.Add("Runic Anvil", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Magic Grinder"))
                    Game1.player.craftingRecipes.Add("Magic Grinder", 0);
                foreach (var spell in RuneMagic.PlayerStats.KnownSpells)
                {
                    if (!Game1.player.craftingRecipes.ContainsKey($"Rune of {spell.Name}"))
                        Game1.player.craftingRecipes.Add($"Rune of {spell.Name}", 0);
                }
            }
        }
    }
}