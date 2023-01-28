using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Famework;
using RuneMagic.Source;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Object = StardewValley.Object;

namespace RuneMagic.Framework
{
    public class Scroll : Object, IMagicItem
    {
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

            string spellName = Name[0..^7];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);

        }
        public void Use()
        {
            ModEntry.RuneMagic.PlayerStats.ItemHeld = this;
        }
        public void Activate()
        {
            if (!Fizzle())
                if (Spell.Cast())
                {
                    ModEntry.RuneMagic.Farmer.AddCustomSkillExperience(ModEntry.RuneMagic.PlayerStats.MagicSkill, 5);
                }

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
            var castingTimer = ModEntry.RuneMagic.PlayerStats.CastingTimer;
            if (castingTimer > 0)
            {
                var castingTimerMax = Spell.CastingTime * 60;
                var castingTimerPercent = castingTimer / castingTimerMax;
                var barWidth = 60;
                var barHeight = 6;
                var castingTimerWidth = barWidth * castingTimerPercent;
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 160, (int)castingTimerWidth, barHeight), Color.DarkBlue);
            }
        }

        public override int maximumStackSize()
        {
            return 10;
        }
        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
            DrawCastbar(spriteBatch, objectPosition, f);
        }
    }
}
