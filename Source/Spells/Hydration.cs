using RuneMagic.Source.Interfaces;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace RuneMagic.Source.Spells
{
    public class Hydration : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }

        public Hydration() : base()
        {
            Name = "Hydration";
            School = School.Conjuration;
            Description = "Water a tile at the cursor.";
            Level = 1;
        }

        public bool Cast()
        {
            var Cursor = Game1.currentCursorTile;
            if (Game1.currentLocation.terrainFeatures.ContainsKey(Cursor) && Game1.currentLocation.terrainFeatures[Cursor] is HoeDirt)
            {
                HoeDirt dirt = (HoeDirt)Game1.currentLocation.terrainFeatures[Cursor];
                dirt.state.Value = HoeDirt.watered;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}