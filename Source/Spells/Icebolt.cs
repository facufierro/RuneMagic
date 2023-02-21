using RuneMagic.Source.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Icebolt : Spell
    {
        public Icebolt() : base(School.Evocation)
        {
            Description += "Shoots bolt of ice to the target.";
            Level = 2;
        }
    }
}