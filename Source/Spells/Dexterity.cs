﻿using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Dexterity : Spell
    {
        public Dexterity()
        {
            School = School.Enchantment;
            Description = "Increases the caster's casting speed.";
            Level = 1;
        }

        public override bool Cast()
        {
            Target = Game1.player;

            return false;
        }
    }
}