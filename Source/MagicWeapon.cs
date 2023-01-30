using Microsoft.Xna.Framework;
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
using StardewValley.Tools;

namespace RuneMagic.Source
{
    [XmlType("Mods_MagicWeapon")]
    public class MagicWeapon : MeleeWeapon, IMagicItem
    {
        public Spell Spell { get; set; }
        public int ChargesMax { get; set; }
        public float Charges { get; set; }

        public MagicWeapon()
        {

        }
        public MagicWeapon(int parentSheetIndex) : base(parentSheetIndex)
        {
            ChargesMax = 20;
            Charges = ChargesMax;
            InitializeSpell();
        }

        public void InitializeSpell()
        {
            Spell = new Spells.MagicMissile();
        }
        public void Use()
        {
            ModEntry.RuneMagic.PlayerStats.ItemHeld = this;
        }
        public void Activate()
        {
            if (!Fizzle())
                if (Spell.Cast() && Charges > 0)
                {
                    ModEntry.RuneMagic.Farmer.AddCustomSkillExperience(ModEntry.RuneMagic.PlayerStats.MagicSkill, 5);
                    Charges--;
                }

        }
        public void Update()
        {
            if (Charges < ChargesMax)
            {
                Charges += 0.0010f;
            }
            if (Charges > ChargesMax)
                Charges = ChargesMax;
            if (Charges < 0)
                Charges = 0;
        }
        public bool Fizzle()
        {

            if (Game1.random.Next(1, 100) < 0)
            {
                Game1.player.stamina -= 10;
                Game1.playSound("stoneCrack");
                Game1.player.removeItemFromInventory((Item)this);
                Game1.player.addItemToInventory(new Object(390, 1));
                return true;
            }
            else
                return false;
        }
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (f.CurrentItem == this)
            {
                var castingTimer = ModEntry.RuneMagic.PlayerStats.CastingTimer;
                if (castingTimer > 0)
                {
                    var castingTimerMax = Spell.CastingTime * 60;
                    var castingTimerPercent = castingTimer / castingTimerMax;
                    var barWidth = 60;
                    var barHeight = 6;
                    var castingTimerWidth = barWidth * castingTimerPercent;
                    spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X + ((64 - barWidth) / 2), (int)objectPosition.Y + 64 - barHeight, (int)castingTimerWidth, barHeight), Color.DarkBlue);
                }
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
    }
}
