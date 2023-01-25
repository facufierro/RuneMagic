using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.Magic;

namespace RuneMagic.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object
    {
        public Spell Spell { get; set; }
        public int MaxCharges { get; set; }
        public float CurrentCharges { get; set; }
        public float CurrentCooldown { get; set; }
        public float MaxCooldown { get; set; }

        public Rune() : base()
        {
            MaxCharges = 5;
            CurrentCharges = MaxCharges;
            MaxCooldown = 1;
            CurrentCooldown = 0;

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
                    CurrentCharges -= 1;
                    CurrentCooldown = MaxCooldown;
                }


            }

        }
        public void InitializeSpell()
        {
            string spellName = Name[8..];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);
        }

        public void UpdateCharges()
        {
            if (CurrentCharges < MaxCharges)
            {
                CurrentCharges += 0.0005f;
            }
        }
        public void UpdateCooldown()
        {

            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= 0.01f;
            }
        }

        private void DrawCharges(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            var charges = Math.Floor(CurrentCharges).ToString();
            spriteBatch.DrawString(Game1.tinyFont, charges, new Vector2(location.X + 64 - Game1.smallFont.MeasureString(charges).X, location.Y),
                           Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);

            DrawCharges(spriteBatch, location, layerDepth);
            //draw a clock icon on the item
            //spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(64f, 64f),
            //    new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 269, 16, 16)),
            //    Color.White * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth + 0.0001f);
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
//*************************
//private void DrawRune(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float layerDepth)
//{
//    var transparency = (float)CurrentCharges / (float)MaxCharges;
//    spriteBatch.Draw(Glyph.Value, location + new Vector2(32f, 32f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Glyph.Value, 0, 16, 16)),
//      Spell.GetColor() * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
//}
