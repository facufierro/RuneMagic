using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.Framework;
using SpaceCore;
using RuneMagic.Skills;
using System.Threading;
using StardewModdingAPI;
using RuneMagic.Famework;
using RuneMagic.Source;
using StardewModdingAPI.Events;
using System.Runtime.InteropServices;
using RuneMagic.Items;
using System.Collections.Generic;

namespace RuneMagic.Source
{
    public class PlayerStats
    {

        public MagicSkill MagicSkill { get; set; }
        public bool MagicLearned { get; set; } = false;
        public IMagicItem ItemHeld { get; set; } = null;
        public Spell SelectedSpell { get; set; }
        public int SpellAttack { get; set; }
        public int CastingFailureChance { get; set; }
        public bool IsCasting { get; set; } = false;
        public float CastingTimer { get; set; } = 0;

        public PlayerStats()
        {
            CastingFailureChance = 10;
            SpellAttack = 0;
        }

        public void CheckCasting(object sender, UpdateTickedEventArgs e)
        {


            if (ItemHeld is not null)
            {
                var castingTime = ItemHeld.Spell.CastingTime;
                if (ModEntry.RuneMagic.Farmer.HasCustomProfession(MagicSkill.Scribe) && ItemHeld is Scroll)
                    castingTime *= 0.5f;
                IsCasting = true;
                if (CastingTimer >= Math.Floor(castingTime * 60))
                {
                    ItemHeld.Activate();
                    ItemHeld = null;
                    IsCasting = false;
                    CastingTimer = 0;
                }
                else
                    CastingTimer += 1;
            }

        }
        public void LearnRecipes()
        {

            var level = ModEntry.RuneMagic.Farmer.GetCustomSkillLevel(MagicSkill);

            foreach (var spell in ModEntry.RuneMagic.SpellList)
            {
                if (level >= spell.Level)
                {
                    if (level >= 1)
                    {
                        if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey("Runic Anvil"))
                            ModEntry.RuneMagic.Farmer.craftingRecipes.Add("Runic Anvil", 0);
                        if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey("Inscription Table"))
                            ModEntry.RuneMagic.Farmer.craftingRecipes.Add("Inscription Table", 0);
                        if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey("Magic Grinder"))
                            ModEntry.RuneMagic.Farmer.craftingRecipes.Add("Magic Grinder", 0);
                        if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey("Blank Rune"))
                            ModEntry.RuneMagic.Farmer.craftingRecipes.Add("Blank Rune", 0);
                        if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey("Blank Parchment"))
                            ModEntry.RuneMagic.Farmer.craftingRecipes.Add("Blank Parchment", 0);

                    }
                    if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey($"Rune of {spell.Name}"))
                        ModEntry.RuneMagic.Farmer.craftingRecipes.Add($"Rune of {spell.Name}", 0);
                    if (!ModEntry.RuneMagic.Farmer.craftingRecipes.ContainsKey($"{spell.Name} Scroll"))
                        ModEntry.RuneMagic.Farmer.craftingRecipes.Add($"{spell.Name} Scroll", 0);
                }

            }
        }
    }
}

