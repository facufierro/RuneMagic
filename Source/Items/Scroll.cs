

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Source.Interfaces;
using RuneMagic.Source.Skills;
using SpaceCore;
using StardewValley;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source.Items
{
    [XmlType("Mods_Scroll")]
    public class Scroll : Object, IMagicItem
    {
        [XmlIgnore]
        public ISpell Spell { get; set; }
        public float Charges { get; set; }
        public Scroll() : base()
        {
            InitializeSpell();
        }
        public Scroll(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            InitializeSpell();

        }

        public void InitializeSpell()
        {
            //set spellName to Name without " Scroll" at the end

            string spellName = Name[0..^7];
            spellName = spellName.Replace(" ", "");
            foreach (var spell in RuneMagic.Spells)
            {
                if (spell.Name == spellName)
                {

                    Spell = spell;
                    if (Game1.player.HasCustomProfession(MagicSkill.Scribe))
                        Spell.CastingTime *= 0.8f;
                    break;
                }
            }

        }
        public void Activate()
        {
            if (Spell.Cast())
            {
                //remove an item stack from this object if Farmer doesnt have Lorekeeper profession and if it does give it a 20% chance of not consuming the scroll
                if (!Game1.player.HasCustomProfession(MagicSkill.Lorekeeper) || Game1.random.Next(1, 100) > 20)
                    Stack--;
                if (Stack <= 0)
                    Game1.player.removeItemFromInventory(this);

            }
        }
        public void Update()
        {

        }
        public bool Fizzle()
        {
            return false;
        }
        public void DrawCastbar(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            //draw a castbar on the item if isCasting is true taking into account that if player has Scribe profession the castbar is 50% shorter
            if (RuneMagic.PlayerStats.IsCasting)
            {
                if (RuneMagic.PlayerStats.IsCasting && Game1.player.CurrentItem == this)
                {
                    var castingTime = Spell.CastingTime;
                    var castbarWidth = (int)(RuneMagic.PlayerStats.CastingTimer / (castingTime * 60) * 64);
                    spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 64, castbarWidth, 5), Color.DarkBlue);
                }

            }


        }
        public override int maximumStackSize()
        {
            return 10;
        }
        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            DrawCastbar(spriteBatch, location, Game1.player);
        }

    }
}
