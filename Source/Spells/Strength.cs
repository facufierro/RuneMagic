using StardewValley;

namespace RuneMagic.Source.NotImplementedSpells
{
    public class Strength : Spell
    {
        public Strength()
        {
            School = School.Enchantment;
            Description = "Increases the caster's attack damage.";
            Level = 1;
        }

        public override bool Cast()
        {
            //if (!Game1.buffsDisplay.hasBuff(Id))
            //{
            //    //Buff = new Buff(0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 5, Duration, "Glyph of Haste", "Glyph of Haste") { which = Id, description = Description, millisecondsDuration = Duration * 1000 };
            //    //Game1.buffsDisplay.addOtherBuff(Buff);
            //    Game1.player.changeFriendship(250, Target as NPC);
            //    return true;
            //}
            //else
            //{
            return false;
            //}
        }

        //public override void Update()
        //{
        //    base.Update();
        //}
    }
}