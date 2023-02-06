using RuneMagic.Source.Interfaces;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Translocation : ISpell
    {
        public Translocation() : base()
        {
            Name = "Translocation";
            School = School.Evocation;
            Description = "The caster changes position with a target living creature.";
            Level = 2;
            CastingTime = 2;
        }

        public float CastingTime { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public School School { get; set; }
        public ISpellEffect Effect { get; set; }

        public bool Cast()
        {
            var cursorLocation = Game1.currentCursorTile;
            var target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == cursorLocation);
            if (target != null)
            {
                var temp = target.Position;
                target.Position = Game1.player.Position;
                Game1.player.Position = temp;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}