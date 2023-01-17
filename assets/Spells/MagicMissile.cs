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

            Game1.currentLocation.projectiles.Add(new SpellProjectile(Player, 1, 4, 1, 5, 400, true));
        }
    }
}
