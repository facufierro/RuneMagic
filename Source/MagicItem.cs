using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RuneMagic.Famework;
using RuneMagic.Magic;
using RuneMagic.Source;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using xTile;

namespace RuneMagic.Framework
{
    public abstract class MagicItem : StardewValley.Object
    {
        public float GlobalCooldown { get; set; }
        public float GlobalCooldownMax { get; set; }
        public Spell Spell { get; set; }


        public MagicItem()
        {

        }
        public MagicItem(int parentSheetIndex, int stack, bool isRecipe = false) : base(parentSheetIndex, stack, isRecipe)
        {

        }

        public abstract void InitializeSpell();
        public abstract void Activate();
        public abstract void Use();
        public abstract bool Fizzle();

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
            var castingTimer = ModEntry.RuneMagic.PlayerStats.CastingTimer;
            //draw casting timer as a bar on the rune
            if (castingTimer > 0)
            {
                var castingTimerMax = Spell.CastingTime * 60;
                var castingTimerPercent = castingTimer / castingTimerMax;
                var barWidth = 60;
                var barHeight = 6;
                var castingTimerWidth = barWidth * castingTimerPercent;


                ModEntry.Instance.Monitor.Log($"{castingTimer}/{castingTimerMax} = {castingTimerPercent}", LogLevel.Alert);
                spriteBatch.Draw(Game1.staminaRect, new Rectangle((int)objectPosition.X, (int)objectPosition.Y + 160, (int)castingTimerWidth, barHeight), Color.DarkBlue);
            }
        }

    }
}
