using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Stone : Spell
    {
        public Stone() : base(School.Conjuration)
        {
            Description += "Conjures a big stone at the target location.";
            Level = 4;
        }
    }
}