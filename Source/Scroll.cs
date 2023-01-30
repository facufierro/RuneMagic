

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewValley;
using System;
using System.Reflection;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source
{
    [XmlType("Mods_Scroll")]
    public class Scroll : Object, IMagicItem
    {
        public Spell Spell { get; set; }

        public Scroll() : base()
        {
            InitializeSpell();
        }
        public Scroll(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            InitializeSpell();

        }

        public void InitializeSpell()
        {
            //set spellName to Name without " Scroll" at the end

            string spellName = Name[0..^7];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Source.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);

        }
        public void Use()
        {
            RuneMagic.PlayerStats.ItemHeld = this;
        }
        public void Activate()
        {
            if (!Fizzle())
                if (Spell.Cast())
                {
                    //remove an item stack from this object if Farmer doesnt have Lorekeeper profession and if it does give it a 20% chance of not consuming the scroll
                    if (!RuneMagic.Farmer.HasCustomProfession(MagicSkill.Lorekeeper) || Game1.random.Next(1, 100) > 20)
                        Stack--;
                    if (Stack <= 0)
                        RuneMagic.Farmer.removeItemFromInventory((Item)this);

                }
        }

        public void Update() { }
        public bool Fizzle()
        {

            if (Game1.random.Next(1, 100) < 0)
            {
                RuneMagic.Farmer.stamina -= 10;
                Game1.playSound("stoneCrack");
                RuneMagic.Farmer.removeItemFromInventory((Item)this);
                RuneMagic.Farmer.addItemToInventory(new Object(390, 1));
                return true;
            }
            else
                return false;
        }
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            //draw a cast bar below the item in the inventory if the player is casting and make the bar always the size of the item width and 5 pixels high, but make it scale with the casting time
            if (RuneMagic.PlayerStats.IsCasting)
            {
                var castingTime = Spell.CastingTime;
                if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Scribe) && this is Scroll)
                    castingTime *= 0.5f;
                var castbarWidth = (int)(RuneMagic.PlayerStats.CastingTimer / (castingTime * 60) * 64);
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 64, castbarWidth, 5), Color.DarkBlue);
            }
        }
        public override int maximumStackSize()
        {
            return 10;
        }
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            DrawCastbar(spriteBatch, location, Game1.player);
        }

    }
}
