﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Reflection;
using RuneMagic.Framework;
using SpaceCore;
using RuneMagic.Skills;
using System.Threading;
using StardewModdingAPI;
using RuneMagic.Famework;
using RuneMagic.Source;

namespace RuneMagic.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object, IMagicItem
    {

        public int ChargesMax { get; set; }
        public float Charges { get; set; }
        public Spell Spell { get; set; }

        public Rune() : base()
        {
            ChargesMax = 5;
            Charges = ChargesMax;
            InitializeSpell();
        }
        public Rune(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            ChargesMax = 5;
            Charges = ChargesMax;
            InitializeSpell();

        }

        public void InitializeSpell()
        {

            string spellName = Name[8..];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);

        }
        public void Activate()
        {
            if (Charges > 0 && Spell != null)
            {

            }
        }
        public virtual void Use()
        {
            ModEntry.RuneMagic.PlayerStats.ItemHeld = this;
        }
        public bool Fizzle()
        {

            if (Game1.random.Next(1, 100) < 0)
            {
                Game1.player.stamina -= 10;
                Game1.playSound("stoneCrack");
                Game1.player.removeItemFromInventory(this);
                Game1.player.addItemToInventory(new Object(390, 1));
                return true;
            }
            else
                return false;
        }
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            var castingTimer = ModEntry.RuneMagic.PlayerStats.CastingTimer;
            if (castingTimer > 0)
            {
                var castingTimerMax = Spell.CastingTime * 60;
                var castingTimerPercent = castingTimer / castingTimerMax;
                var barWidth = 60;
                var barHeight = 6;
                var castingTimerWidth = barWidth * castingTimerPercent;
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 160, (int)castingTimerWidth, barHeight), Color.DarkBlue);
            }
        }
        public void UpdateCharges()
        {
            if (Charges < ChargesMax)
            {
                Charges += 0.0005f;
            }
        }
        public void DrawCharges(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            spriteBatch.DrawString(Game1.tinyFont, Math.Floor(Charges).ToString(), new Vector2(location.X + 64 - Game1.smallFont.MeasureString(Math.Floor(Charges).ToString()).X, location.Y),
                           Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        }
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);

            DrawCharges(spriteBatch, location, layerDepth);
        }
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
            DrawCastbar(spriteBatch, objectPosition, f);
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
