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

namespace RuneMagic
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;

        private IJsonAssetsApi JsonAssets;
        private ISpaceCoreApi SpaceCore;



        public override void Entry(IModHelper helper)
        {
            Instance = this;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.GameLoop.Saving += OnSaving;
            //on charcter friendship change



        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets/ContentPack"));
            SpaceCore = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            SpaceCore.RegisterSerializerType(typeof(Rune));
            SpaceCore.RegisterSerializerType(typeof(Spell));

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
                e.Edit(RegisterLetter);
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Events"))
            {
                e.Edit(RegisterEvents);
            }


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
            }

        }
        private void OnWarped(object sender, WarpedEventArgs e)
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
                        Rune rune = (Rune)Game1.player.Items[i];
                        if (rune.Charges < 1)
                        {
                            //remove one stack from the rune
                            Game1.player.Items[i].Stack--;


                        }
                    }
                }
            }
        }
        private void RegisterLetter(IAssetData asset)
        {
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter1"] =
                "Hello @... " +
                "^^ I have been watching you for a while now, and I have noticed that you have an interest in magic. " +
                "^ This is for you, friend." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssets.GetObjectId("Rune of Haste")} 1 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter2"] =
                "My friend... " +
                "^^ I can sense you have been using the gift I gave you, I hope you enjoy this one." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssets.GetObjectId("Rune of Magic Missile")} 1 %%";
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
        private void RegisterEvents(IAssetData asset)
        {
            var data = asset.AsDictionary<string, string>().Data;
            data[""] = "";
        }
    }
}