using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Tools;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Network;


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
            ManageRunes();
        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {
                if (Game1.player.CurrentItem is Rune)
                {
                    Rune rune = (Rune)Game1.player.CurrentItem;
                    Monitor.Log("Charges: " + rune.Charges, LogLevel.Warn);
                    rune.Charges--;
                }
            }
        }

        private void ManageRunes()
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
                            Game1.player.Items[i] = null;
                            //play break stone sound
                            Game1.playSound("stoneStep");
                        }
                    }
                }

            }
        }
    }
}
