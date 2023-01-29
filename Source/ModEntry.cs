using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using Rune = RuneMagic.Items.Rune;
using System.Linq;
using Object = StardewValley.Object;
using Microsoft.Xna.Framework;
using static SpaceCore.Skills;
using JsonAssets.Data;
using System.Collections.Generic;
using RuneMagic.Magic;
using RuneMagic.Framework;
using SpaceCore;
using RuneMagic.Famework;
using RuneMagic.Skills;
using StardewValley.Tools;
using StardewValley.Locations;

namespace RuneMagic.Source
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;
        public static RuneMagic RuneMagic;

        public JsonAssets.IApi JsonAssetsApi;
        public IApi SpaceCoreApi;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            RuneMagic = new RuneMagic();
            RuneMagic.RegisterSpells();
            RuneMagic.CCSRegister();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.Warped += OnWarped;
            RegisterSkill(RuneMagic.PlayerStats.MagicSkill = new MagicSkill());
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;
            SpaceCoreApi = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            SpaceCoreApi.RegisterSerializerType(typeof(Rune));
            SpaceCoreApi.RegisterSerializerType(typeof(Scroll));
            SpaceCoreApi.RegisterSerializerType(typeof(MagicWeapon));
            SpaceCoreApi.RegisterSerializerType(typeof(Spell));
        }
        private void OnItemsRegistered(object sender, EventArgs e)
        {
            RuneMagic.JARegisterCraftingStation("Runic Anvil", "An anvil marked with strange runes.", "assets/Items/big-craftable.png",
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            RuneMagic.JARegisterCraftingStation("Inscription Table", "A table marked with strange runes.", "assets/Items/big-craftable.png",
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            RuneMagic.JARegisterCraftingStation("Magic Grinder", "It's used to produce magic dust for glyphs.", "assets/Items/big-craftable.png",
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            RuneMagic.JARegisterRunes();
            RuneMagic.JARegisterScrolls();
            RuneMagic.JARegisterObject("Blank Rune", "A stone carved and prepared to carve runes in it.", "assets/Items/blank_rune.png",
                 new ObjectRecipe()
                 {
                     ResultCount = 1,
                     Ingredients = new List<ObjectIngredient>() {
                     new ObjectIngredient() { Object = "Stone", Count = 1 }, },
                     IsDefault = true
                 });
            RuneMagic.JARegisterObject("Blank Parchment", "A peace of parchment ready for inscribing", "assets/Items/blank_parchment.png",
                new ObjectRecipe()
                {
                    ResultCount = 1,
                    Ingredients = new List<ObjectIngredient>() {
                     new ObjectIngredient() { Object = "Fiber", Count = 1 }, },
                    IsDefault = true
                });
            RuneMagic.JARegisterObject("Magic Dust", "Magically processed dust obtained from Gems", "assets/Items/magic_dust.png", null);
            RuneMagic.JARegisterWeapon("Runic Staff", "Runic Staff description.", "assets/Items/runic_staff.png");


        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            RuneMagic.Farmer = Game1.player;
            RuneMagic.Farmer.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Magic Dust"), 100));
        }
        private void OnSaving(object sender, SavingEventArgs e)
        {
            foreach (var item in RuneMagic.Farmer.Items)
            {
                if (item is IMagicItem)
                    (item as IMagicItem).Spell = null;
            }

        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            RuneMagic.ManageMagicItems(Game1.player, JsonAssetsApi);
            if (Context.IsWorldReady)
            {
                RuneMagic.PlayerStats.CheckCasting(sender, e);
            }
        }
        private void OnEventFinished(object sender, EventArgs e)
        {
            if (Game1.CurrentEvent.id == 15065001)
            {
                Game1.player.AddCustomSkillExperience(RuneMagic.PlayerStats.MagicSkill, 100);
            }
        }
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            foreach (Item item in RuneMagic.Farmer.Items)
            {
                if (item is IMagicItem magicItem && magicItem.Spell == null)
                {
                    magicItem.InitializeSpell();
                }
            }
        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.R)
            {

                if (RuneMagic.Farmer.CurrentItem is IMagicItem magicItem)
                {
                    magicItem.Use();
                }
            }
        }
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            RuneMagic.WizardEvent(e.NewLocation);
        }
    }
}


