using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;

namespace RuneMagic
{
    public interface IDynamicGameAssetsApi
    {
        public string GetDGAItemId(object item_);
        public object SpawnDGAItem(string fullId, Color? color);
        public string[] ListContentPacks();
        public string[] GetItemsByPack(string packname);
        public string[] GetAllItems();
        public void AddEmbeddedPack(IManifest manifest, string dir);
    }
    public interface IJsonAssetsApi
    {
        int GetObjectId(string name);
        void LoadAssets(string path);
    }

    internal sealed class ModEntry : Mod
    {


        private IDynamicGameAssetsApi apiDGA;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
        }
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            apiDGA = Helper.ModRegistry.GetApi<IDynamicGameAssetsApi>("spacechase0.DynamicGameAssets");
            if (apiDGA != null)
            {
                IManifest manifest = new MyManifest("fierro.rune_magic_dga", "rune_magic_dga", "fierro", "Content for RuneMagicMod", new SemanticVersion("0.1.0"))
                {
                    ContentPackFor = new MyManifestContentPackFor
                    {
                        UniqueID = "spacechase0.DynamicGameAssets"
                    },
                    ExtraFields = new Dictionary<string, object>() { { "DGA.FormatVersion", 2 }, { "DGA.ConditionsFormatVersion", "1.28.4" } }
                };
                //get the path to dga folder

                apiDGA.AddEmbeddedPack(manifest, Path.Combine(Helper.DirectoryPath, "content-pack"));
            }
        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            //add embeded content pack using the manifest.json in the content pack



        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            InitializeRunes();
        }
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {
                //if the item is a rune and is current item write to console the name and charges
                if (Game1.player.CurrentItem is Rune)
                {

                    //write to console the name and charges
                    Monitor.Log($"Name: {((Rune)Game1.player.CurrentItem).Name} Charges: {((Rune)Game1.player.CurrentItem).Charges}", LogLevel.Info);
                }

            }
        }
        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {

        }

        private void InitializeRunes()
        {
            //get all items from player inventory
            var items = Game1.player.Items;
            //loop through all items with a for loop
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i].Name.Contains("Rune of "))
                {
                    //cast the item to a rune
                    items[i] = new Rune(items[i].ParentSheetIndex, items[i].Stack);
                }
            }
        }
    }
}
