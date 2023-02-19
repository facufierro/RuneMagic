using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Banishing : Spell
    {
        public Banishing() : base(School.Abjuration)
        {
            Description += "Destroys a target Monster.";
            Level = 5;
        }
    }
}