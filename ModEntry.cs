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
using RuneMagic.Skills;
using System.Collections.Generic;

namespace RuneMagic
{
    public sealed class ModEntry : Mod
    {
        //instance of the Mod class
        public static ModEntry Instance;
        private JsonAssets.IApi JsonAssetsApi;
        private SpaceCore.IApi SpaceCoreApi;
        private static MagicSkill Skill;


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
            RegisterSkill(Skill = new MagicSkill());


        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;
            SpaceCoreApi = Helper.ModRegistry.GetApi<SpaceCore.IApi>("spacechase0.SpaceCore");
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


            if (e.NameWithoutLocale.IsEquivalentTo("Maps/springobjects"))
            {
                //(384, 4096)
                Monitor.Log($"{e.NameWithoutLocale}", LogLevel.Alert);
                e.Edit(asset =>
                {
                    var editor = asset.AsImage();
                    IRawTextureData sourceImage = Helper.ModContent.Load<IRawTextureData>("assets/Glyphs/glyph1.png");
                    editor.PatchImage(sourceImage, targetArea: new Rectangle(0, 624, 16, 16), patchMode: PatchMode.Overlay);
                });
            }


            //if (e.NameWithoutLocale.IsEquivalentTo("Data/ObjectInformation"))
            //{
            //    e.Edit(asset =>
            //    {
            //        asset.AsDictionary<int, string>().Data.Add(3100, "Rune of Testing/0/-300/Basic/Rune of Testing/Rune of Testing Description.");
            //    });
            //}
        }
        private void OnItemsRegistered(object sender, EventArgs e)
        {
            RegisterRuneMagic(sender, e);
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
            ManageRunes(Game1.player);
        }
        private void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {
            var runes = JsonAssetsApi.GetAllObjectIds();
            //if item added to inventory is a rune
            if (e.Added.Any())
            {
                foreach (var item in e.Added)
                {
                    //check if the JsonAssetsApi.GetObjectId(item.Name) is in the runes.keys list
                    if (runes.ContainsKey(JsonAssetsApi.GetObjectId(item.Name).ToString()))
                    {
                        Monitor.Log("Rune added to inventory", LogLevel.Alert);
                    }
                }
            }

        }
        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            //if the last two digits on e.NewTime are 00
            if (e.NewTime % 100 == 0)
            {
                //regenerate runes
                foreach (var item in Game1.player.Items)
                {
                    if (item is Rune rune)
                    {
                        rune.AddCharges(1);
                    }
                }
            }

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
            //foreach (Item item in Game1.player.Items)
            //{
            //    if (item is Rune rune && rune.Spell == null)
            //    {
            //        rune.InitializeSpell();
            //    }
            //}
            //Game1.player.AddCustomSkillExperience(Skill, 2150);

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




        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight)
            {
                if (Game1.player.CurrentItem is Rune rune)
                {
                    rune.Activate();

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

        private void ManageRunes(Farmer player)
        {
            if (player is null)
            {
                return;
            }

            for (int i = 0; i < player.Items.Count; i++)
            {

                if (player.Items[i] != null)
                {
                    if (player.Items[i] is not Rune)
                    {
                        if (player.Items[i].Name.Contains("Rune of "))
                        {
                            player.Items[i] = new Rune(player.Items[i].ParentSheetIndex, player.Items[i].Stack);
                        }
                    }
                    else
                    {
                        (player.Items[i] as Rune).UpdateCooldown();
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

        private void RegisterRuneMagic(object sender, EventArgs e)
        {

            JsonAssets.Mod.instance.RegisterBigCraftable(ModManifest, new BigCraftableData()
            {
                Name = $"Runic Anvil",
                Description = $"Anvil used to carve Runes",
                Texture = Helper.ModContent.Load<Texture2D>($"assets/Textures/Items/big-craftable.png"),
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
                Texture = Helper.ModContent.Load<Texture2D>($"assets/Textures/Items/big-craftable.png"),
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

            int glyphIndex = 0;
            string[] spells = typeof(ModEntry).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToArray();
            for (int i = 0; i < spells.Length; i++)
            {
                Spell spell = (Spell)Activator.CreateInstance(Type.GetType($"RuneMagic.Spells.{spells[i]}"));
                if (glyphIndex > 6)
                    glyphIndex = 0;
                Texture2D texture = Helper.ModContent.Load<Texture2D>($"assets/Glyphs/glyph-{glyphIndex}.png");
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                for (int j = 0; j < data.Length; ++j)
                {
                    if (data[j] == Color.White)
                        data[j] = spell.GetColor();
                }
                texture.SetData(data);
                JsonAssets.Mod.instance.RegisterObject(ModManifest, new ObjectData()
                {
                    Name = $"Rune of {spell.Name}",
                    Description = $"{spell.Description}",

                    Texture = texture,

                    Category = ObjectCategory.Crafting,
                    CategoryTextOverride = $"{spell.School}",
                    CategoryColorOverride = spell.GetColor(),
                    Price = 0,
                    //ContextTags = new List<string>(new[] { "color_red" }),
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
                glyphIndex++;
            }
        }

    }
}

