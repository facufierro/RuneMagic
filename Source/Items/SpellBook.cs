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
    [XmlType("Mods_SpellBook")]
    public class SpellBook : Object, ISpellCastingItem
    {
        [XmlIgnore]
        public Spell Spell { get; set; }

        public SpellBook() : base()
        {
            InitializeSpell();
        }

        public SpellBook(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            InitializeSpell();
        }

        public void InitializeSpell()
        {
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
            if (Spell != null && Spell.Cast())
            {
                Game1.playSound("flameSpell");
            }
        }

        public void Update()
        {
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