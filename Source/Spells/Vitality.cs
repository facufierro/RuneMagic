﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Vitality : Spell
    {
        public Vitality() : base()
        {
            School = School.Enchantment;
            Description = "Busts the caster's stamina.";
            Level = 8;
        }
    }
}