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
            ProjectileNumber = 3;
        }
        public override void Cast()
        {


            for (int i = 0; i < ProjectileNumber; i++)
            {
                SpellProjectile projectile = new SpellProjectile(Player, 10, 3, 0.5f, true);
                Game1.currentLocation.projectiles.Add(projectile);
            }
        }
    }
}
