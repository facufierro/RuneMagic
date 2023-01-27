using JsonAssets.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using RuneMagic.Famework;
using RuneMagic.Framework;
using RuneMagic.Items;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace RuneMagic.Source
{
    public class RuneMagic
    {
        public PlayerStats PlayerStats { get; set; }
        public Farmer Farmer { get; set; } = Game1.player;
        public List<Spell> SpellList { get; set; }
        public RuneMagic()
        {
            PlayerStats = new PlayerStats();
        }

        public void ManageMagicItems(Farmer player, JsonAssets.IApi jsonAssetsApi)
        {
            if (Context.IsWorldReady)
                for (int i = 0; i < player.Items.Count; i++)
                {
                    var inventory = player.Items;
                    List<string> itemsFromPack = new List<string>(jsonAssetsApi.GetAllObjectsFromContentPack("fierro.rune_magic"));

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
        public void RegisterMail(IAssetData asset)
        {
            asset.AsDictionary<string, string>().Data["RuneMagicWizardLetter"] =
               "@... " +
               "^^ I have something I wish you to have but its too powerful and precious to send it trough mail. " +
               "^Please come pay me a visit when you can." +
               "^^-M. Rasmodius, Wizard";
        }
        public void RegisterEvent(IAssetData asset)
        {
            var data = asset.AsDictionary<string, string>().Data;
            data["15065001/n RuneMagicWizardLetter4"] = "";
        }
        public void RegisterSpells()
        {

            var spells = typeof(ModEntry).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToList();
            SpellList = new List<Spell>();
            for (int i = 0; i < spells.Count; i++)
            {
                SpellList.Add((Spell)Activator.CreateInstance(Type.GetType($"RuneMagic.Spells.{spells[i]}")));
            }
        }
        public void JARegisterRunes()
        {
            int index = 0;
            //add every spell to spells
            foreach (var spell in SpellList)
            {
                int textureCount = Directory.GetFiles(@$"{ModEntry.Instance.Helper.DirectoryPath}/assets/Runes").Length;
                if (index > textureCount)
                    index = 0;
                Texture2D texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"assets/Runes/rune-{index}.png");
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

                JsonAssets.Mod.instance.RegisterObject(ModEntry.Instance.ModManifest, new ObjectData()
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
                        SkillUnlockName = $"Magic",
                        SkillUnlockLevel = 1,
                        Ingredients =
                    {
                        new ObjectIngredient()
                        {
                            Object = "Blank Rune",
                            Count = 1
                        },
                        new ObjectIngredient()
                        {
                            Object = "Magic Dust",
                            Count = 5
                        },
                    },
                        IsDefault = false



                    }
                });
                index++;
            }

        }
        public void JARegisterScrolls()
        {
            var spells = typeof(ModEntry).Assembly.GetTypes().Where(t => t.Namespace == "RuneMagic.Spells").Select(t => t.Name).ToList();
            foreach (var spell in SpellList)
            {
                Texture2D texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"assets/Scrolls/scroll-0.png");
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

                JsonAssets.Mod.instance.RegisterObject(ModEntry.Instance.ModManifest, new ObjectData()
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
                            Object = "Blank Scroll",
                            Count = 1
                        },
                        new ObjectIngredient()
                        {
                            Object = "Magic Dust",
                            Count = 5
                        },
                    },
                        IsDefault = false

                    }

                });

            }
        }
        public void JARegisterCraftingStation(string name, string description, string texturePath, List<BigCraftableIngredient> ingredients)
        {
            JsonAssets.Mod.instance.RegisterBigCraftable(ModEntry.Instance.ModManifest, new BigCraftableData()
            {
                Name = $"{name}",
                Description = $"{description}",
                Texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"{texturePath}"),
                Price = 0,
                Recipe = new BigCraftableRecipe()
                {
                    ResultCount = 1,
                    Ingredients = ingredients,
                    IsDefault = false
                }
            });

        }
        public void JARegisterObject(string name, string description, string texturePath, List<ObjectIngredient> ingredients)
        {
            Texture2D texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"{texturePath}");
            JsonAssets.Mod.instance.RegisterObject(ModEntry.Instance.ModManifest, new ObjectData()
            {
                Name = $"{name}",
                Description = $"{description}",
                Texture = texture,
                Category = ObjectCategory.Crafting,
                Price = 0,
                HideFromShippingCollection = true,
                Recipe = new ObjectRecipe()
                {
                    ResultCount = 1,
                    Ingredients = ingredients,
                    IsDefault = false

                }

            });
        }
        public void CCSRegister()
        {

            var runeRecipes = new List<string>();
            var scrollRecipes = new List<string>();
            foreach (var spell in SpellList)
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

            string rootPath = Path.Combine(ModEntry.Instance.Helper.DirectoryPath, "..", "[RM]ContentPacks/[CCS]RuneMagic/");
            string fileName = "content.json";
            string fullPath = Path.Combine(rootPath, fileName);

            File.WriteAllText(fullPath, json);


        }
        public void PFMRegister()
        {
        }
        public void BCRegister()
        {

        }
    }
}
