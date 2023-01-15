using System.IO;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using System.Xml.Serialization;
using System.Text;
using RuneMagic.assets.Items;
using RuneMagic.assets.Api;
using Rune = RuneMagic.assets.Items.Rune;

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
            JsonAssets.LoadAssets(Path.Combine(Helper.DirectoryPath, "assets/ContentPack"));

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
                    rune.Activate(Game1.player.CurrentItem);
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
