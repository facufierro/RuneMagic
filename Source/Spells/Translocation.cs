﻿using StardewValley;
using System.Linq;

namespace RuneMagic.Source.Spells
{
    public class Translocation : Spell
    {
        public Translocation() : base(School.Abjuration)
        {
            Description += "The caster changes position with a target living creature.";
            Level = 4;
        }

        public override bool Cast()
        {
            var target = Game1.currentLocation.characters.FirstOrDefault(c => c.getTileLocation() == Game1.currentCursorTile);
            if (target != null)
            {
                (Game1.player.Position, target.Position) = (target.Position, Game1.player.Position);
                return base.Cast();
            }
            else
            {
                return false;
            }
        }
    }
}