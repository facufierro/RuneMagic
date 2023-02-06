using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewValley;
using System.Reflection;
using System.Xml.Serialization;
using Object = StardewValley.Object;
using System.Threading.Tasks;
using RuneMagic.Source.Interfaces;

namespace RuneMagic.Source.Spells
{
    public class Light : ISpell
    {


        public string Name { get; set; }
        public School School { get; set; }
        public string Description { get; set; }
        public float CastingTime { get; set; }
        public int Level { get; set; }
        public ISpellEffect Effect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Light()
        {
            Name = "Light";
            School = School.Conjuration;
            Description = "Conjures a torch at a target location.";
            CastingTime = 1;
            Level = 2;
        }
        public bool Cast()
        {
            //get cursorlocation
            var cursorLocation = Game1.currentCursorTile;
            //place a Light object at cursorlocation
            var light = new SpellEffects.LightEffect(cursorLocation, 0, false);
            Game1.currentLocation.objects.Add(cursorLocation, light);






            return true;
        }
    }
}
