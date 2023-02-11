﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Interfaces
{
    public interface ISpellEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public abstract void Update();
        public abstract void Start();
    }
}
