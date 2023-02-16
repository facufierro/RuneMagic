using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneMagic.Source
{
    public class MagicFarmer : Farmer
    {
        public MagicFarmer() : base()
        {
        }

        public new void updateMovementAnimation(GameTime time)
        {
            if (_emoteGracePeriod > 0)
            {
                _emoteGracePeriod -= time.ElapsedGameTime.Milliseconds;
            }

            if (isEmoteAnimating)
            {
                bool flag = false;
                flag = ((!IsLocalPlayer) ? IsRemoteMoving() : (movementDirections.Count > 0));
                if ((flag && _emoteGracePeriod <= 0) || !FarmerSprite.PauseForSingleAnimation)
                {
                    EndEmoteAnimation();
                }
            }

            bool flag2 = IsCarrying();
            if (!isRidingHorse())
            {
                xOffset = 0f;
            }

            if (CurrentTool is FishingRod)
            {
                FishingRod fishingRod = CurrentTool as FishingRod;
                if (fishingRod.isTimingCast || fishingRod.isCasting)
                {
                    fishingRod.setTimingCastAnimation(this);
                    return;
                }
            }

            if (FarmerSprite.PauseForSingleAnimation || UsingTool)
            {
                return;
            }

            if (IsSitting())
            {
                ShowSitting();
                return;
            }

            if (IsLocalPlayer && !CanMove && !Game1.eventUp)
            {
                if (isRidingHorse() && mount != null && !isAnimatingMount)
                {
                    showRiding();
                }
                else if (flag2)
                {
                    showCarrying();
                }

                return;
            }

            if (IsLocalPlayer || isFakeEventActor)
            {
                moveUp = movementDirections.Contains(0);
                moveRight = movementDirections.Contains(1);
                moveDown = movementDirections.Contains(2);
                moveLeft = movementDirections.Contains(3);
                if (moveUp || moveRight || moveDown)
                {
                    _ = 1;
                }
                else
                    _ = moveLeft;
                if (moveLeft)
                {
                    FacingDirection = 3;
                }
                else if (moveRight)
                {
                    FacingDirection = 1;
                }
                else if (moveUp)
                {
                    FacingDirection = 0;
                }
                else if (moveDown)
                {
                    FacingDirection = 2;
                }

                if (isRidingHorse() && !mount.dismounting)
                {
                    base.speed = 2;
                }
            }
            else
            {
                moveLeft = IsRemoteMoving() && FacingDirection == 3;
                moveRight = IsRemoteMoving() && FacingDirection == 1;
                moveUp = IsRemoteMoving() && FacingDirection == 0;
                moveDown = IsRemoteMoving() && FacingDirection == 2;
                bool num = moveUp || moveRight || moveDown || moveLeft;
                float num2 = position.CurrentInterpolationSpeed() / ((float)Game1.currentGameTime.ElapsedGameTime.Milliseconds * 0.066f);
                running = Math.Abs(num2 - 5f) < Math.Abs(num2 - 2f) && !bathingClothes && !onBridge.Value;
                if (!num)
                {
                    FarmerSprite.StopAnimation();
                }
            }

            if (hasBuff(19))
            {
                running = false;
                moveUp = false;
                moveDown = false;
                moveLeft = false;
                moveRight = false;
            }

            if (!FarmerSprite.PauseForSingleAnimation && !UsingTool)
            {
                if (isRidingHorse() && !mount.dismounting)
                {
                    showRiding();
                }
                else if (moveLeft && running && !flag2)
                {
                    FarmerSprite.animate(56, time);
                }
                else if (moveRight && running && !flag2)
                {
                    FarmerSprite.animate(40, time);
                }
                else if (moveUp && running && !flag2)
                {
                    FarmerSprite.animate(48, time);
                }
                else if (moveDown && running && !flag2)
                {
                    FarmerSprite.animate(32, time);
                }
                else if (moveLeft && running)
                {
                    FarmerSprite.animate(152, time);
                }
                else if (moveRight && running)
                {
                    FarmerSprite.animate(136, time);
                }
                else if (moveUp && running)
                {
                    FarmerSprite.animate(144, time);
                }
                else if (moveDown && running)
                {
                    FarmerSprite.animate(128, time);
                }
                else if (moveLeft && !flag2)
                {
                    FarmerSprite.animate(24, time);
                }
                else if (moveRight && !flag2)
                {
                    FarmerSprite.animate(8, time);
                }
                else if (moveUp && !flag2)
                {
                    FarmerSprite.animate(16, time);
                }
                else if (moveDown && !flag2)
                {
                    FarmerSprite.animate(0, time);
                }
                else if (moveLeft)
                {
                    FarmerSprite.animate(120, time);
                }
                else if (moveRight)
                {
                    FarmerSprite.animate(104, time);
                }
                else if (moveUp)
                {
                    FarmerSprite.animate(112, time);
                }
                else if (moveDown)
                {
                    FarmerSprite.animate(96, time);
                }
                else if (flag2)
                {
                    showNotCarrying();
                }
                else
                {
                    showNotCarrying();
                }
            }
        }
    }
}