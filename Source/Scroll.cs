using RuneMagic.Famework;
using RuneMagic.Source;
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
    public class Scroll : MagicItem
    {


        public Scroll()
          : base()
        {

        }
        public Scroll(int parentSheetIndex, int stack)
            : base(parentSheetIndex, stack)
        {
            InitializeSpell();
        }
        public override void Use()
        {
            if (GlobalCooldown <= 0 && Spell != null)
            {
                if (Spell.Cast())
                {
                    Game1.player.removeItemFromInventory(ParentSheetIndex);
                    GlobalCooldown = GlobalCooldownMax;
                }
            }
        }
        public override bool Fizzle()
        {
            int castFailure = Convert.ToInt32(Game1.player.modData[$"{ModEntry.Instance.ModManifest.UniqueID}/CastingFailureChance"]);
            if (Game1.random.Next(1, 100) < 10 - castFailure)
            {
                Game1.player.stamina -= 10;
                Game1.playSound("Wand");
                Game1.player.removeItemFromInventory(this);
                return true;
            }
            else
                return false;
        }

        public override void InitializeSpell()
        {
            //remove " Scroll" from the name
            string spellName = Name[0..^7];
            spellName = spellName.Replace(" ", "");
            Type spellType = Assembly.GetExecutingAssembly().GetType($"RuneMagic.Spells.{spellName}");
            Spell = (Spell)Activator.CreateInstance(spellType);
        }
        public override int maximumStackSize()
        {
            return 10;
        }
    }
}
