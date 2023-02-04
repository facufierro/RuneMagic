using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Blasting : ISpell
    {
        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }

        public Blasting() : base()
        {
            Name = "Blasting";
            School = School.Evocation;
            Description = "Creates an explosion at a target location.";
            Level = 1;
            CastingTime = 2;
        }
        public bool Cast()
        {

            var cursorLocation = Game1.currentCursorTile;
            Game1.currentLocation.explode(cursorLocation, 1, Game1.player);

            return true;
        }
    }
}
