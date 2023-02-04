using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source
{
    internal interface ISpellEffect
    {
        public int Duration { get; set; }
        public int Timer { get; set; }
        public int Cooldown { get; set; }


    }
}
