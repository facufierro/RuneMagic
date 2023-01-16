using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;

namespace RuneMagic.assets.Spells.Effects
{
    internal class SpellProjectile : Projectile
    {
        /*********
        ** Fields
        *********/
        private readonly Farmer Source;
        private readonly NetInt Damage = new();
        private readonly NetFloat Direction = new();
        private readonly NetFloat Velocity = new();
        private readonly NetFloat Angle = new();
        private NetBool Homing = new();

        private Texture2D Texture;
        private readonly NetString TextureId = new();
        private Monster LookForTarget;

        private static readonly Random Rand = new();

        /*********
        ** Public methods
        *********/
        public SpellProjectile()
        {
            NetFields.AddFields(Damage, Direction, Velocity, Angle, Homing, TextureId);
        }
        public SpellProjectile(Farmer source, int damage, float velocity, float angle, bool homing)
            : this()
        {
            Source = source;
            Damage.Value = damage;
            Velocity.Value = velocity;
            Angle.Value = angle;
            Homing.Value = homing;

            theOneWhoFiredMe.Set(source.currentLocation, Source);
            position.Value = Source.getStandingPosition();
            position.X += Source.GetBoundingBox().Width;
            position.Y += Source.GetBoundingBox().Height;

            //get cursor tile position
            Vector2 target = Game1.currentCursorTile;


            int facingDirection = source.FacingDirection;

            switch (facingDirection)
            {
                case 0:
                    xVelocity.Value = (float)Rand.NextDouble() - angle;
                    yVelocity.Value = -Velocity.Value;
                    break;
                case 1:
                    xVelocity.Value = Velocity.Value;
                    yVelocity.Value = (float)Rand.NextDouble() - angle;
                    break;
                case 2:
                    xVelocity.Value = (float)Rand.NextDouble() - angle;
                    yVelocity.Value = Velocity.Value;
                    break;
                case 3:
                    xVelocity.Value = -Velocity.Value;
                    yVelocity.Value = (float)Rand.NextDouble() - angle;
                    break;
            }

            damagesMonsters.Value = true;

            Texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"assets/Textures/projectile.png");
            TextureId.Value = ModEntry.Instance.Helper.ModContent.GetInternalAssetName($"assets/Textures/projectile.png").BaseName;


            if (Homing.Value)
            {
                float nearestDist = float.MaxValue;
                Monster nearestMob = null;
                foreach (var character in source.currentLocation.characters)
                {
                    if (character is Monster mob)
                    {
                        float dist = Utility.distance(mob.Position.X, position.X, mob.Position.Y, position.Y);
                        if (dist < nearestDist)
                        {
                            nearestDist = dist;
                            nearestMob = mob;
                        }
                    }
                }


                LookForTarget = nearestMob;

            }
        }

        public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
        {
        }

        public override void behaviorOnCollisionWithMonster(NPC npc, GameLocation loc)
        {
            if (npc is not Monster)
                return;

            bool didDmg = loc.damageMonster(npc.GetBoundingBox(), Damage.Value, Damage.Value + 1, false, Source);
            Disappear(loc);
        }

        public override void behaviorOnCollisionWithOther(GameLocation loc)
        {
            if (!Homing.Value)
                Disappear(loc);
        }

        public override void behaviorOnCollisionWithPlayer(GameLocation loc, Farmer farmer)
        {
        }

        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation loc)
        {
            if (!Homing.Value)
                Disappear(loc);
        }

        public override bool isColliding(GameLocation location)
        {
            if (Homing.Value)
            {
                return location.doesPositionCollideWithCharacter(getBoundingBox()) != null;
            }
            else return base.isColliding(location);
        }

        public override Rectangle getBoundingBox()
        {
            return new((int)(position.X - Game1.tileSize), (int)(position.Y - Game1.tileSize), Game1.tileSize / 2, Game1.tileSize / 2);
        }

        public override bool update(GameTime time, GameLocation location)
        {
            if (Homing.Value)
            {
                if (LookForTarget is not { Health: > 0 } || LookForTarget.currentLocation == null)
                {
                    Disappear(location);
                    return true;
                }
                else
                {
                    Vector2 unit = new Vector2(LookForTarget.GetBoundingBox().Center.X + 32, LookForTarget.GetBoundingBox().Center.Y + 32) - position;
                    unit.Normalize();

                    xVelocity.Value = unit.X * Velocity.Value;
                    yVelocity.Value = unit.Y * Velocity.Value;
                }
            }

            return base.update(time, location);
        }

        public override void updatePosition(GameTime time)
        {
            position.X += xVelocity.Value;
            position.Y += yVelocity.Value;
        }

        public override void draw(SpriteBatch b)
        {
            Texture ??= Game1.content.Load<Texture2D>(TextureId.Value);
            Vector2 drawPos = Game1.GlobalToLocal(new Vector2(getBoundingBox().X + getBoundingBox().Width / 2, getBoundingBox().Y + getBoundingBox().Height / 2));
            b.Draw(Texture, drawPos, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Direction.Value, new Vector2(Texture.Width / 2, Texture.Height / 2), 2, SpriteEffects.None, (float)((position.Y + (double)(Game1.tileSize * 3 / 2)) / 10000.0));
        }

        /*********
        ** Private methods
        *********/
        private void Disappear(GameLocation loc)
        {
            Game1.createRadialDebris(loc, TextureId.Value, Game1.getSourceRectForStandardTileSheet(projectileSheet, 0), 4, (int)position.X, (int)position.Y, 6 + Rand.Next(10), (int)(position.Y / (double)Game1.tileSize) + 1, new Color(255, 255, 255, 8 + Rand.Next(64)), 2.0f);
            destroyMe = true;
        }
    }
}