using RuneMagic.assets.Framework;
using RuneMagic.assets.Items;

using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Framework.Spells
{
    public class MagicMissile : Spell
    {
        public int ProjectileNumber { get; set; }

        public MagicMissile() : base()
        {

            Name = "Magic Missile";
            School = School.Evocation;
            Description = "Shoots a magic missile";
            Glyph = "assets/Textures/Alteration/MagicMissile.png";
            ProjectileNumber = 1;

        }
        public override bool Cast()
        {
            for (int i = 0; i < ProjectileNumber; i++)
                Game1.currentLocation.projectiles.Add(new SpellProjectile(Game1.player, "magic_missile", 1, 4, 1, 8, 400, 3, true, ""));
            Game1.playSound("wand");
            return true;
        }
    }
}
