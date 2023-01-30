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
using StardewValley.Tools;
using StardewValley.Locations;
using StardewValley.Monsters;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace RuneMagic.Source
{
    public sealed class RuneMagic : Mod
    {

        public static RuneMagic Instance { get; private set; }
        public static PlayerStats PlayerStats = new();
        public static Farmer Farmer { get; set; } = Game1.player;
        public static List<Spell> Spells { get; set; }

        public JsonAssets.IApi JsonAssetsApi;
        public IApi SpaceCoreApi;

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            RegisterSpells();
            RegisterCustomCraftingStations();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.Saving += OnSaving;
            SpaceCore.Events.SpaceEvents.OnEventFinished += OnEventFinished;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Player.Warped += OnWarped;
            RegisterSkill(PlayerStats.MagicSkill = new MagicSkill());

        }

        //Event Handlers
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {

            JsonAssetsApi = Helper.ModRegistry.GetApi<JsonAssets.IApi>("spacechase0.JsonAssets");
            JsonAssetsApi.ItemsRegistered += OnItemsRegistered;
            SpaceCoreApi = Helper.ModRegistry.GetApi<IApi>("spacechase0.SpaceCore");
            SpaceCoreApi.RegisterSerializerType(typeof(Rune));
            SpaceCoreApi.RegisterSerializerType(typeof(Scroll));
            SpaceCoreApi.RegisterSerializerType(typeof(MagicWeapon));
            SpaceCoreApi.RegisterSerializerType(typeof(Spell));
        }
        private void OnItemsRegistered(object sender, EventArgs e)
        {
            int textureIndex = 0;
            foreach (var spell in Spells)
            {

                int textureCount = Directory.GetFiles(Path.Combine(Helper.DirectoryPath, "assets", "Runes")).Length;

                var texture = Instance.Helper.ModContent.Load<Texture2D>($"assets/Runes/rune-{textureIndex}.png");
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
                RegisterJasonAssets(typeof(ObjectData), $"Rune of {spell.Name}", $"{spell.Description}", texture,
                new() { new ObjectIngredient() { Object = "Blank Rune", Count = 1 }, new ObjectIngredient() { Object = "Magic Dust", Count = 10 }, });
                textureIndex++;
                if (textureIndex >= textureCount)
                    textureIndex = 0;

                texture = Instance.Helper.ModContent.Load<Texture2D>($"assets/Items/scroll.png");
                data = new Color[texture.Width * texture.Height];
                texture.GetData(data);
                for (int j = 0; j < data.Length; ++j)
                {
                    if (data[j] == Color.White)
                        data[j] = spell.GetColor()[0];
                    if (data[j] == Color.Black)
                        data[j] = spell.GetColor()[1];
                }
                texture.SetData(data);
                RegisterJasonAssets(typeof(ObjectData), $"{spell.Name} Scroll", $"{spell.Description}", texture,
                       new() { new ObjectIngredient() { Object = "Blank Parchment", Count = 1 }, new ObjectIngredient() { Object = "Magic Dust", Count = 1 }, });

            }

            //Register Crafting Stations
            RegisterJasonAssets(typeof(BigCraftableData), "Runic Anvil", "An anvil marked with strange runes.", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/big-craftable.png"),
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            RegisterJasonAssets(typeof(BigCraftableData), "Inscription Table", "A table marked with strange runes.", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/big-craftable.png"),
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            RegisterJasonAssets(typeof(BigCraftableData), "Magic Grinder", "It's used to produce magic dust for glyphs.", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/big_craftable.png"),
                new() { new BigCraftableIngredient() { Object = "Stone", Count = 1 }, });
            //Register other Objects
            RegisterJasonAssets(typeof(ObjectData), "Blank Rune", "A stone carved and prepared to carve runes in it.", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/blank_rune.png"),
                new() { new ObjectIngredient() { Object = "Stone", Count = 1 }, });
            RegisterJasonAssets(typeof(ObjectData), "Blank Parchment", "A peace of parchment ready for inscribing", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/blank_parchment.png"),
                new() { new ObjectIngredient() { Object = "Fiber", Count = 1 }, });
            RegisterJasonAssets(typeof(ObjectData), "Magic Dust", "Magically processed dust obtained from Gems", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/magic_dust.png"), null);
            //Register Weapons
            RegisterJasonAssets(typeof(WeaponData), "Runic Staff", "Runic Staff description.", Instance.Helper.ModContent.Load<Texture2D>("assets/Items/runic_staff.png"), null, WeaponType.Club);
        }
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Farmer = Game1.player;
            Farmer.addItemToInventory(new Object(JsonAssetsApi.GetObjectId("Magic Dust"), 100));

        }
        private void OnSaving(object sender, SavingEventArgs e)
        {
            foreach (var item in Farmer.Items)
            {
                if (item is IMagicItem)
                    (item as IMagicItem).Spell = null;
            }

        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            ManageMagicItems(Game1.player, JsonAssetsApi);
            if (Context.IsWorldReady)
            {
                PlayerStats.CheckCasting(sender, e);
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
            foreach (Item item in Farmer.Items)
            {
                if (item is IMagicItem magicItem && magicItem.Spell == null)
                {
                    magicItem.InitializeSpell();
                }
            }
        }
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.R)
            {

                if (Farmer.CurrentItem is IMagicItem magicItem)
                {
                    magicItem.Use();
                }
            }
            if (e.Button == SButton.F5)
            {
                Farmer.AddCustomSkillExperience(PlayerStats.MagicSkill, 15000);
                Monitor.Log(Farmer.GetCustomSkillExperience(PlayerStats.MagicSkill).ToString());
            }



        }
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            WizardEvent(e.NewLocation);
        }

        //Registering Methods
        public void RegisterSpells()
        {
            var spells = typeof(RuneMagic).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToList();
            Spells = new List<Spell>();
            for (int i = 0; i < spells.Count; i++)
            {
                Spells.Add((Spell)Activator.CreateInstance(Type.GetType($"RuneMagic.Spells.{spells[i]}")));
            }
        }
        public void RegisterJasonAssets(Type dataType, string name, string description, Texture2D texture, List<dynamic> ingredients = null, WeaponType weaponType = WeaponType.Club)
        {
            if (dataType == typeof(ObjectData))
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
                JsonAssets.Mod.instance.RegisterObject(Instance.ModManifest, new ObjectData()
                {
                    Name = $"{name}",
                    Description = $"{description}",
                    Texture = texture,
                    Category = ObjectCategory.Crafting,
                    Price = 0,
                    HideFromShippingCollection = true,
                    Recipe = recipe
                });
            }

            else if (dataType == typeof(BigCraftableData))
            {
                BigCraftableRecipe recipe = null;
                if (ingredients is not null)
                {
                    recipe = new BigCraftableRecipe()
                    {
                        ResultCount = 1,
                        Ingredients = ingredients.OfType<BigCraftableIngredient>().ToList(),
                        IsDefault = false
                    };
                }
                JsonAssets.Mod.instance.RegisterBigCraftable(Instance.ModManifest, new BigCraftableData()
                {
                    Name = $"{name}",
                    Description = $"{description}",
                    Texture = texture,
                    Price = 0,
                    Recipe = recipe
                });
            }
            else if (dataType == typeof(WeaponData))
                JsonAssets.Mod.instance.RegisterWeapon(Instance.ModManifest, new WeaponData()
                {
                    Name = $"{name}",
                    Description = $"{description}",
                    Texture = texture,
                    Type = weaponType,
                    MinimumDamage = 6,
                    MaximumDamage = 12,
                    Knockback = 0,
                    Speed = -1,
                    Accuracy = 100,
                    Defense = 100,
                    CritChance = 0.04,
                    ExtraSwingArea = 1
                });
        }
        public void RegisterCustomCraftingStations()
        {

            var runeRecipes = new List<string>() { "Blank Rune" };
            var scrollRecipes = new List<string>() { "Blank Parchment" };

            foreach (var spell in Spells)
            {
                if (spell.Name.Contains("_"))
                {
                    spell.Name.Replace("_", " ");
                }
                runeRecipes.Add($"Rune of {spell.Name}");
                scrollRecipes.Add($"{spell.Name} Scroll");
            }

            var craftingStations = new List<Dictionary<string, object>> {
                new Dictionary<string, object> { { "BigCraftable", "Runic Anvil" }, { "ExclusiveRecipes", true }, { "CraftingRecipes", runeRecipes } },
                new Dictionary<string, object> { { "BigCraftable", "Inscription Table" }, { "ExclusiveRecipes", true }, { "CraftingRecipes", scrollRecipes } } };

            var json = JsonConvert.SerializeObject(new Dictionary<string, object> { { "CraftingStations", craftingStations } }, Formatting.Indented);

            string rootPath = Path.Combine(RuneMagic.Instance.Helper.DirectoryPath, "..", "[RM]ContentPacks/[CCS]RuneMagic/");
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
                    List<string> itemsFromPack = new(jsonAssetsApi.GetAllObjectsFromContentPack("fierro.rune_magic"));

                    if (inventory[i] is not IMagicItem and not null)
                    {
                        if (itemsFromPack.Contains(inventory[i].Name))
                        {
                            if (inventory[i].Name.Contains("Rune of "))
                                player.Items[i] = new Rune(inventory[i].ParentSheetIndex, inventory[i].Stack);
                            if (inventory[i].Name.Contains(" Scroll"))
                                player.Items[i] = new Scroll(inventory[i].ParentSheetIndex, inventory[i].Stack);
                            if (inventory[i].Name.Contains("Runic Staff"))
                            {
                                player.Items[i] = new MagicWeapon(inventory[i].ParentSheetIndex);
                            }
                        }

                    }
                    if (inventory[i] is IMagicItem magicItem)
                    {
                        magicItem.Update();
                    }

                }
        }
        public void WizardEvent(GameLocation location)
        {
            RuneMagic.Instance.Monitor.Log(PlayerStats.MagicLearned.ToString());
            if (location.Name == "WizardHouse" && Farmer.getFriendshipHeartLevelForNPC("Wizard") >= 6 && PlayerStats.MagicLearned == false)
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
                Farmer.AddCustomSkillExperience(PlayerStats.MagicSkill, 100);
                Farmer.addItemToInventory(new MagicWeapon(RuneMagic.Instance.JsonAssetsApi.GetWeaponId("Runic Staff")));

                PlayerStats.MagicLearned = true;
            }
        }
    }
}


