﻿using StardewValley;
using xTile.Dimensions;
using static StardewValley.Menus.CharacterCustomization;

namespace RuneMagic.Source.Spells
{
    public class Light : Spell
    {
        public Light()
        {
            School = School.Conjuration;
            Description = "Conjures a torch at a target location.";
            Level = 3;
        }

        public override bool Cast()
        {
            //if (!Game1.buffsDisplay.hasBuff(Id))
            //{
            //    //Buff = new Buff(Id) { which = Id, millisecondsDuration = Duration * 1000, sheetIndex = 16, description = Description, source = $"Glyph of {Name}", displaySource = $"Glyph of {Name}" };
            //    //Game1.buffsDisplay.addOtherBuff(Buff);
            //    Target = Game1.currentCursorTile;
            //    Game1.currentLocation.objects.Add(Target, new Torch(Target, 1));
            //    return true;
            //}
            //else
            //{
            return false;
            //}
        }

        //public override void Update()
        //{
        //    //if (!Game1.buffsDisplay.hasBuff(Id) && Buff != null)
        //    //{
        //    //    Game1.currentLocation.objects.Remove(Target);
        //    //}
        //    //else
        //    //{
        //    //}
        //}
    }
}