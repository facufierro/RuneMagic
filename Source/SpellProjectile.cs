using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source
{
    public class SpellProjectile : Projectile
    {
        public Farmer Source { get; set; }
        public Spell Spell { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int BonusDamage { get; set; }
        public float Direction { get; set; }
        public float Velocity { get; set; }
        public Texture2D Texture { get; set; }
        public bool Homing { get; set; }

        public SpellProjectile(Texture2D texture, int minDamage, int maxDamage, int bonusDamage, int velocity, bool homing)
        {
            Source = Game1.player;
            Homing = homing;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            BonusDamage = bonusDamage;
            Texture = texture;

            theOneWhoFiredMe.Set(Source.currentLocation, Source);
            damagesMonsters.Value = true;

            position.Value = Source.getStandingPosition();
            position.X -= Source.GetBoundingBox().Width / 1.5f;
            position.Y -= (Source.GetBoundingBox().Height);

            Velocity = velocity;
            Direction = (float)Math.Atan2(Game1.currentCursorTile.Y * 64 - position.Y, Game1.currentCursorTile.X * 64 - position.X);
        }

        public override void updatePosition(GameTime time)
        {
            Velocity += 0.1f;
            if (Homing)
            {
                var monsters = Game1.currentLocation.characters.OfType<Monster>().ToList();
                var closestMonster = monsters.OrderBy(m => Vector2.Distance(m.position, position)).FirstOrDefault();
                if (closestMonster != null)
                {
                    var monsterCenter = new Vector2(closestMonster.position.X - closestMonster.GetBoundingBox().Width / 4, closestMonster.position.Y - closestMonster.GetBoundingBox().Height / 4);
                    Direction = (float)Math.Atan2(monsterCenter.Y - position.Y, monsterCenter.X - position.X);
                }
            }
            position.X += (float)Math.Cos(Direction) * Velocity;
            position.Y += (float)Math.Sin(Direction) * Velocity;
        }

        public override void draw(SpriteBatch b)
        {
            Vector2 drawPos = Game1.GlobalToLocal(new Vector2(getBoundingBox().X + getBoundingBox().Width / 2, getBoundingBox().Y + getBoundingBox().Height / 2));
            b.Draw(Texture, drawPos, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Direction, new Vector2(Texture.Width / 2, Texture.Height / 2), 2, SpriteEffects.None, (float)((position.Y + (double)(Game1.tileSize * 3 / 2)) / 10000.0));
        }

        public override void behaviorOnCollisionWithOther(GameLocation loc)
        {
            //if (!Homing)
            //    destroyMe = true;
        }

        public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
        {
            destroyMe = true;
        }

        public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
        {
            if (n is not Monster)
                return;
            location.damageMonster(new Rectangle(n.GetBoundingBox().X, n.GetBoundingBox().Y, 64, 64), MinDamage + BonusDamage, MaxDamage + BonusDamage, false, -100, 100, 0, 0, false, Source);
            destroyMe = true;
        }

        public override void behaviorOnCollisionWithPlayer(GameLocation location, Farmer player)
        {
        }

        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
        {
            //if (!Homing)
            //    destroyMe = true;
        }
    }
}