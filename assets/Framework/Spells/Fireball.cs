using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuneMagic.assets.Framework;

namespace RuneMagic.assets.Framework.Spells
{
    public class Fireball : Spell
    {
        public Fireball() : base()
        {
            Name = "Fireball";
            School = School.Evocation;
            Description = "Shoots a fireball.";
            Glyph = "assets/Textures/Alteration/Fireball.png";

        }

        public override bool Cast()
        {
            throw new NotImplementedException();
        }
    }
}
