using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Transmutation : Spell
    {
        public Transmutation() : base(School.Alteration)
        {
            Description += "Strips an item of its quality and gives the caster some money back.";
            Level = 1;
        }

        public override bool Cast()
        {
            Game1.warpFarmer("FarmHouse", 4, 3, false);
            return base.Cast();
        }
    }
}