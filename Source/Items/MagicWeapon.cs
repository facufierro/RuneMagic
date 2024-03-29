﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Spells;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RuneMagic.Source.Items
{
    [XmlType("Mods_MagicWeapon")]
    public class MagicWeapon : MeleeWeapon, ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

        public int ChargesMax { get; set; }
        public float Charges { get; set; }

        public MagicWeapon() : base()
        {
            ChargesMax = 20;
            Charges = ChargesMax;
            InitializeSpell();
        }

        public MagicWeapon(int parentSheetIndex) : base(parentSheetIndex)
        {
            ChargesMax = 20;
            Charges = ChargesMax;
            InitializeSpell();
        }

        public void InitializeSpell()
        {
            if (Name is null)
                return;
            if (Name.Contains("Apprentice"))
            {
                Spell = new MagicMissile();
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
            if (Name.Contains("Adept"))
            {
                Spell = new Blasting();
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
            if (Name.Contains("Master"))
            {
                Spell = new Fireball();
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
        }

        public void Activate()
        {
            if (Spell.Cast() && Charges > 0)
            {
                Game1.playSound("flameSpell");
                Charges--;
            }
        }

        public bool Fizzle()
        { return false; }

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

        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            //draw a castbar on the item if isCasting is true taking into account that if player has Scribe profession the castbar is 50% shorter
            if (Player.MagicStats.CastingTime > 0)
            {
                if (Player.MagicStats.CastingTime > 0 && Game1.player.CurrentItem == this)
                {
                    var castingTime = Spell.CastingTime;
                    var castbarWidth = (int)(Player.MagicStats.CastingTime / (castingTime * 60) * 58);

                    spriteBatch.Draw(RuneMagic.Textures["castbar_frame"], new Rectangle((int)objectPosition.X, (int)objectPosition.Y, 64, 84), Color.White);
                    spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X + 3, (int)objectPosition.Y + 75, castbarWidth, 5), new Color(new Vector4(0, 0, 200, 0.8f)));
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