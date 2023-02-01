﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewValley;
using System;
using System.Reflection;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source
{
    [XmlType("Mods_Rune")]
    public class Rune : Object, IMagicItem
    {
        public Spell Spell { get; set; }
        public int ChargesMax { get; set; }
        public float Charges { get; set; }

        public Rune() : base()
        {
            if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Runelord))
                ChargesMax = 10;
            else
                ChargesMax = 5;
            Charges = ChargesMax;
            InitializeSpell();
        }
        public Rune(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Runelord))
                ChargesMax = 10;
            else
                ChargesMax = 5;
            Charges = ChargesMax;
            InitializeSpell();

        }

        public void InitializeSpell()
        {

            string spellName = Name[8..];
            spellName = spellName.Replace(" ", "");
            foreach (var spell in RuneMagic.Spells)
            {
                if (spell.Name == spellName)
                {
                    Spell = spell;
                    break;
                }
            }
        }
        public virtual void Use()
        {
            RuneMagic.PlayerStats.ItemHeld = this;

        }
        public void Activate()
        {
            if (!Fizzle())
                if (Spell.Cast() && Charges > 0)
                {
                    RuneMagic.Farmer.AddCustomSkillExperience(RuneMagic.PlayerStats.MagicSkill, 5);
                    Charges--;
                }
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
        public void Update()
        {
            if (Charges < ChargesMax)
            {
                if (RuneMagic.Farmer.HasCustomProfession(MagicSkill.Runesmith))
                    Charges += 0.0010f;
                else
                    Charges += 0.0005f;
            }
            if (Charges > ChargesMax)
                Charges = ChargesMax;
            if (Charges < 0)
                Charges = 0;
        }
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (RuneMagic.PlayerStats.IsCasting && RuneMagic.PlayerStats.ItemHeld == this)
            {
                var castingTime = Spell.CastingTime;
                var castbarWidth = (int)(RuneMagic.PlayerStats.CastingTimer / (castingTime * 60) * 64);
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 64, castbarWidth, 5), Color.DarkBlue);
            }
        }
        public void DrawCharges(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            spriteBatch.DrawString(Game1.tinyFont, Math.Floor(Charges).ToString(), new Vector2(location.X + 64 - Game1.tinyFont.MeasureString(Math.Floor(Charges).ToString()).X, location.Y + 64 - Game1.tinyFont.MeasureString(Math.Floor(Charges).ToString()).Y),
                           Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            DrawCharges(spriteBatch, location, layerDepth);
            DrawCastbar(spriteBatch, location, Game1.player);


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
