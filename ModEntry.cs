using Microsoft.Xna.Framework.Graphics;
using RuneMagic.assets.Api;
using RuneMagic.assets.Spells;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.IO;
using System.Reflection;
using Rune = RuneMagic.assets.Items.Rune;
using System.Linq;
using Object = StardewValley.Object;
using StardewValley.Monsters;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using SpaceCore;
using RuneMagic.assets.Skills;
using static SpaceCore.Skills;
using CustomCraftingStation;

namespace RuneMagic
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;

        private IJsonAssetsApi JsonAssets;

        private static MagicSkill Skill;


        public override void Entry(IModHelper helper)
        {
            Instance = this;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            SpaceCore.Events.SpaceEvents.OnBlankSave += OnBlankSave;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            //on objectdraw event


            RegisterSkill(Skill = new MagicSkill());
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets/ContentPack"));
            IApi spaceCore = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            spaceCore.RegisterSerializerType(typeof(Rune));
            spaceCore.RegisterSerializerType(typeof(Spell));

        }
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            string[] textureNames = Directory.GetFiles(Path.Combine(Helper.DirectoryPath, "assets/Spells/Effects"), "*.png", SearchOption.AllDirectories)
                      .Select(Path.GetFileNameWithoutExtension)
                      .ToArray();
            if (e.Name.IsEquivalentTo($"assets/Textures/{textureNames}"))
            {
                e.LoadFromModFile<Texture2D>($"assets/Textures/{textureNames}.png", AssetLoadPriority.Medium);
            }
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Mail"))
                e.Edit(RegisterMail);
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Event"))
                e.Edit(RegisterEvent);



        }
        private void OnBlankSave(object sender, EventArgs e)
        {
            Game1.player.addItemToInventory(new Object(Vector2.Zero, JsonAssets.GetObjectId("InscriptionTable")));
        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {


        }
        private void OnSaving(object sender, SavingEventArgs e)
        {
            //remove spells from runes
            foreach (var item in Game1.player.Items)
            {
                if (item is Rune rune)
                {
                    rune.Spell = null;
                }
            }
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            RegisterRunes();

        }
        private void OnEventFinished(object sender, EventArgs e)
        {
            if (Game1.CurrentEvent.id == 15065001)
            {
                Object inscriptionTable = new Object(Vector2.Zero, JsonAssets.GetBigCraftableId("Inscription Table"));
                Game1.player.Position = new Vector2(7 * Game1.tileSize, 22 * Game1.tileSize);
                Game1.player.addItemToInventory(inscriptionTable);
                Game1.player.holdUpItemThenMessage(inscriptionTable);
            }


        }
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            //Initialize spells for runes on day start
            foreach (Item item in Game1.player.Items)
            {
                if (item is Rune rune && rune.Spell == null)
                {
                    rune.InitializeSpell();
                }
            }
            Game1.player.AddCustomSkillExperience(Skill, 2150);

            //Check for wizard letters 
            //if player has 3 or more friendship with wizard
            if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 3)
            {
                if (!Game1.player.mailReceived.Contains("RuneMagicWizardLetter1"))
                {
                    Game1.player.mailbox.Add("RuneMagicWizardLetter1");
                }

            }
            if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 4)
            {
                if (!Game1.player.mailReceived.Contains("RuneMagicWizardLetter2"))
                {
                    Game1.player.mailbox.Add("RuneMagicWizardLetter2");
                }

            }
            if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 5)
            {
                if (!Game1.player.mailReceived.Contains("RuneMagicWizardLetter3"))
                {
                    Game1.player.mailbox.Add("RuneMagicWizardLetter3");
                }

            }
            if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 6)
            {
                if (!Game1.player.mailReceived.Contains("RuneMagicWizardLetter4"))
                {
                    Game1.player.mailbox.Add("RuneMagicWizardLetter4");
                }

            }

        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {
                if (Game1.player.CurrentItem is Rune)
                {
                    Rune rune = (Rune)Game1.player.CurrentItem;
                    rune.Activate();
                }
                //print cursor tile position and the map name
                Monitor.Log($"Location: {Game1.currentCursorTile.X}, {Game1.currentCursorTile.Y} {Game1.currentLocation.Name}");
                Monitor.Log($"Wizard Friendship: {Game1.player.getFriendshipHeartLevelForNPC("Wizard")}");
            }

        }
        private void OnWarped(object sender, WarpedEventArgs e)
        {

            if (e.NewLocation.Name == "WizardHouse")
            {
                if (Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 6)
                {
                    var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                        $"/addBigProp 10 17 {JsonAssets.GetBigCraftableId("Inscription Table")}" +
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
        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {

        }

        private void RegisterRunes()
        {
            if (Game1.player == null)
            {
                return;
            }
            for (int i = 0; i < Game1.player.Items.Count; i++)
            {
                if (Game1.player.Items[i] != null)
                {
                    if (Game1.player.Items[i] is not Rune)
                    {
                        if (Game1.player.Items[i].Name.Contains("Rune of "))
                            Game1.player.Items[i] = new Rune(Game1.player.Items[i].ParentSheetIndex, Game1.player.Items[i].Stack);
                    }
                    else
                    {


                    }
                }
            }
        }
        private void RegisterMail(IAssetData asset)
        {
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter1"] =
                "Hello @... " +
                "^^ I have been watching you for a while now, and I have noticed that you have an interest in magic. " +
                "^ This is for you, friend." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssets.GetObjectId("Rune of Magic Missile")} 5 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter2"] =
                "My friend... " +
                "^^ I can sense you have been using the gift I gave you, I hope you enjoy this one." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssets.GetObjectId("Rune of Magic Displacement")} 5 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter3"] =
                "My dear friend @... " +
                "^^ I think you will appreciate this. " +
                "^It is very rare so use it with care." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssets.GetObjectId("Rune of Teleportation")} 1 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter4"] =
               "My dear friend @... " +
               "^^ I have something I wish you to have but its too powerful and precious to send it trough mail. " +
               "^Please come pay me a visit when you can." +
               "^^-M. Rasmodius, Wizard";


        }
        private void RegisterEvent(IAssetData asset)
        {
            var data = asset.AsDictionary<string, string>().Data;
            data["15065001/n RuneMagicWizardLetter4"] = "";
        }
    }
}