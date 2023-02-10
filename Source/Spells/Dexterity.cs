using RuneMagic.Source.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Dexterity : ISpell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public School School { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get; set; }

        public Dexterity()
        {
            Name = "Dexterity";
            Description = "Improves the casting speed of the caster.";
            School = School.Enchantment;
            CastingTime = 1;
            Level = 3;
           


        }
        public bool Cast()
        {
            return false;
        }
    }
}
