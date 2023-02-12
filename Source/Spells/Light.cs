using RuneMagic.Source.Interfaces;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Light : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public Buff Buff { get; set; }
        public void Update() { }

        public Light()
        {
            Name = "Light";
            School = School.Conjuration;
            Description = "Conjures a torch at a target location.";
            CastingTime = 1;
            Level = 3;
        }
        public bool Cast()
        {
            //get cursorlocation
            var cursorLocation = Game1.currentCursorTile;
            //place a Light object at cursorlocation
            var light = new SpellEffects.LightEffect(cursorLocation, 0, false);
            Game1.currentLocation.objects.Add(cursorLocation, light);






            return true;
        }
    }
}
