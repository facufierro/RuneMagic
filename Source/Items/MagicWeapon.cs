using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Interfaces;
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
    public class MagicWeapon : MeleeWeapon, IMagicItem
    {
        [XmlIgnore]
        public ISpell Spell { get; set; }
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
            List<ISpell> apprenticeSpells = new List<ISpell>();
            List<ISpell> adeptSpells = new List<ISpell>();
            List<ISpell> masterSpells = new List<ISpell>();
            foreach (var spell in RuneMagic.Spells)
            {
                if (spell.Level >= 1 && spell.Level <= 2)
                    apprenticeSpells.Add(spell);
                if (spell.Level >= 3 && spell.Level <= 4)
                    apprenticeSpells.Add(spell);
                if (spell.Level >= 5 && spell.Level <= 6)
                    apprenticeSpells.Add(spell);

            }
            if (Name.Contains("Apprentice"))
            {
                Spell = apprenticeSpells[new Random().Next(apprenticeSpells.Count)];
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
            if (Name.Contains("Adept"))
            {
                Spell = adeptSpells[new Random().Next(apprenticeSpells.Count)];
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
            if (Name.Contains("Master"))
            {
                Spell = masterSpells[new Random().Next(apprenticeSpells.Count)];
                description += $" Looks like it contains the {Spell.Name} spell.";
            }
        }
        public void Activate()
        {
            if (Spell.Cast() && Charges > 0)
            {
                Game1.player.AddCustomSkillExperience(RuneMagic.PlayerStats.MagicSkill, 5);
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
            if (RuneMagic.PlayerStats.IsCasting && Game1.player.CurrentItem == this)
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
    }
}
