using Microsoft.Xna.Framework.Input;
using RuneMagic.Source.Items;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace RuneMagic.Source
{
    // A class for tracking and manipulating the player's magical abilities and actions in the game
    public class PlayerStats
    {
        public MagicSkill MagicSkill { get; set; } // The player's magic skill added using SpaceCore
        public bool MagicLearned { get; set; } = false; // Indicates whether the player has learned magic
        public bool IsCasting { get; set; } = false; // Indicates whether the player is currently casting a spell
        public float CastingTimer { get; set; } = 0; // Tracks how long the player has been casting the current spell
        public int SpellAttack { get; set; } // Represents the strength of the player's spell attack NOT IMPLEMENTED
        public int CastingFailureChance { get; set; } // Represents the chance that the player's casting attempt will fail

        private int PreviousHealth;

        public PlayerStats()
        {
        }

        // Attempts to cast a spell using the specified IMagicItem object
        public void Cast(IMagicItem item)
        {
            // Check if the player is holding a valid magic item and whether the "R" key is being pressed
            if (Game1.player.CurrentItem is not IMagicItem) return;
            if (!RuneMagic.Instance.Helper.Input.IsDown(SButton.R))
            {
                IsCasting = false;
                CastingTimer = 0;
                return;
            }

            if (!IsCasting)
            {
                // Store the player's current health before starting to cast the spell
                PreviousHealth = Game1.player.health;
                IsCasting = true;
            }

            // Check if the player has taken damage during the casting process and interrupt casting
            if (Game1.player.health < PreviousHealth)
            {
                IsCasting = false;
                CastingTimer = 0;
                RuneMagic.Instance.Helper.Input.Suppress(SButton.R);
                return;
            }

            // Enter a casting state and suppress certain movement keys
            if (item.Charges >= 1)
            {
                RuneMagic.Instance.Helper.Input.Suppress(SButton.W);
                RuneMagic.Instance.Helper.Input.Suppress(SButton.A);
                RuneMagic.Instance.Helper.Input.Suppress(SButton.S);
                RuneMagic.Instance.Helper.Input.Suppress(SButton.D);
            }

            // If the casting timer exceeds the casting time of the spell, activate the spell and
            // reset the casting state
            if (CastingTimer >= Math.Floor(item.Spell.CastingTime * 60))
            {
                item.Activate();
                CastingTimer = 0;
            }
            else
            {
                CastingTimer += 1;
            }
        }

        // Adds crafting recipes for various magical items to the player's recipe list, based on the
        // player's current magic level and the list of spells
        public void LearnRecipes()
        {
            var level = Game1.player.GetCustomSkillLevel(MagicSkill);
            if (level < 1) return;

            var craftingRecipes = new string[]
            {
                "Runic Anvil",
                "Inscription Table",
                "Magic Grinder",
                "Blank Rune",
                "Blank Parchment"
            };

            // Add crafting recipes for the above items
            foreach (var recipe in craftingRecipes)
            {
                if (!Game1.player.craftingRecipes.ContainsKey(recipe))
                    Game1.player.craftingRecipes.Add(recipe, 0);
            }

            // Add crafting recipes for each spell that the player has learned
            foreach (var spell in RuneMagic.Spells)
            {
                if (level >= spell.Level)
                {
                    var runeName = $"Rune of {spell.Name}";
                    if (!Game1.player.craftingRecipes.ContainsKey(runeName))
                        Game1.player.craftingRecipes.Add(runeName, 0);

                    var scrollName = $"{spell.Name} Scroll";
                    if (!Game1.player.craftingRecipes.ContainsKey(scrollName))
                        Game1.player.craftingRecipes.Add(scrollName, 0);
                }
            }
        }
    }
}