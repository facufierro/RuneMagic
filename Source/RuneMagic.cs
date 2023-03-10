using JsonAssets.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using RuneMagic.Source.Interface;
using RuneMagic.Source.Items;
using SpaceCore;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using xTile.Dimensions;
using static SpaceCore.Skills;
using Object = StardewValley.Object;

namespace RuneMagic.Source
{
    public sealed class RuneMagic : Mod
    {
        public static RuneMagic Instance { get; private set; }
        public static PlayerStats PlayerStats { get; private set; }
        public static List<Spell> Spells { get; private set; }
        public static Dictionary<string, Texture2D> Textures;

        public static Dictionary<School, MagicSkill> MagicSkills { get; set; }
        public static JsonAssets.IApi JsonAssetsApi { get; private set; }
        public static IApi SpaceCoreApi { get; private set; }
        public static IGenericModConfigMenuApi ConfigMenuApi { get; private set; }

        public static ModConfig Config { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            Config = Helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.Warped += OnWarped;
            helper.Events.Display.RenderedHud += OnRenderedHud;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            SpaceCore.Events.SpaceEvents.OnBlankSave += OnBlankSave;

            LoadTextures();
            MagicSkills = new Dictionary<School, MagicSkill>()
            {
                {School.Abjuration,new(School.Abjuration) },
                {School.Alteration,new(School.Alteration) },
                {School.Conjuration,new(School.Conjuration) },
                {School.Evocation,new(School.Evocation) },
            };
            PlayerStats = new PlayerStats();

            RegisterSpells();
        }

        //Event Handlers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            SpaceCoreApi = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            ConfigMenuApi = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            SpaceCoreApi.RegisterSerializerType(typeof(SpellBook));
            SpaceCoreApi.RegisterSerializerType(typeof(Rune));
            SpaceCoreApi.RegisterSerializerType(typeof(Scroll));
            SpaceCoreApi.RegisterSerializerType(typeof(MagicWeapon));

            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;

            RegisterConfigMenu();
        }

        private void OnItemsRegistered(object sender, EventArgs e)
        {
            var jsonAssetsInstance = JsonAssets.Mod.instance;

            //Register BigCraftables
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Runic Anvil", "An anvil marked with strange runes.", Textures["big_craftable"],
                new() { new BigCraftableIngredient() { Object = "Iron Bar", Count = 20 }, new BigCraftableIngredient() { Object = "Amethyst", Count = 25 }, }));
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Inscription Table", "A table marked with strange runes.", Textures["big_craftable"],
                new() { new BigCraftableIngredient() { Object = "Wood", Count = 40 }, new BigCraftableIngredient() { Object = "Amethyst", Count = 25 }, }));
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Magic Grinder", "It's used to produce magic dust for glyphs.", Textures["magic_grinder"],
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 40 }, new BigCraftableIngredient() { Object = "Topaz", Count = 25 }, }));
            jsonAssetsInstance.RegisterBigCraftable(ModManifest, SetBigCraftableData("Spell Excavation", "", Textures["excavation"], null));
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
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Blank Rune", "A stone carved and prepared to carve runes in it.", Textures[$"blank_rune"],
                new List<ObjectIngredient>() { new ObjectIngredient() { Object = "Stone", Count = 1 }, }));
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Blank Parchment", "A peace of parchment ready for inscribing.", Textures[$"blank_parchment"],
                new List<ObjectIngredient>() { new ObjectIngredient() { Object = "Fiber", Count = 1 }, }));
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Magic Dust", "Magically processed dust obtained from Gems", Textures[$"magic_dust"], null));
            jsonAssetsInstance.RegisterObject(ModManifest, SetObjectData("Spell Book", "A magic journal with intricate symbols on cover and pages detailing secrets of spellcasting.", Textures[$"spell_book"], null));
            //Register Weapons
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Apprentice Staff", "A stick with strange markings in it.", Textures[$"apprentice_staff"], WeaponType.Club, 10));
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Adept Staff", "A stick with strange markings in it.", Textures[$"adept_staff"], WeaponType.Club, 80));
            jsonAssetsInstance.RegisterWeapon(ModManifest, SetWeaponData("Master Staff", "A stick with strange markings in it.", Textures[$"master_staff"], WeaponType.Club, 120));
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
        }

        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            PlayerStats.LearnRecipes();

            if (Game1.player.CurrentItem is ISpellCastingItem spellItem)
            {
                if (Instance.Helper.Input.IsDown(Config.ActionBarKey))
                {
                    if (spellItem is SpellBook spellBook)
                        spellBook.ActionBar.Render(e.SpriteBatch);
                }
                if (Instance.Helper.Input.IsDown(Config.CastKey))
                {
                    PlayerStats.CastBar.Render(e.SpriteBatch, spellItem);
                }
            }
        }

        private void OnBlankSave(object sender, EventArgs e)
        {
            if (Config.DevMode)
            {
                Game1.player.addItemByMenuIfNecessary(new SpellBook(JsonAssetsApi.GetObjectId("Spell Book"), 1));
                Game1.player.addItemByMenuIfNecessary(new Object(Vector2.Zero, JsonAssetsApi.GetBigCraftableId("Runic Anvil")));
                Game1.player.addItemByMenuIfNecessary(new Object(Vector2.Zero, JsonAssetsApi.GetBigCraftableId("Inscription Table")));
                Game1.player.addItemByMenuIfNecessary(new Object(JsonAssetsApi.GetObjectId("Magic Dust"), 100));
                Game1.player.addItemByMenuIfNecessary(new Object(JsonAssetsApi.GetObjectId("Blank Rune"), 100));
                Game1.player.addItemByMenuIfNecessary(new Object(JsonAssetsApi.GetObjectId("Blank Parchment"), 100));

                PlayerStats.MagicLearned = true;
                PlayerStats.RuneCarving = true;
                PlayerStats.ScrollScribing = true;
            }
        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            foreach (var item in Game1.player.Items)
            {
                if (item is ISpellCastingItem)
                    (item as ISpellCastingItem).Spell = null;
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            PlayerStats.Update();
            if (Game1.player.CurrentItem is ISpellCastingItem)
            {
                //PlayerStats.Cast(Game1.player.CurrentItem as ISpellCastingItem);
                PlayerAnimation();
            }
        }

        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
        }

        private void OnEventFinished(object sender, EventArgs e)
        {
            if (Game1.CurrentEvent.id == 15065001)
            {
                Game1.player.addItemToInventory(new SpellBook(JsonAssetsApi.GetObjectId("Spell Book"), 1));

                PlayerStats.MagicLearned = true;
                PlayerStats.LearnRecipes();
                Response[] responses = new List<Response>()
                {
                    new Response("Abjuration", "Abjuration"),
                    new Response("Alteration", "Alteration"),
                    new Response("Conjuration", "Conjuration"),
                    new Response("Evocation", "Evocation")
                }.ToArray();

                Game1.currentLocation.createQuestionDialogue("Choose your Specialization:", responses, (Farmer f, string responseKey) =>
                {
                    PlayerStats.MagicSkill = MagicSkills[(School)Enum.Parse(typeof(School), responseKey)];
                });
            }
            if (Game1.CurrentEvent.id == 15065002)
            {
                PlayerStats.ScrollScribing = true;
                PlayerStats.LearnRecipes();
            }
            if (Game1.CurrentEvent.id == 15065003)
            {
                PlayerStats.RuneCarving = true;
                PlayerStats.LearnRecipes();
            }
            if (Game1.CurrentEvent.id == 15065004)
            {
                PlayerStats.RuneCarving = true;
                PlayerStats.LearnRecipes();
            }
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            foreach (Item item in Game1.player.Items)
            {
                if (item is ISpellCastingItem magicItem && magicItem.Spell == null)
                {
                    magicItem.InitializeSpell();
                }
            }

            //if the player has a SpellBook in the inventory set all its slots to active
            foreach (Item item in Game1.player.Items)
            {
                if (item is SpellBook spellBook)
                {
                    spellBook.UpdateSpellSlots();
                }
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                if (e.Button == SButton.MouseRight)
                    PlayerStats.MagicCraftingActions();
                if (e.Button == Config.SpellBookKey)
                    //if the player has a spellbook in inventory open spellmenu
                    foreach (Item item in Game1.player.Items)
                    {
                        if (item is SpellBook spellBook)
                        {
                            Game1.activeClickableMenu = new SpellBookMenu(spellBook);
                            break;
                        }
                    }

                if (Config.DevMode)
                {
                    switch (e.Button)
                    {
                        case SButton.F9:
                            PlayerStats.MagicSkill.Experience += 100;
                            break;

                        case SButton.F11:
                            Game1.nextMineLevel();
                            break;

                        case SButton.F12:

                            Game1.warpFarmer("UndergroundMine2", 10, 4, false);
                            break;
                    }
                }
            }
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            TriggerEvent(e.NewLocation);
            Monitor.Log(e.NewLocation.Name);
        }

        //Registering Methods
        public void LoadTextures()
        {
            Textures = new Dictionary<string, Texture2D>();
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Spells"))
            {
                Textures.Add($"{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Spells/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Runes"))
            {
                Textures.Add($"rune_{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Runes/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Glyphs"))
            {
                Textures.Add($"glyph_{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Glyphs/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Items"))
            {
                Textures.Add($"{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Items/{Path.GetFileName(file)}"));
            }
            foreach (var file in Directory.GetFiles($"{Helper.DirectoryPath}/assets/Interface"))
            {
                Textures.Add($"{Path.GetFileNameWithoutExtension(file)}", Helper.ModContent.Load<Texture2D>($"assets/Interface/{Path.GetFileName(file)}"));
            }
            //print the Textures keys on console as alert
            foreach (var texture in Textures)
            {
                Monitor.Log(texture.Key);
            }
        }

        private void RegisterSpells()
        {
            var spellTypes = typeof(RuneMagic).Assembly
            .GetTypes()
            .Where(t => t.Namespace == "RuneMagic.Source.Spells" && typeof(Spell).IsAssignableFrom(t));

            Spells = spellTypes
                .Select(t =>
                {
                    ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
                    return constructor.Invoke(null) as Spell;
                })
                .ToList();

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
            Spells = Spells.OrderBy(s => s.Level).ThenBy(s => s.School).ThenBy(s => s.Name).ToList();
            //get the number of files in the Glyphs folder
            var glyphIndex = 0;
            foreach (var spell in Spells)
            {
                if (glyphIndex >= Directory.GetFiles($"{Helper.DirectoryPath}/assets/Glyphs").Length)
                    glyphIndex = 0;
                spell.SetGlyph(Instance.Helper.ModContent.Load<Texture2D>($"assets/Glyphs/{glyphIndex}"));
                glyphIndex++;
            }
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
                Recipe = recipe,
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

        public ObjectData SetRuneData(Spell spell, int textureIndex)
        {
            var texture = Instance.Helper.ModContent.Load<Texture2D>($"assets/Runes/{textureIndex}");
            var data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i] == Color.White)
                    data[i] = spell.Skill.Colors.Item1;
                if (data[i] == Color.Black)
                    data[i] = spell.Skill.Colors.Item2;
            }
            texture.SetData(data);
            return new ObjectData()
            {
                Name = $"Rune of {spell.Name}",
                Description = $"{spell.Description}",
                Texture = texture,
                Category = ObjectCategory.Crafting,
                CategoryTextOverride = $"{spell.School}",
                CategoryColorOverride = spell.Skill.Colors.Item2,
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
                    data[i] = spell.Skill.Colors.Item1;
                if (data[i] == Color.Black)
                    data[i] = spell.Skill.Colors.Item2;
            }
            texture.SetData(data);
            return new ObjectData()
            {
                Name = $"{spell.Name} Scroll",
                Description = $"{spell.Description}",
                Texture = texture,
                Category = ObjectCategory.Crafting,
                CategoryTextOverride = $"{spell.School}",
                CategoryColorOverride = spell.Skill.Colors.Item2,
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

        public void TriggerEvent(GameLocation location)
        {
            //Instance.Monitor.Log(PlayerStats.MagicLearned.ToString());
            if (location.Name == "WizardHouse" && Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 3 && PlayerStats.MagicLearned == false)
            {
                var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                    $"/speak Wizard \"@! Come in my friend, come in...\"" +
                    $"/pause 400" +
                    $"/advancedMove Wizard false -2 0 3 100 0 2 2 3000" +
                    $"/move farmer 0 -6 0 true" +
                    $"/speak Wizard \"What do you think of this? beautiful isn't it?\"" +
                    $"/pause 500" +
                    $"/speak Wizard \"It's a spell book, a gift for you.\"" +
                    $"/pause 1000" +
                    $"/speak Wizard \"Now pay attention, young adept! I am going to teach you everything you need to know to use it!\"" +
                    $"/end";
                location.startEvent(new Event(eventString, 15065001));
            }
            if (location.Name == "WizardHouse" && Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 5 && PlayerStats.ScrollScribing == false && PlayerStats.MagicLearned == true)
            {
                var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                       $"/speak Wizard \"@! Come in young adept, come in...\"" +
                       $"/pause 400" +
                       $"/advancedMove Wizard false -2 0 3 100 0 2 2 3000" +
                       $"/move farmer 0 -6 0 true" +
                       $"/pause 2000" +
                       $"/speak Wizard \"Today I am going to teach you a new form of magic.\"" +
                       $"/pause 500" +
                       $"/speak Wizard \"Scroll Scribing.\"" +
                       $"/pause 1000" +
                       $"/speak Wizard \"Now pay attention, this can be a bit tricky. And if not done properly even dangerous!\"" +
                       $"/end";
                location.startEvent(new Event(eventString, 15065002));
            }
            if (location.Name == "WizardHouse" && Game1.player.getFriendshipHeartLevelForNPC("Wizard") >= 6 && PlayerStats.MagicLearned == true)
            {
                //var eventString = $"WizardSong/6 18/Wizard 10 15 2 farmer 8 24 0/skippable" +
                //       $"/speak Wizard \"@... I know why you are here my friend...\"" +
                //       $"/pause 400" +
                //       $"/move farmer 0 -6 0 true" +
                //       $"/pause 2000" +
                //       $"/speak Wizard \"You want to specialize in another magic school. I understand, but it's not possible. \"" +
                //       $"/pause 500" +
                //       $"/speak Wizard \"The only way is to reverse your current knowledge and select another. I'm sorry.\"" +
                //       $"/pause 1000" +
                //       $"/speak Wizard \"I'll give you the means to do it, but be warned, it's a costly process.\"" +
                //       $"/end";
                //location.startEvent(new Event(eventString, 15065004));
            }
            if (location.Name == "Mine" && Game1.player.getFriendshipHeartLevelForNPC("Dwarf") >= 5 && PlayerStats.RuneCarving == false && Game1.player.canUnderstandDwarves && PlayerStats.MagicLearned == true)
            {
                var eventString = $"WizardSong/43 8/Dwarf 43 6 2 farmer 39 8 1/skippable" +
                       $"/speak Dwarf \"Hey!\"" +
                       $"/pause 400" +
                       $"/advancedMove farmer false 43 8 1 100 43 7 3 3000" +
                       $"/pause 2000" +
                       $"/speak Dwarf \"Did you know that dwarves know how to use magic?.\"" +
                       $"/pause 500" +
                       $"/speak Wizard \"We do it different though, like this!\"" +
                       $"/pause 1000" +
                       $"/speak Wizard \"Wanna try? It's not hard if you already know the basics.\"" +
                       $"/end";
                location.startEvent(new Event(eventString, 15065003));
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
                else if (PlayerStats.CastingTime > 0)
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

        public void RegisterConfigMenu()
        {
            if (ConfigMenuApi is null)
                return;

            ConfigMenuApi.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config));
            ConfigMenuApi.AddKeybind(
                mod: ModManifest,
                name: () => "Casting Key",
                getValue: () => Config.CastKey,
                setValue: value => Config.CastKey = value);
            ConfigMenuApi.AddKeybind(
                mod: ModManifest,
                name: () => "Spell Selection Key",
                getValue: () => Config.ActionBarKey,
                setValue: value => Config.ActionBarKey = value);
            ConfigMenuApi.AddKeybind(
                mod: ModManifest,
                name: () => "Spell Book Menu Key",
                getValue: () => Config.SpellBookKey,
                setValue: value => Config.SpellBookKey = value);

            ConfigMenuApi.AddNumberOption(
                mod: ModManifest,
                name: () => "Castbar Scale",
                getValue: () => Config.CastbarScale,
                setValue: value => Config.CastbarScale = value,
                min: 1,
                max: 3);
            ConfigMenuApi.SetTitleScreenOnlyForNextOptions(ModManifest, true);
            ConfigMenuApi.AddBoolOption(
               mod: ModManifest,
               name: () => "Developer Mode",
               getValue: () => Config.DevMode,
               setValue: value => Config.DevMode = value);
        }
    }
}