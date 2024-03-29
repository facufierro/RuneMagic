﻿using RuneMagic.Source.Effects;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = StardewValley.Object;

namespace RuneMagic.Source.Spells
{
    public class Stone : Spell
    {
        public Stone() : base(School.Conjuration)
        {
            Description += "Conjures a big stone at the target location."; Level = 4;
        }

        //public override bool Cast()
        //{
        //    if (!RuneMagic.PlayerStats.ActiveEffects.OfType<ObjectSummoned>().Any())
        //    {
        //        Effect = new ObjectSummoned(this, new Object(Game1.currentCursorTile, RuneMagic.JsonAssetsApi.GetBigCraftableId("Stone")));
        //        return base.Cast();
        //    }
        //    else
        //        return false;
        //}
    }
}