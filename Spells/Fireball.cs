using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuneMagic.Famework;
using RuneMagic.Magic;

namespace RuneMagic.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base()
        {
            Name = "Fireball";
            School = School.Evocation;
            Description = "Shoots a fireball.";
            Level = 1;





        }

        public override bool Cast()
        {
            throw new NotImplementedException();
        }
    }
}
