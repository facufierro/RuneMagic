﻿using Microsoft.Xna.Framework.Graphics;
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

namespace RuneMagic.Source
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;
        public static RuneMagic RuneMagic;

        private JsonAssets.IApi JsonAssetsApi;
        private IApi SpaceCoreApi;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            RuneMagic = new RuneMagic();
            RuneMagic.RegisterSpells();
            RuneMagic.CCSRegister();


            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            SpaceCore.Events.SpaceEvents.OnBlankSave += OnBlankSave;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;

            RegisterSkill(RuneMagic.PlayerStats.MagicSkill = new MagicSkill());


        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;
            SpaceCoreApi = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            SpaceCoreApi.RegisterSerializerType(typeof(Rune));
            SpaceCoreApi.RegisterSerializerType(typeof(Spell));


        }
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            //Monitor.Log($"{e.NameWithoutLocale}", LogLevel.Alert);
            if (e.Name.IsEquivalentTo("Data/Mail"))
                e.Edit(RuneMagic.RegisterMail);
            if (e.Name.IsEquivalentTo("Data/Event"))
                e.Edit(RuneMagic.RegisterEvent);
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
        private void OnBlankSave(object sender, EventArgs e)
        {

        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            //Add playerStats to the farmer
            RuneMagic.Farmer = Game1.player;
            //add magic dust to the inventory of the player
            RuneMagic.Farmer.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Magic Dust"), 100));



        }
        private void OnSaving(object sender, SavingEventArgs e)
        {
            foreach (var item in RuneMagic.Farmer.Items)
            {
                if (item is IMagicItem)
                    (item as IMagicItem).Spell = null;
                //



            }

        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            RuneMagic.ManageMagicItems(Game1.player, JsonAssetsApi);
            if (Context.IsWorldReady)
            {
                RuneMagic.PlayerStats.CheckCasting(sender, e);
                //Monitor.Log(RuneMagic.Farmer.GetCustomSkillExperience(RuneMagic.PlayerStats.MagicSkill).ToString(), LogLevel.Alert);
            }

        }
        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {


        }
        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {

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


            var runicStaff = new RuneWeapon(JsonAssetsApi.GetWeaponId("Runic Staff"));
            RuneMagic.Farmer.addItemToInventory(runicStaff);







            foreach (Item item in RuneMagic.Farmer.Items)
            {
                if (item is Rune rune && rune.Spell == null)
                {
                    rune.InitializeSpell();
                }
            }
            //check wizard letter
            if (RuneMagic.Farmer.getFriendshipHeartLevelForNPC("Wizard") >= 6)
            {
                if (!RuneMagic.Farmer.mailReceived.Contains("RuneMagicWizardLetter"))
                {
                    RuneMagic.Farmer.mailbox.Add("RuneMagicWizardLetter");
                }

            }



        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.R)
            {

                if (Game1.player.CurrentItem is IMagicItem magicItem)
                {
                    magicItem.Use();
                    //print to console the item Type
                    Monitor.Log(magicItem.GetType().ToString(), LogLevel.Alert);

                }
            }

        }
        private void OnWarped(object sender, WarpedEventArgs e)
        {

            if (e.NewLocation.Name == "WizardHouse")
            {
                if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 6)
                {
                    var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                        $"/addBigProp 10 17 {JsonAssetsApi.GetBigCraftableId("Inscription Table")}" +
                        $"/speak Wizard \"@! Come in my friend, come in...\"" +
                        $"/pause 400" +
                        $"/advancedMove Wizard false -2 0 3 100 0 2 1 3000" +
                        $"/move farmer 0 -6 1 true" +
                        $"/pause 2000" +
                        $"/speak Wizard \"What do you think about this? Beautiful, isn't it?\"" +
                        $"/pause 500" +
                        $"/speak Wizard \"It is a Inscription Table.\"" +
                        $"/pause 500" +
                        $"/speak Wizard \"With it, and the right recipes you can make magic!\"" +
                        $"/pause 1000" +
                        $"/speak Wizard \"Its a gift. I hope you enjoy it as much as I do.\"" +
                        $"/end";

                    e.NewLocation.startEvent(new Event(eventString, 15065001));

                }




            }
        }

    }
}

