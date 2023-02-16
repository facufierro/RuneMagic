using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Smiting : Spell
    {
        public Smiting()
        {
            School = School.Abjuration;
            Description = "Has a chance to destroy undead on the range of vision of the caster.";
            Level = 6;
        }
    }
}