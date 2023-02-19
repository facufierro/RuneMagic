using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Storm : Spell
    {
        public Storm() : base(School.Evocation)
        {
            Description += "Summons a storm to the target location.";
            Level = 5;
        }
    }
}