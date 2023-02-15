using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Healing : Spell
    {
        public Healing() : base()
        {
            School = School.Abjuration;
            Description = "";
            Level = 2;
        }
    }
}