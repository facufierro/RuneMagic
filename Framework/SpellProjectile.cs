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

namespace RuneMagic.Magic
{
    internal class SpellProjectile : Projectile
    {
        /*********
        ** Fields
        *********/
        private readonly Farmer Source;
        private NetString SpellTexture = new();
        private readonly NetInt MinDamage = new();
        private readonly NetInt MaxDamage = new();
        private readonly NetInt BonusDamage = new();
        private readonly NetFloat Direction = new();
        private readonly NetFloat Velocity = new();
        private NetInt Range = new();
        private NetBool IsHoming = new();
        private NetVector2 CursorPosition = new();
        private NetVector2 RandomCursorPosition = new();
        private NetInt Spread = new();
        private NetString SoundHit = new();


        private Texture2D Texture;
        private readonly NetString TextureId = new();
        private Vector2 Target;
        private float nearestDistance = float.MaxValue;
        private Monster nearestMonster = null;
        private bool seekTarget = false;
        private double creationTime;

        private static readonly Random Rand = new();

        /*********
        ** Public methods
        *********/
        public SpellProjectile()
        {
            NetFields.AddFields(SpellTexture, MinDamage, MaxDamage, BonusDamage, Direction, Velocity, Range, Spread, IsHoming, SoundHit, TextureId);
        }
        public SpellProjectile(Farmer source, string spellTexture, int minDamage, int maxDamage, int bonusDamage, float velocity, int range, int spread, bool isHoming, string soundHit)
            : this()
        {
            Source = source;
            SpellTexture.Value = spellTexture;
            MinDamage.Value = minDamage;
            MaxDamage.Value = maxDamage;
            BonusDamage.Value = bonusDamage;
            Velocity.Value = velocity;
            Range.Value = range;
            Spread.Value = spread;
            IsHoming.Value = isHoming;
            SoundHit.Value = soundHit;

            theOneWhoFiredMe.Set(source.currentLocation, Source);
            position.Value = Source.getStandingPosition();
            position.X += Source.GetBoundingBox().Width;
            position.Y += Source.GetBoundingBox().Height;
            damagesMonsters.Value = true;
            Texture = ModEntry.Instance.Helper.ModContent.Load<Texture2D>($"assets/Textures/Spells/{spellTexture}.png");
            TextureId.Value = ModEntry.Instance.Helper.ModContent.GetInternalAssetName($"assets/Textures/Spells/{spellTexture}.png").BaseName;

            CursorPosition.Value = new Vector2(Game1.getMousePosition().X + Game1.viewport.X + Game1.tileSize, Game1.getMousePosition().Y + Game1.viewport.Y + Game1.tileSize);
            RandomCursorPosition.Value = new Vector2(CursorPosition.X + Rand.Next(-Game1.tileSize * spread, Game1.tileSize * spread), CursorPosition.Y + Rand.Next(-Game1.tileSize * spread, Game1.tileSize * spread));
            creationTime = Game1.currentGameTime.TotalGameTime.TotalMilliseconds;

            if (IsHoming.Value)
            {
                foreach (var character in source.currentLocation.characters)
                {
                    if (character is Monster mob)
                    {
                        float distance = Utility.distance(mob.Position.X, CursorPosition.X, mob.Position.Y, CursorPosition.Y);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestMonster = mob;
                        }
                    }
                }
            }
        }

        public override bool update(GameTime time, GameLocation location)
        {



            if (!seekTarget)
            {

                if (time.TotalGameTime.TotalMilliseconds - creationTime > 500)
                {
                    seekTarget = true;
                }


            }

            if (seekTarget == false)
            {
                //Target = random position around the cursor
                Target = RandomCursorPosition;

            }
            else
            {
                if (nearestMonster is not null && (int)nearestDistance < Range)
                {
                    Target.X = nearestMonster.position.Value.X + Game1.tileSize;
                    Target.Y = nearestMonster.position.Value.Y + Game1.tileSize;
                }
                else
                {
                    Target = CursorPosition;
                }
            }





            Vector2 direction = Target - position;
            direction.Normalize();
            xVelocity.Value = direction.X * Velocity;
            yVelocity.Value = direction.Y * Velocity;

            float distance = Vector2.Distance(position, Target);

            if (distance <= 3)
            {
                if (time.TotalGameTime.TotalMilliseconds > 300)
                {
                    //play sound 
                    if (SoundHit.Value != "")
                    {
                        Game1.playSound(SoundHit.Value);
                    }
                    Disappear(location);
                    return true;
                }
            }
            if (time.TotalGameTime.TotalMilliseconds - creationTime > 1000)
            {
                Disappear(location);
                return true;
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