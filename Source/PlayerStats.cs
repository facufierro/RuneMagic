using Microsoft.Xna.Framework.Input;
using RuneMagic.Source.Items;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

namespace RuneMagic.Source
{
    public class PlayerStats
    {
        public MagicSkill MagicSkill { get; set; }
        public bool MagicLearned { get; set; } = false;
        public bool IsCasting { get; set; } = false;
        public float CastingTimer { get; set; } = 0;
        public int SpellAttack { get; set; }
        public int CastingFailureChance { get; set; }

        public PlayerStats()
        {
        }

        public void Cast(IMagicItem item)
        {
            //playsound

            KeyboardState keyboardState = Keyboard.GetState();
            if (Game1.player.CurrentItem is IMagicItem)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {
                    if (!Game1.player.HasCustomProfession(MagicSkill.Sage) && item is Scroll)
                    {
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.W);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.A);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.S);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.D);
                    }
                    else if (item is not Scroll && item.Charges >= 1)
                    {
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.W);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.A);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.S);
                        RuneMagic.Instance.Helper.Input.Suppress(SButton.D);
                    }

                    IsCasting = true;
                    if (CastingTimer >= Math.Floor(item.Spell.CastingTime * 60))
                    {
                        item.Activate();
                        Game1.playSound("flameSpell");
                        CastingTimer = 0;
                        IsCasting = false;
                    }
                    else
                        CastingTimer += 1;
                }
                else
                {
                    CastingTimer = 0;
                    IsCasting = false;
                }
            }
        }

        public void LearnRecipes()
        {
            var level = Game1.player.GetCustomSkillLevel(MagicSkill);
            if (level >= 1)
            {
                if (!Game1.player.craftingRecipes.ContainsKey("Runic Anvil"))
                    Game1.player.craftingRecipes.Add("Runic Anvil", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Inscription Table"))
                    Game1.player.craftingRecipes.Add("Inscription Table", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Magic Grinder"))
                    Game1.player.craftingRecipes.Add("Magic Grinder", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Blank Rune"))
                    Game1.player.craftingRecipes.Add("Blank Rune", 0);
                if (!Game1.player.craftingRecipes.ContainsKey("Blank Parchment"))
                    Game1.player.craftingRecipes.Add("Blank Parchment", 0);
            }

            foreach (var spell in RuneMagic.Spells)
            {
                if (level >= spell.Level)
                {
                    if (!Game1.player.craftingRecipes.ContainsKey($"Rune of {spell.Name}"))
                        Game1.player.craftingRecipes.Add($"Rune of {spell.Name}", 0);
                    if (!Game1.player.craftingRecipes.ContainsKey($"{spell.Name} Scroll"))
                        Game1.player.craftingRecipes.Add($"{spell.Name} Scroll", 0);
                }
            }
        }
    }
}