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

            helper.Events.GameLoop.Saving += OnSaving;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JsonAssets == null)
            {
                Monitor.Log("Can't find Json Assets API", LogLevel.Error);
                return;
            }
            JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets/ContentPack"));

            SpaceCore = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            if (SpaceCore == null)
            {
                Monitor.Log("Can't find SpaceCore API", LogLevel.Error);
                return;
            }
            SpaceCore.RegisterSerializerType(typeof(Rune));
            SpaceCore.RegisterSerializerType(typeof(Spell));





        }
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo("assets/Textures/projectile"))
            {

                //get only the names from the textures in the assets folder
                string[] textureNames = Directory.GetFiles(Path.Combine(Helper.DirectoryPath, "assets/Spells/Effects"), "*.png", SearchOption.AllDirectories)
                          .Select(Path.GetFileNameWithoutExtension)
                          .ToArray();
                e.LoadFromModFile<Texture2D>($"assets/Textures/{textureNames}.png", AssetLoadPriority.Medium);
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

            foreach (Item item in Game1.player.Items)
            {
                if (item is Rune rune && rune.Spell == null)
                {
                    rune.InitializeSpell();
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

                //log player facing direction to a flot in console as warn
                Monitor.Log(Game1.player.FacingDirection.ToString(), LogLevel.Warn);
            }
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
    }
}