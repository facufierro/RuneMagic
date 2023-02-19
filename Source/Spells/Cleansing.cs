using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Cleansing : Spell
    {
        public Cleansing() : base(School.Abjuration)

        {
            Description += "Removes harmful effects from the player";
            Level = 2;
        }
    }
}