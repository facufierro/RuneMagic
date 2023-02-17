using RuneMagic.Source.Effects;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Light : Spell
    {
        public Light() : base()
        {
            School = School.Conjuration;
            Description = "Conjures a torch at a target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            var target = Game1.currentCursorTile;
            if (Game1.currentLocation.objects.ContainsKey(target))
                return false;
            Effect = new Lighted(Name, target);
            return true;
        }
    }
}