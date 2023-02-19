using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Shielding : Spell
    {
        public Shielding() : base(School.Abjuration)
        {
            Description += "Rises the caster's defense.";
            Level = 1;
        }
    }
}