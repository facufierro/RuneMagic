using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewModdingAPI;
using StardewValley;
using System.Xml.Serialization;
using Object = StardewValley.Object;

namespace RuneMagic.Source.Items
{
    [XmlType("Mods_Scroll")]
    public class Scroll : Object, ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

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

            foreach (var spell in RuneMagic.Spells)
            {
                if (Name.Contains(spell.Name))
                {
                    Spell = spell;
                    break;
                }
            }
        }

        public void Activate()
        {
            if (Spell.Cast())
            {
                Game1.playSound("flameSpell");
                Stack--;
                if (Stack <= 0)
                    Game1.player.removeItemFromInventory(this);
            }
        }

        public void Update()
        {
        }

        public override int maximumStackSize()
        {
            return 10;
        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (RuneMagic.PlayerStats.CastingTime > 0)
                base.drawWhenHeld(spriteBatch, objectPosition, f);
        }
    }
}