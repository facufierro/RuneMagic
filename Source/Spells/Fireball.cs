using SpaceCore;
using StardewValley;

namespace RuneMagic.Source.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base(School.Evocation)
        {
            Description += "Shoots a fireball";
            Level = 4;
        }
    }
}