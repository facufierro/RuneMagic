using RuneMagic.Source.Interfaces;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Haste : Spell
    {
        public Haste() : base()
        {
            Name = "Haste";
            School = School.Enchantment;
            Description = "Increases the caster's movement speed.";
            Level = 3;
        }

        public override bool Cast()
        {
            if (!Game1.buffsDisplay.hasBuff(Id))
            {
                Buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 10, "Glyph of Haste", "Glyph of Haste") { which = Id, description = Description, millisecondsDuration = Duration * 1000 };
                Game1.buffsDisplay.addOtherBuff(Buff);
                Game1.player.changeFriendship(250, Target as NPC);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update()
        { }
    }
}