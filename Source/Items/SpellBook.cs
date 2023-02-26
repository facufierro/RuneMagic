using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Interface;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source.Items
{
    [XmlType("Mods_SpellBook")]
    public class SpellBook : Object, ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

        [XmlIgnore]
        public SpellSlot SelectedSlot { get; set; }

        public SpellBook() : base()
        {
        }

        public SpellBook(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
        }

        public void InitializeSpell()
        {
            if (SelectedSlot != null)
                Spell = SelectedSlot.Spell;
            else
                RuneMagic.Instance.Monitor.Log("No spell in Selected Slot");
        }

        public void Activate()
        {
            if (Spell != null && Spell.Cast())
            {
                Game1.playSound("flameSpell");
                RuneMagic.PlayerStats.MemorizedSpells.Remove(Spell);
                Spell = null;
            }
        }

        public void Update()
        {
        }

        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            //draw a castbar on the item if isCasting is true taking into account that if player has Scribe profession the castbar is 50% shorter
            if (RuneMagic.PlayerStats.CastingTime > 0)
            {
                if (RuneMagic.PlayerStats.CastingTime > 0 && Game1.player.CurrentItem == this && Spell != null)
                {
                    var castingTime = Spell.CastingTime;
                    var castbarWidth = (int)(RuneMagic.PlayerStats.CastingTime / (castingTime * 60) * 58);
                    spriteBatch.Draw(RuneMagic.Textures["castbar_frame"], new Rectangle((int)objectPosition.X, (int)objectPosition.Y, 64, 84), Color.White);
                    spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X + 3, (int)objectPosition.Y + 75, castbarWidth, 5), new Color(new Vector4(0, 0, 200, 0.8f)));
                }
            }
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            DrawCastbar(spriteBatch, location, Game1.player);
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