using RuneMagic.Source.Interfaces;
using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Translocation : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public Translocation() : base()
        {
            Name = "Translocation";
            School = School.Evocation;
            Description = "The caster changes position with a target living creature.";
            Level = 2;
            CastingTime = 2;
        }

        public bool Cast()
        {
            var cursorLocation = Game1.currentCursorTile;
            var target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == cursorLocation);
            if (target != null)
            {
                (Game1.player.Position, target.Position) = (target.Position, Game1.player.Position);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update()
        { }
    }
}