using RuneMagic.Source.Interfaces;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace RuneMagic.Source.Spells
{
    public class Labor : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }
        public void Update() { }

        public Labor() : base()
        {
            Name = "Labor";
            School = School.Conjuration;
            Description = "Creates a hoe dirt tile at the cursor.";
            Level = 2;
        }
        public bool Cast()
        {
            var Cursor = Game1.currentCursorTile;

            if (Game1.currentLocation.isTileLocationTotallyClearAndPlaceable(Cursor))
            {
                Game1.currentLocation.terrainFeatures.Add(Cursor, new HoeDirt());
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
