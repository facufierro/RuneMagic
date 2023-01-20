using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class Fireball : Spell
    {
        public Fireball()
        {
            Type = SpellType.Active;
            School = MagicSchool.Evocation;
        }

        public override bool Cast()
        {
            throw new NotImplementedException();
        }
    }
}
