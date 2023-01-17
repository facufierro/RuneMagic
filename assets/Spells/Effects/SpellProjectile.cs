using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
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
        private readonly NetInt MinDamage = new();
        private readonly NetInt MaxDamage = new();
        private readonly NetInt BonusDamage = new();
        private readonly NetFloat Direction = new();
        private readonly NetFloat Velocity = new();
        private NetInt Range = new();
        private NetBool IsHoming = new();



        private Texture2D Texture;
        private readonly NetString TextureId = new();
        private Vector2 Target;
        private float nearestDistance = float.MaxValue;
        private Monster nearestMonster = null;


        private static readonly Random Rand = new();

        /*********
        ** Public methods
        *********/
        public SpellProjectile()
        {
            NetFields.AddFields(MinDamage, MaxDamage, BonusDamage, Direction, Velocity, Range, IsHoming, TextureId);
        }
        public SpellProjectile(Farmer source, int minDamage, int maxDamage, int bonusDamage, float velocity, int range, bool isHoming)
            : this()
        {
            Source = source;
            MinDamage.Value = minDamage;
            MaxDamage.Value = maxDamage;
            BonusDamage.Value = bonusDamage;
            Velocity.Value = velocity;
            Range.Value = range;
            IsHoming.Value = isHoming;


            theOneWhoFiredMe.Set(source.currentLocation, Source);
            position.Value = Source.getStandingPosition();
            position.X += Source.GetBoundingBox().Width;
            position.Y += Source.GetBoundingBox().Height;
            damagesMonsters.Value = true;
            Texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"assets/Textures/projectile.png");
            TextureId.Value = ModEntry.Instance.Helper.ModContent.GetInternalAssetName($"assets/Textures/projectile.png").BaseName;
            var cursorPosition = Game1.getMousePosition();
            var cursorPositionInGame = new Vector2(cursorPosition.X + Game1.viewport.X + Game1.tileSize, cursorPosition.Y + Game1.viewport.Y + Game1.tileSize);

            if (IsHoming.Value)
            {

                foreach (var character in source.currentLocation.characters)
                {
                    if (character is Monster mob)
                    {
                        float distance = Utility.distance(mob.Position.X, cursorPositionInGame.X, mob.Position.Y, cursorPositionInGame.Y);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestMonster = mob;
                        }
                    }
                }

            }
            if (nearestMonster is null || (int)nearestDistance > Range)
            {

                Target = cursorPositionInGame;
            }
        }

        public override bool update(GameTime time, GameLocation location)
        {
            if (nearestMonster is not null && (int)nearestDistance < Range)
            {
                Target.X = nearestMonster.position.Value.X + Game1.tileSize;
                Target.Y = nearestMonster.position.Value.Y + Game1.tileSize;
            }


            Vector2 direction = Target - position;
            direction.Normalize();
            xVelocity.Value = direction.X * Velocity;
            yVelocity.Value = direction.Y * Velocity;

            float distance = Vector2.Distance(position, Target);

            if (distance <= 3)
            {
                //wait 200 game ticks before despawning
                if (time.TotalGameTime.TotalMilliseconds > 300)
                {
                    Disappear(location);
                    return true;
                }


            }
            return base.update(time, location);
        }


        public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
        {

        }

        public override void behaviorOnCollisionWithMonster(NPC npc, GameLocation location)
        {
            if (npc is not Monster)
                return;

            bool didDmg = location.damageMonster(npc.GetBoundingBox(), MinDamage.Value + BonusDamage.Value, MaxDamage.Value + BonusDamage.Value, false, Source);
            Disappear(location);
        }

        public override void behaviorOnCollisionWithOther(GameLocation location)
        {


        }

        public override void behaviorOnCollisionWithPlayer(GameLocation location, Farmer farmer)
        {

        }

        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
        {
            Disappear(location);
        }

        public override bool isColliding(GameLocation location)
        {
            return base.isColliding(location);
        }

        public override Rectangle getBoundingBox()
        {
            return new((int)(position.X - Game1.tileSize), (int)(position.Y - Game1.tileSize), Game1.tileSize / 2, Game1.tileSize / 2);
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
        private void Disappear(GameLocation location)
        {


            Game1.createRadialDebris(location, TextureId.Value, Game1.getSourceRectForStandardTileSheet(projectileSheet, 0), 4,
                (int)position.X, (int)position.Y, 6 + Rand.Next(10), (int)(position.Y / (double)Game1.tileSize) + 1,
                new Color(255, 255, 255, 8 + Rand.Next(64)), 2.0f);

            destroyMe = true;
        }
    }
}