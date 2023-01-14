using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Monsters;
using Object = StardewValley.Object;
namespace RuneMagic
{


    internal sealed class ModEntry : Mod
    {

        private List<Rune> Runes = new List<Rune>();


        private IJsonAssetsApi JsonAssets;
        private ISpaceCoreApi SpaceCore;
        private IConditionsChecker ConditionsChecker;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.GameLoop.Saving += OnSaving;
            helper.Events.GameLoop.Saved += OnSaved;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JsonAssets == null)
            {
                Monitor.Log("Can't find Json Assets API", LogLevel.Error);
                return;
            }
            JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets"));

            SpaceCore = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            if (SpaceCore == null)
            {
                Monitor.Log("Can't find SpaceCore API", LogLevel.Error);
                return;
            }
            SpaceCore.RegisterSerializerType(typeof(Rune));

            ConditionsChecker = Helper.ModRegistry.GetApi<IConditionsChecker>("Cherry.ExpandedPreconditionsUtility");
            if (ConditionsChecker == null)
            {
                Monitor.Log("Can't find ConditionsChecker API", LogLevel.Error);
                return;
            }
            ConditionsChecker.Initialize(false, "RuneMagic");

        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {

        }
        private void OnSaving(object sender, SavingEventArgs e)
        {

        }
        private void OnSaved(object sender, SavedEventArgs e)
        {

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
                    Rune rune = (Rune)Game1.player.CurrentItem;
                    rune.Charges--;
                    //write to console the name and charges
                    Monitor.Log(rune.Name + " " + rune.Charges);

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
                    //check if 
                    //cast the item to a rune
                    items[i] = new Rune(items[i].ParentSheetIndex, items[i].Stack);
                    //add the rune to the list of runes in inventory
                    Runes.Add((Rune)items[i]);
                }
            }

        }

    }
}
