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
    public class Scroll : MagicItem
    {
        public Scroll() : base()
        {
            InitializeSpell();
        }
        public Scroll(int parentSheetIndex, int stack) : base(parentSheetIndex, stack)
        {
            InitializeSpell();

        }
        public override void Activate()
        {
            if (Spell != null)
            {
                if (Spell.Cast())
                {
                    ModEntry.RuneMagic.Farmer.AddCustomSkillExperience(ModEntry.RuneMagic.PlayerStats.MagicSkill, 5);
                    Game1.player.removeItemFromInventory(this);
                }
            }
        }
        public override void Use()
        {
            ModEntry.RuneMagic.PlayerStats.ItemHeld = this;
        }
        public override bool Fizzle()
        {

            if (Game1.random.Next(1, 100) < 0)
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
