
using Microsoft.Xna.Framework.Input;
using SpaceCore;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using System;
using System.Threading;

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
            CastingFailureChance = 12;
            SpellAttack = 0;
        }
        public void Cast(IMagicItem item)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (RuneMagic.Farmer.CurrentItem is IMagicItem)
            {
                if (keyboardState.IsKeyDown(Keys.R))
                {

                    if (!RuneMagic.Farmer.HasCustomProfession(MagicSkill.Sage) && ItemHeld is Scroll)
                        RuneMagic.Farmer.CanMove = false;
                    else if (ItemHeld is not Scroll)
                        RuneMagic.Farmer.CanMove = false;


                    IsCasting = true;
                    if (CastingTimer >= Math.Floor(item.Spell.CastingTime * 60))
                    {
                        item.Activate();
                        CastingTimer = 0;
                        IsCasting = false;
                        RuneMagic.Farmer.CanMove = true;
                    }
                    else
                        CastingTimer += 1;
                }
                else
                {
                    CastingTimer = 0;
                    IsCasting = false;
                    RuneMagic.Farmer.CanMove = true;
                }
            }



        }
        public void LearnRecipes()
        {

            var level = RuneMagic.Farmer.GetCustomSkillLevel(MagicSkill);

            foreach (var spell in RuneMagic.Spells)
            {
                if (level >= spell.Level)
                {
                    if (level >= 1)
                    {
                        if (!RuneMagic.Farmer.craftingRecipes.ContainsKey("Runic Anvil"))
                            RuneMagic.Farmer.craftingRecipes.Add("Runic Anvil", 0);
                        if (!RuneMagic.Farmer.craftingRecipes.ContainsKey("Inscription Table"))
                            RuneMagic.Farmer.craftingRecipes.Add("Inscription Table", 0);
                        if (!RuneMagic.Farmer.craftingRecipes.ContainsKey("Magic Grinder"))
                            RuneMagic.Farmer.craftingRecipes.Add("Magic Grinder", 0);
                        if (!RuneMagic.Farmer.craftingRecipes.ContainsKey("Blank Rune"))
                            RuneMagic.Farmer.craftingRecipes.Add("Blank Rune", 0);
                        if (!RuneMagic.Farmer.craftingRecipes.ContainsKey("Blank Parchment"))
                            RuneMagic.Farmer.craftingRecipes.Add("Blank Parchment", 0);

                    }
                    if (!RuneMagic.Farmer.craftingRecipes.ContainsKey($"Rune of {spell.Name}"))
                        RuneMagic.Farmer.craftingRecipes.Add($"Rune of {spell.Name}", 0);
                    if (!RuneMagic.Farmer.craftingRecipes.ContainsKey($"{spell.Name} Scroll"))
                        RuneMagic.Farmer.craftingRecipes.Add($"{spell.Name} Scroll", 0);
                }

            }
        }
    }
}

