using JsonAssets.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using RuneMagic.Source.Items;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Object = StardewValley.Object;

namespace RuneMagic.Source
{
    public sealed class RuneMagic : Mod
    {
        public static RuneMagic Instance { get; private set; }
        public static PlayerStats PlayerStats { get; private set; } = new PlayerStats();
        public static List<Spell> Spells { get; private set; }

        public static Dictionary<string, Texture2D> Textures;

        public static JsonAssets.IApi JsonAssetsApi { get; private set; }
        public static IApi SpaceCoreApi { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            LoadTextures();
            RegisterSpells();
            RegisterCustomCraftingStations();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.Warped += OnWarped;

            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            SpaceCore.Events.SpaceEvents.OnBlankSave += OnBlankSave;

            PlayerStats.MagicSkill = new MagicSkill();
            SpaceCore.Skills.RegisterSkill(PlayerStats.MagicSkill);
        }

        //Event Handlers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            SpaceCoreApi = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            SpaceCoreApi.RegisterSerializerType(typeof(Rune));
            SpaceCoreApi.RegisterSerializerType(typeof(Scroll));
            SpaceCoreApi.RegisterSerializerType(typeof(MagicWeapon));
            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;
        }

        private void OnItemsRegistered(object sender, EventArgs e)
        {
            var jsonAssetsInstance = JsonAssets.Mod.instance;

            //Register BigCraftables
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Runic Anvil", "An anvil marked with strange runes.", Textures["item_big_craftable"],
                new() { new BigCraftableIngredient() { Object = "Iron Bar", Count = 20 }, new BigCraftableIngredient() { Object = "Amethyst", Count = 25 }, }));
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Inscription Table", "A table marked with strange runes.", Textures["item_big_craftable"],
                new() { new BigCraftableIngredient() { Object = "Wood", Count = 40 }, new BigCraftableIngredient() { Object = "Amethyst", Count = 25 }, }));
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Magic Grinder", "It's used to produce magic dust for glyphs.", Textures["item_magic_grinder"],
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 40 }, new BigCraftableIngredient() { Object = "Topaz", Count = 25 }, }));

            //Register Runes and Scrolls
            int textureIndex = 0;
            foreach (var spell in Spells)
            {
                jsonAssetsInstance.RegisterObject(ModManifest, SetScrollData(spell));
                jsonAssetsInstance.RegisterObject(ModManifest, SetRuneData(spell, textureIndex));
                if (textureIndex == 8)
                    textureIndex = 0;
                else
                    textureIndex++;
            }

            //Register other Objects
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Blank Rune", "A stone carved and prepared to carve runes in it.", Textures[$"item_blank_rune"],
                new List<ObjectIngredient>() { new ObjectIngredient() { Object = "Stone", Count = 1 }, }));
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Blank Parchment", "A peace of parchment ready for inscribing.", Textures[$"item_blank_parchment"],
                new List<ObjectIngredient>() { new ObjectIngredient() { Object = "Fiber", Count = 1 }, }));
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Magic Dust", "Magically processed dust obtained from Gems", Textures[$"item_magic_dust"], null));

            //Register Weapons
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Apprentice Staff", "A stick with strange markings in it.", Textures[$"item_apprentice_staff"], WeaponType.Club, 10));
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Adept Staff", "A stick with strange markings in it.", Textures[$"item_adept_staff"], WeaponType.Club, 80));
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Master Staff", "A stick with strange markings in it.", Textures[$"item_master_staff"], WeaponType.Club, 120));
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
        }

        private void OnBlankSave(object sender, EventArgs e)
        {
            Game1.player.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Magic Dust"), 100));
            Game1.player.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Blank Rune"), 100));
            Game1.player.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Blank Parchment"), 100));
            Game1.player.addItemToInventory(new Object(390, 2));
        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            foreach (var item in Game1.player.Items)
            {
                if (item is IMagicItem)
                    (item as IMagicItem).Spell = null;
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            ManageMagicItems(Game1.player, JsonAssetsApi);
            PlayerStats.Cast(Game1.player.CurrentItem as IMagicItem);

            if (Game1.player.ActiveObject is IMagicItem)
            {
                PlayerAnimation();
            }
        }

        private void OnEventFinished(object sender, EventArgs e)
        {
            if (Game1.CurrentEvent.id == 15065001)
            {
                Game1.player.AddCustomSkillExperience(PlayerStats.MagicSkill, 100);
            }
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            PlayerStats.LearnRecipes();
            foreach (Item item in Game1.player.Items)
            {
                if (item is IMagicItem magicItem && magicItem.Spell == null)
                {
                    magicItem.InitializeSpell();
                }
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                if (e.Button == SButton.Q)
                {
                    if (Game1.player.HasCustomProfession(MagicSkill.Runemaster) && Game1.player.CurrentItem is Rune rune)
                    {
                        if (!rune.RunemasterActive && rune.Charges >= 3)
                        {
                            rune.RunemasterActive = true;
                            rune.Spell.CastingTime = 0.01f;
                        }
                        else
                        {
                            rune.RunemasterActive = false;
                            rune.Spell.CastingTime = 1 + (rune.Spell.Level / 10f) * 1.5f;
                        }
                    }
                }

                if (e.Button == SButton.F5)
                {
                    Game1.player.AddCustomSkillExperience(PlayerStats.MagicSkill, 15000);
                    Monitor.Log(Game1.player.GetCustomSkillExperience(PlayerStats.MagicSkill).ToString());
                }
            }
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            WizardEvent(e.NewLocation);
        }

        //Registering Methods
        public void LoadTextures()
        {
            Textures = new Dictionary<string, Texture2D>();
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Spells"))
            {
                Textures.Add($"spell_{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Spells/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Runes"))
            {
                Textures.Add($"rune_{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Runes/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Items"))
            {
                Textures.Add($"item_{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Items/{Path.GetFileName(file)}"));
            }
            //print the Textures keys on console as alert
            foreach (var texture in Textures)
            {
                Monitor.Log(texture.Key);
            }
        }

        public void RegisterSpells()
        {
            var spellTypes = typeof(RuneMagic).Assembly
              .GetTypes()
              .Where(t => t.Namespace == "RuneMagic.Source.Spells" && typeof(Spell).IsAssignableFrom(t));

            Spells = spellTypes.Select(t => (Spell)Activator.CreateInstance(t)).ToList();
            Spells = Spells.OrderBy(s => s.Level).ThenBy(s => s.School).ThenBy(s => s.Name).ToList();
            var spellGroups = Spells.GroupBy(s => s.Level);
            Instance.Monitor.Log($"Registering Spells...", LogLevel.Debug);
            foreach (var spellGroup in spellGroups)
            {
                Instance.Monitor.Log($"--------------Level {spellGroup.Key} Spells--------------", LogLevel.Debug);
                foreach (var spell in spellGroup)
                {
                    Instance.Monitor.Log($"\t{spell.Name,-25}REGISTERED \t {spell.School}", LogLevel.Debug);
                }
            }
            Spells = Spells.OrderBy(s => s.School).ThenBy(s => s.Level).ThenBy(s => s.Name).ToList();
        }

        public BigCraftableData SetBigCraftableData(string name, string description, Texture2D texture, List<BigCraftableIngredient> ingredients)
        {
            BigCraftableRecipe recipe = null;
            if (ingredients is not null)
            {
                recipe = new BigCraftableRecipe()
                {
                    ResultCount = 1,
                    Ingredients = ingredients.ToList(),
                    IsDefault = false
                };
            }
            return new BigCraftableData()
            {
                Name = $"{name}",
                Description = $"{description}",
                Texture = texture,
                Price = 0,
                Recipe = recipe
            };
        }

        public ObjectData SetObjectData(string name, string description, Texture2D texture, List<ObjectIngredient> ingredients)
        {
            ObjectRecipe recipe = null;
            if (ingredients is not null)
            {
                recipe = new ObjectRecipe()
                {
                    ResultCount = 1,
                    Ingredients = ingredients.OfType<ObjectIngredient>().ToList(),
                    IsDefault = false
                };
            }
            return new ObjectData()
            {
                Name = $"{name}",
                Description = $"{description}",
                Texture = texture,
                Category = ObjectCategory.Crafting,
                Price = 0,
                HideFromShippingCollection = true,
                Recipe = recipe
            };
        }

        public WeaponData SetWeaponData(string name, string description, Texture2D texture, WeaponType type, int mineDropVar)
        {
            return new WeaponData()
            {
                Name = $"{name}",
                Description = $"{description}",
                Texture = texture,
                Type = type,
                MinimumDamage = 6,
                MaximumDamage = 12,
                Knockback = 0,
                Speed = -20,
                Accuracy = 100,
                Defense = 1,
                CritChance = 0.04,
                CritMultiplier = 1.5,
                ExtraSwingArea = 0,
                CanPurchase = true,
                CanTrash = true,
                MineDropVar = mineDropVar,
                MineDropMinimumLevel = 10,
            };
        }

        public static ObjectData SetRuneData(Spell spell, int textureIndex)
        {
            var texture = Instance.Helper.ModContent.Load<Texture2D>($"assets/Runes/{textureIndex}");
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int j = 0; j < data.Length; ++j)
            {
                if (data[j] == Color.White)
                    data[j] = spell.GetColor()[0];
                if (data[j] == Color.Black)
                    data[j] = spell.GetColor()[1];
            }
            texture.SetData(data);
            return new ObjectData()
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
                    IsDefault = false,
                    Ingredients = new List<ObjectIngredient>()
                    {
                        new ObjectIngredient()
                        {
                            Object = "Blank Rune",
                            Count = 1,
                        },
                         new ObjectIngredient()
                        {
                            Object = "Magic Dust",
                            Count = 5 + spell.Level * 2,
                        },
                    }
                }
            };
        }

        public ObjectData SetScrollData(Spell spell)
        {
            var texture = Instance.Helper.ModContent.Load<Texture2D>($"assets/Items/scroll.png");
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] == Color.White)
                    data[i] = spell.GetColor()[0];
                if (data[i] == Color.Black)
                    data[i] = spell.GetColor()[1];
            }
            texture.SetData(data);
            return new ObjectData()
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
                    IsDefault = false,
                    Ingredients = new List<ObjectIngredient>()
                    {
                        new ObjectIngredient()
                        {
                            Object = "Blank Parchment",
                            Count = 1,
                        },
                         new ObjectIngredient()
                        {
                            Object = "Magic Dust",
                            Count = spell.Level,
                        },
                    }
                }
            };
        }

        public void RegisterCustomCraftingStations()

        {
            var runeRecipes = new List<string>() { "Blank Rune" };
            var scrollRecipes = new List<string>() { "Blank Parchment" };

            foreach (var spell in Spells)
            {
                if (spell.Name.Contains("_"))
                {
                    _ = spell.Name.Replace("_", " ");
                }
                runeRecipes.Add($"Rune of {spell.Name}");
                scrollRecipes.Add($"{spell.Name} Scroll");
            }

            var craftingStations = new List<Dictionary<string, object>> {
                new Dictionary<string, object> { { "BigCraftable", "Runic Anvil" }, { "ExclusiveRecipes", true }, { "CraftingRecipes", runeRecipes } },
                new Dictionary<string, object> { { "BigCraftable", "Inscription Table" }, { "ExclusiveRecipes", true }, { "CraftingRecipes", scrollRecipes } } };

            var json = JsonConvert.SerializeObject(new Dictionary<string, object> { { "CraftingStations", craftingStations } }, Formatting.Indented);

            string rootPath = Path.Combine(Instance.Helper.DirectoryPath, "..", "[RM]ContentPacks/[CCS]RuneMagic/");
            string fileName = "content.json";
            string fullPath = Path.Combine(rootPath, fileName);

            File.WriteAllText(fullPath, json);
        }

        //Game Management Methods
        public void ManageMagicItems(Farmer player, JsonAssets.IApi jsonAssetsApi)
        {
            if (Context.IsWorldReady)
                for (int i = 0; i < player.Items.Count; i++)
                {
                    var inventory = player.Items;
                    List<string> objectsFromPack = new(jsonAssetsApi.GetAllObjectsFromContentPack("fierro.rune_magic"));
                    List<string> weaponsFromPack = new(jsonAssetsApi.GetAllWeaponsFromContentPack("fierro.rune_magic"));

                    if (inventory[i] is not IMagicItem and not null)
                    {
                        if (objectsFromPack.Contains(inventory[i].Name))
                        {
                            if (inventory[i].Name.Contains("Rune of "))
                                player.Items[i] = new Rune(inventory[i].ParentSheetIndex, inventory[i].Stack);
                            if (inventory[i].Name.Contains(" Scroll"))
                                player.Items[i] = new Scroll(inventory[i].ParentSheetIndex, inventory[i].Stack);
                        }
                        if (weaponsFromPack.Contains(inventory[i].Name))
                        {
                            player.Items[i] = new MagicWeapon(JsonAssetsApi.GetWeaponId(inventory[i].Name));
                        }
                    }
                    if (inventory[i] is IMagicItem magicItem)
                    {
                        magicItem.Update();
                        magicItem.Spell?.Update();
                    }
                }
        }

        public void WizardEvent(GameLocation location)
        {
            //Instance.Monitor.Log(PlayerStats.MagicLearned.ToString());
            if (location.Name == "WizardHouse" && Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 6 && PlayerStats.MagicLearned == false)
            {
                var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                       $"/speak Wizard \"@! Come in my friend, come in...\"" +
                       $"/pause 400" +
                       $"/advancedMove Wizard false -2 0 3 100 0 2 2 3000" +
                       $"/move farmer 0 -6 0 true" +
                       $"/pause 2000" +
                       $"/speak Wizard \"What do you think about this? Beautiful, isn't it?\"" +
                       $"/pause 500" +
                       $"/speak Wizard \"It's a Magic Staff.\"" +
                       $"/pause 500" +
                       $"/speak Wizard \"It is a gift for you...\"" +
                       $"/pause 1000" +
                       $"/speak Wizard \"Now pay attention, young adept. I will teach you the bases you will need to learn Magic!\"" +
                       $"/end";
                location.startEvent(new Event(eventString, 15065001));
                Game1.player.AddCustomSkillExperience(PlayerStats.MagicSkill, 100);
                Game1.player.addItemToInventory(new MagicWeapon(JsonAssetsApi.GetWeaponId("Apprentice Staff")));
                PlayerStats.MagicLearned = true;
            }
        }

        public void PlayerAnimation()
        {
            var time = 1;
            if (!Game1.player.FarmerSprite.PauseForSingleAnimation && !Game1.player.UsingTool)
            {
                if (Game1.player.isRidingHorse() && !Game1.player.mount.dismounting.Value)
                {
                    Game1.player.showRiding();
                }
                else if (Game1.player.FacingDirection == 3 && Game1.player.isMoving() && Game1.player.running)
                {
                    Game1.player.FarmerSprite.animate(56, time);
                }
                else if (Game1.player.FacingDirection == 1 && Game1.player.isMoving() && Game1.player.running)
                {
                    Game1.player.FarmerSprite.animate(40, time);
                }
                else if (Game1.player.FacingDirection == 0 && Game1.player.isMoving() && Game1.player.running)
                {
                    Game1.player.FarmerSprite.animate(48, time);
                }
                else if (Game1.player.FacingDirection == 2 && Game1.player.isMoving() && Game1.player.running)
                {
                    Game1.player.FarmerSprite.animate(32, time);
                }
                else if (Game1.player.FacingDirection == 3 && Game1.player.isMoving())
                {
                    Game1.player.FarmerSprite.animate(24, time);
                }
                else if (Game1.player.FacingDirection == 1 && Game1.player.isMoving())
                {
                    Game1.player.FarmerSprite.animate(8, time);
                }
                else if (Game1.player.FacingDirection == 0 && Game1.player.isMoving())
                {
                    Game1.player.FarmerSprite.animate(16, time);
                }
                else if (Game1.player.FacingDirection == 2 && Game1.player.isMoving())
                {
                    Game1.player.FarmerSprite.animate(0, time);
                }
                else if (PlayerStats.IsCasting)
                {
                    Game1.player.faceDirection(2);
                    Game1.player.showCarrying();
                }
                else
                {
                    Game1.player.showNotCarrying();
                }
            }
        }
    }
}