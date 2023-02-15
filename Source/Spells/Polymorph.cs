using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Polymorph : Spell
    {
        public Polymorph() : base()
        {
            School = School.Alteration;
            Description = "Transforms a target into a different creature.";
            Level = 8;
        }
    }
}