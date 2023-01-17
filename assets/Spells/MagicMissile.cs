using RuneMagic.assets.Items;
using RuneMagic.assets.Spells.Effects;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.assets.Spells
{
    public class MagicMissile : Spell
    {
        public int ProjectileNumber { get; set; }

        public MagicMissile()
        {
            Type = SpellType.Active;
            School = MagicSchool.Evocation;
            ProjectileNumber = 3;

        }
        public override void Cast()
        {

            SpellProjectile projectile1 = new SpellProjectile(Player, 5, 3, 0f, false);
            Game1.currentLocation.projectiles.Add(projectile1);


        }
    }
}
