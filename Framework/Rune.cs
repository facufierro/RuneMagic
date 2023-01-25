﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.Magic;
using RuneMagic.Framework;

namespace RuneMagic.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : MagicItem
    {

        public int ChargesMax { get; set; }
        public float Charges { get; set; }


        public Rune()
           : base()
        {
            ChargesMax = 5;
            Charges = ChargesMax;
        }
        public Rune(int parentSheetIndex, int stack)
            : base(parentSheetIndex, stack)
        {
            ChargesMax = 5;
            Charges = ChargesMax;

            InitializeSpell();
        }

        public override void Use()
        {
            if (Charges > 0 && GlobalCooldown <= 0 && Spell != null)
            {
                if (Spell.Cast())
                {
                    Charges -= 1;
                    GlobalCooldown = GlobalCooldownMax;
                }


            }

        }


        public override void InitializeSpell()
        {

            string spellName = Name[8..];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);

        }
        public void UpdateCharges()
        {
            if (Charges < ChargesMax)
            {
                Charges += 0.0005f;
            }
        }


        private void DrawCharges(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            var charges = Math.Floor(Charges).ToString();
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
//    var transparency = (float)Charges / (float)ChargesMax;
//    spriteBatch.Draw(Glyph.Value, location + new Vector2(32f, 32f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Glyph.Value, 0, 16, 16)),
//      Spell.GetColor() * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
//}