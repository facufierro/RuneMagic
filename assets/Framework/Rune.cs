using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.assets.Framework;

namespace RuneMagic.assets.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object
    {
        public Spell Spell { get; set; }
        public int MaxCharges { get; set; }
        public int CurrentCharges { get; set; }
        public int CurrentCooldown { get; set; }
        public int MaxCooldown { get; set; }
        public int RegenerationRate { get; set; }


        public Rune() : base()
        {

        }
        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            MaxCharges = 5;
            CurrentCharges = MaxCharges;
            MaxCooldown = 1;
            CurrentCooldown = 0;
            InitializeSpell();
        }

        public void Activate()
        {
            if (CurrentCharges > 0 && CurrentCooldown <= 0 && Spell != null)
            {
                if (Spell.Cast())
                {
                    CurrentCharges--;
                    CurrentCooldown = MaxCooldown * 100;
                }


            }

        }
        public void InitializeSpell()
        {
            string spellName = Name[8..];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.assets.Framework.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);


        }

        public void AddCharges(int amount)
        {
            if (CurrentCharges < MaxCharges)
            {
                CurrentCharges += amount;
            }
        }
        public void UpdateCooldown()
        {

            if (CurrentCooldown > 0)
            {
                CurrentCooldown--;
            }
        }

        //private void DrawRune(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float layerDepth)
        //{

        //    var transparency = (float)CurrentCharges / (float)MaxCharges;
        //    spriteBatch.Draw(Glyph.Value, location + new Vector2(32f, 32f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Glyph.Value, 0, 16, 16)),
        //      Spell.GetColor() * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
        //}
        //private void DrawCooldown(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        //{
        //    spriteBatch.DrawString(Game1.tinyFont, CurrentCharges.ToString(), new Vector2(location.X + 64 - Game1.smallFont.MeasureString(CurrentCharges.ToString()).X, location.Y),
        //                   Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        //}

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);

            //DrawCooldown(spriteBatch, location, layerDepth);
            //DrawRune(spriteBatch, location, scaleSize, layerDepth);

        }
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 location, Farmer f)
        {
            //base.drawWhenHeld(spriteBatch, location, f);
            //var layerDepth = (float)((f.getStandingY() + 2) / 10000.0 + (double)location.Y / 20000.0) + 0.0001f;
            //DrawRune(spriteBatch, location, 1, layerDepth);
            //DrawCooldown(spriteBatch, location, layerDepth);
        }
        public override bool canBeShipped()
        {
            return false;
        }
        public override bool canBeGivenAsGift()
        {
            return false;
        }
        public override int maximumStackSize()
        {
            return 1;
        }
    }
}

