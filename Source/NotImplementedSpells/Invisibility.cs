using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.NotImplementedSpells
{
    public class Invisibility : ISpell
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public School School { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float CastingTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ProjectileNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Cast()
        {
            throw new NotImplementedException();
        }
    }
}
