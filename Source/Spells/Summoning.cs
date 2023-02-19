using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Summoning : Spell
    {
        public Summoning() : base(School.Conjuration)
        {
            Description += "Summons a creature to fight for you.";
            Level = 3;
        }
    }
}