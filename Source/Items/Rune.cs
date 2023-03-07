using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object, ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

        public int ChargesMax { get; set; }
        public float Charges { get; set; }

        public Rune() : base()
        {
            InitializeSpell();
            ChargesMax = Game1.random.Next(3, 10);
            Charges = ChargesMax;
        }

        public Rune(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            InitializeSpell();
            ChargesMax = Game1.random.Next(3, 10);
            Charges = ChargesMax;
        }

        public void InitializeSpell()
        {
            foreach (var spell in RuneMagic.Spells)
            {
                if (Name.Contains(spell.Name))
                {
                    Spell = spell;
                    RuneMagic.Instance.Monitor.Log($"{Name} Initialized", LogLevel.Debug);
                    break;
                }
            }
        }

        public void Activate()
        {
            if (!Fizzle())
                if (Math.Floor(Charges) > 0)
                {
                    if (Spell.Cast())
                    {
                        Game1.playSound("flameSpell");
                        Charges--;
                    }
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
            //Charges
            if (Charges < ChargesMax)
            {
                Charges += 0.0005f;
            }
            if (Charges > ChargesMax)
                Charges = ChargesMax;
            if (Charges < 0)
                Charges = 0;
        }

        public void DrawCharges(SpriteBatch spriteBatch, Vector2 location, float layerDepth)
        {
            spriteBatch.DrawString(Game1.tinyFont, Math.Floor(Charges).ToString(), new Vector2(location.X + 64 - Game1.tinyFont.MeasureString(Math.Floor(Charges).ToString()).X, location.Y + 64 - Game1.tinyFont.MeasureString(Math.Floor(Charges).ToString()).Y),
                           Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (RuneMagic.PlayerStats.CastingTime > 0)
                base.drawWhenHeld(spriteBatch, objectPosition, f);
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