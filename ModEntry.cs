using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Object = StardewValley.Object;

namespace RuneMagic
{


    internal sealed class ModEntry : Mod
    {

        private IJsonAssetsApi JsonAssets;
        private ISpaceCoreApi SpaceCore;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
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
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            InitializeRunes();
        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {

                //if current item is a rune print name and charges as warn

                Rune rune = (Rune)Game1.player.CurrentItem;
                Monitor.Log(rune.Name + " " + rune.Charges, LogLevel.Warn);




            }
        }

        private void InitializeRunes()
        {
            //player not null 
            if (Game1.player == null)
            {
                return;
            }

            //get objects in player inventory that have "Rune of " in the name
            var runes = Game1.player.Items.Where(i => i != null && i.Name.Contains("Rune of ")).ToList();

            for (int i = 0; i < runes.Count; i++)
            {
                if (runes[i] is not Rune)
                {
                    //convert the item to an object of type Rune
                    runes[i] = new Rune(runes[i]);

                }
            }
        }
    }
}
