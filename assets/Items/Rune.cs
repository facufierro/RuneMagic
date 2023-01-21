using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using System.Threading;
using System.Reflection;
using System.Xml.Linq;
using System.Text;
using RuneMagic.assets.Spells;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Netcode;
using StardewValley.Network;
using StardewValley.Menus;

namespace RuneMagic.assets.Items
{
    [XmlType("Mods_Rune")]
    public class Rune : Object
    {

        public int MaxCharges { get; set; }
        public int CurrentCharges { get; set; }
        public Spell Spell { get; set; }




        public Rune() : base()
        {

        }

        public Rune(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {
            MaxCharges = 5;
            CurrentCharges = MaxCharges;
            InitializeSpell();
        }


        //override the drawinmenu method to add background to the rune using the same parameters as the drawinmenu JUST CHANGE THE TEXTURE
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            Texture2D RuneTexture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>("assets/Textures/Runes/rune_overlay.png");
            //set layerdepth to draw behind the item

            spriteBatch.Draw(RuneTexture, location + new Vector2(32f, 32f), new Rectangle?(Game1.getSourceRectForStandardTileSheet(RuneTexture, 0, 16, 16)),
                Color.BlueViolet * transparency, 0.0f, new Vector2(8f, 8f), 4f * scaleSize, SpriteEffects.None, layerDepth);
            spriteBatch.DrawString(Game1.tinyFont, CurrentCharges.ToString(), new Vector2(location.X + 64 - Game1.smallFont.MeasureString(CurrentCharges.ToString()).X, location.Y),
                           Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
        }












        public void Activate()
        {
            if (CurrentCharges > 0 && Spell != null)
            {
                if (Spell.Cast())
                {
                    CurrentCharges--;
                }
            }
        }


        public void InitializeSpell()
        {
            string spellName = Name.Substring(8);
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType("RuneMagic.assets.Spells." + spellName);
            Spell = (Spell)Activator.CreateInstance(spellType);
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

