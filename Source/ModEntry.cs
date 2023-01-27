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

namespace RuneMagic.Source
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;
        public static MagicSkill MagicSkill;
        public static PlayerStats PlayerStats;

        private JsonAssets.IApi JsonAssetsApi;
        private IApi SpaceCoreApi;





        public override void Entry(IModHelper helper)
        {
            Instance = this;

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.InventoryChanged += OnInventoryChanged;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            RegisterSkill(MagicSkill = new MagicSkill());


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
                e.Edit(RegisterMail);
            if (e.Name.IsEquivalentTo("Data/Event"))
                e.Edit(RegisterEvent);
        }
        private void OnItemsRegistered(object sender, EventArgs e)
        {
            RegisterMagicCraftingStations();
            RegisterRunes();
            RegisterScrolls();
        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {


        }
        private void OnSaving(object sender, SavingEventArgs e)
        {

        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {

            ManageMagicItems(Game1.player);

            if (Context.IsWorldReady)
                PlayerStats.CheckCasting(sender, e);



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
                Object inscriptionTable = new Object(Vector2.Zero, JsonAssetsApi.GetBigCraftableId("Inscription Table"));
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
            //Check for wizard letters 

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

            Game1.player.AddCustomSkillExperience(MagicSkill, 3000);

            PlayerStats = new PlayerStats(Game1.player);

        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {
                if (Game1.player.CurrentItem is Rune rune)
                {
                    rune.Use();

                }
                else if (Game1.player.CurrentItem is Scroll scroll)
                {
                    scroll.Use();
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

        private void ManageMagicItems(Farmer player)
        {
            if (Context.IsWorldReady)
                for (int i = 0; i < player.Items.Count; i++)
                {
                    var inventory = player.Items;
                    List<string> itemsFromPack = new List<string>(JsonAssetsApi.GetAllObjectsFromContentPack("fierro.rune_magic"));

                    if (inventory[i] is not MagicItem and not null)
                    {
                        if (itemsFromPack.Contains(inventory[i].Name))
                        {
                            if (inventory[i].Name.Contains("Rune of "))
                                player.Items[i] = new Rune(inventory[i].ParentSheetIndex, inventory[i].Stack);
                            if (inventory[i].Name.Contains(" Scroll"))
                                player.Items[i] = new Scroll(inventory[i].ParentSheetIndex, inventory[i].Stack);
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
                $"%item object {JsonAssetsApi.GetObjectId("Rune of Magic Missile")} 5 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter2"] =
                "My friend... " +
                "^^ I can sense you have been using the gift I gave you, I hope you enjoy this one." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssetsApi.GetObjectId("Rune of Displacement")} 5 %%";
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter3"] =
                "My dear friend @... " +
                "^^ I think you will appreciate this. " +
                "^It is very rare so use it with care." +
                "^^-M. Rasmodius, Wizard" +
                $"%item object {JsonAssetsApi.GetObjectId("Rune of Teleportation")} 1 %%";
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
        private void RegisterRunes()
        {
            int index = 0;

            var spells = typeof(ModEntry).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToList();

            for (int i = 0; i < spells.Count(); i++)
            {
                Spell spell = (Spell)Activator.CreateInstance(Type.GetType($"RuneMagic.Spells.{spells[i]}"));
                int textureCount = System.IO.Directory.GetFiles(@$"{Helper.DirectoryPath}/assets/Runes").Length;
                if (index > textureCount)
                    index = 0;
                Texture2D texture = Helper.ModContent.Load<Texture2D>($"assets/Runes/rune-{index}.png");
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                for (int j = 0; j < data.Length; ++j)
                {
                    if (data[j] == Color.White)
                        data[j] = spell.GetColor()[0];
                    if (data[j] == Color.Black)
                        data[j] = spell.GetColor()[1];
                }
                texture.SetData(data);

                JsonAssets.Mod.instance.RegisterObject(ModManifest, new ObjectData()
                {
                    Name = $"Rune of {spell.Name}",
                    Description = $"{spell.Description}",
                    Texture = texture,
                    Category = ObjectCategory.Crafting,
                    CategoryTextOverride = $"{spell.School}",
                    CategoryColorOverride = spell.GetColor()[1],
                    Price = 0,
                    HideFromShippingCollection = true,
                    Recipe = new ObjectRecipe()
                    {
                        ResultCount = 1,
                        Ingredients =
                    {
                        new ObjectIngredient()
                        {
                            Object = "Stone",
                            Count = 1
                        }
                    },
                        IsDefault = true

                    }

                });
                index++;

            }
        }
        private void RegisterScrolls()
        {
            var spells = typeof(ModEntry).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToList();
            for (int i = 0; i < spells.Count(); i++)
            {
                Spell spell = (Spell)Activator.CreateInstance(Type.GetType($"RuneMagic.Spells.{spells[i]}"));
                int textureCount = System.IO.Directory.GetFiles(@$"{Helper.DirectoryPath}/assets/Scrolls").Length;
                Texture2D texture = Helper.ModContent.Load<Texture2D>($"assets/Scrolls/scroll-0.png");
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                for (int j = 0; j < data.Length; ++j)
                {
                    if (data[j] == Color.White)
                        data[j] = spell.GetColor()[0];
                    if (data[j] == Color.Black)
                        data[j] = spell.GetColor()[1];
                }
                texture.SetData(data);

                JsonAssets.Mod.instance.RegisterObject(ModManifest, new ObjectData()
                {
                    Name = $"{spell.Name} Scroll",
                    Description = $"{spell.Description}",
                    Texture = texture,
                    Category = ObjectCategory.Crafting,
                    CategoryTextOverride = $"{spell.School}",
                    CategoryColorOverride = spell.GetColor()[1],
                    Price = 0,
                    HideFromShippingCollection = true,
                    Recipe = new ObjectRecipe()
                    {
                        ResultCount = 1,
                        Ingredients =
                    {
                        new ObjectIngredient()
                        {
                            Object = "Stone",
                            Count = 1
                        }
                    },
                        IsDefault = true

                    }

                });

            }
        }
        private void RegisterMagicCraftingStations()
        {
            JsonAssets.Mod.instance.RegisterBigCraftable(ModManifest, new BigCraftableData()
            {
                Name = $"Runic Anvil",
                Description = $"Anvil used to carve Runes",
                Texture = Helper.ModContent.Load<Texture2D>($"assets/Items/big-craftable.png"),
                Price = 0,
                Recipe = new BigCraftableRecipe()
                {
                    ResultCount = 1,
                    Ingredients =
                    {
                        new BigCraftableIngredient()
                        {
                            Object = "Stone",
                            Count = 1
                        }
                    },
                    IsDefault = true

                }
            });
            JsonAssets.Mod.instance.RegisterBigCraftable(ModManifest, new BigCraftableData()
            {
                Name = $"Inscription Table",
                Description = $"Table used to inscribe Scrolls",
                Texture = Helper.ModContent.Load<Texture2D>($"assets/Items/big-craftable.png"),
                Price = 0,
                Recipe = new BigCraftableRecipe()
                {
                    ResultCount = 1,
                    Ingredients =
                    {
                        new BigCraftableIngredient()
                        {
                            Object = "Stone",
                            Count = 1
                        }
                    },
                    IsDefault = true

                }
            });
        }
    }
}

