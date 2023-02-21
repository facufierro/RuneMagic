using RuneMagic.Source.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source.Spells
{
    public class Shielding : Spell
    {
        public Shielding() : base(School.Abjuration)
        {
            Description += "Rises the caster's defense.";
            Level = 1;
        }

        public override bool Cast()
        {
            if (!RuneMagic.PlayerStats.ActiveEffects.OfType<Shielded>().Any())
            {
                Effect = new Shielded(this);
                return base.Cast();
            }
            else
                return false;
        }
    }
}