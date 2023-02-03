
using SpaceCore;
using StardewModdingAPI.Events;
using System;

namespace RuneMagic.Source
{
    public class PlayerStats
    {

        public MagicSkill MagicSkill { get; set; }
        public bool MagicLearned { get; set; } = false;
        public IMagicItem ItemHeld { get; set; } = null;
        public Spell SelectedSpell { get; set; } = null;
        public bool IsCasting { get; set; } = false;
        public float CastingTimer { get; set; } = 0;
        public int SpellAttack { get; set; }
        public int CastingFailureChance { get; set; }
        public bool runeMasterActive { get; set; } = false;

        public PlayerStats()
        {
            CastingFailureChance = 12;
            SpellAttack = 0;
        }

        public void CheckCasting(object sender, UpdateTickedEventArgs e)
        {
            if (ItemHeld is not null)
            {

                if (!RuneMagic.Farmer.HasCustomProfession(MagicSkill.Sage) && ItemHeld is Scroll)
                    RuneMagic.Farmer.CanMove = false;
                else if (ItemHeld is not Scroll)
                    RuneMagic.Farmer.CanMove = false;
                var castingTime = ItemHeld.Spell.CastingTime;
                if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Runemaster) && ItemHeld is Rune && (ItemHeld as Rune).Charges >= 3 && runeMasterActive)
                {
                    castingTime = 0;
                    (ItemHeld as Rune).Charges -= 2;
                    runeMasterActive = false;
                }
                if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Scribe) && ItemHeld is Scroll)
                    castingTime *= 0.5f;
                IsCasting = true;
                if (CastingTimer >= Math.Floor(castingTime * 60))
                {
                    ItemHeld.Activate();
                    ItemHeld = null;
                    IsCasting = false;
                    CastingTimer = 0;
                    RuneMagic.Farmer.CanMove = true;
                }
                else
                    CastingTimer += 1;

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

