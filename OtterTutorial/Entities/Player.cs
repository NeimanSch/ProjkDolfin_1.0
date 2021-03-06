﻿using Otter;

using OtterTutorial;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OtterTutorial.Effects;
using OtterTutorial.Scenes;

using System;

namespace OtterTutorial.Entities
{
    public class Player : Entity
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 40;
        public const float DIAGONAL_SPEED = 3.4f;

        public float moveSpeed = 4.0f;
        public float diagonalSpeed = .7f;

        // Our entity's graphic will be a Spritemap
        public Spritemap<string> sprite;


        public int direction = 0;

        public Player(float x = 0, float y = 0)
        {
            // When creating a new player, the desired X,Y coordinates are passed in. If excluded, we start at 0,0
            X = x;
            Y = y;
            // Create a new spritemap, with the player.png image as our source, 32 pixels wide, and 40 pixels tall
            sprite = new Spritemap<string>(Assets.PLAYER, 32, 40);

            sprite.Add("standLeft", new int[] { 4 }, new float[] { 10f, 10f });
            sprite.Add("standRight", new int[] { 8 }, new float[] { 10f, 10f });
            sprite.Add("standDown", new int[] { 0 }, new float[] { 10f, 10f });
            sprite.Add("standUp", new int[] { 12 }, new float[] { 10f, 10f });
            sprite.Add("walkLeft", new int[] { 4,5,6,7 }, new float[] { 10f, 10f });
            sprite.Add("walkRight", new int[] { 8, 9, 10, 11 }, new float[] { 10f, 10f });
            sprite.Add("walkDown", new int[] { 0,1,2,3 }, new float[] { 10f, 10f });
            sprite.Add("walkUp", new int[] { 12,13,14,15 }, new float[] { 10f, 10f });

            // Tell the spritemap which animation to play when the scene starts
            sprite.Play("standDown");

            // Set our Enemy hitbox to be 32 x 40. This goes in our Enemy class
            SetHitbox(32, 40, (int)Global.Type.PLAYER);

            // Lastly, we must set our Entity's graphic, otherwise it will not display
            Graphic = sprite;
        }

        public override void Update()
        {
            base.Update();

            // Used to determine which directions we are moving in
            bool horizontalMovement = true;
            bool verticalMovement = true;

            float xSpeed = 0;
            float ySpeed = 0;
            float newX;
            float newY;
            GameScene checkScene = (GameScene)Scene;


            //jb testing speed changes
            if (Global.PlayerSession.Controller.L1.Pressed)
            {
                moveSpeed = moveSpeed * 1.2f;
            }

            // Check horizontal movement
            if (Global.PlayerSession.Controller.Left.Down)
            {
                newX = X - moveSpeed;
                // Check if we are colliding with a solid rectangle or not.
                // Ensure the GridCollider snaps our values to a grid, by passing
                // in a false boolean for the usingGrid parameter
                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = -moveSpeed;
                }

                sprite.Play("walkLeft");
                sprite.FlippedX = false;

                direction = Global.DIR_LEFT;
            }
            else if (Global.PlayerSession.Controller.Right.Down)
            {
                newX = X + moveSpeed;
                if (!checkScene.grid.GetRect(newX, Y, newX + WIDTH, Y + HEIGHT, false))
                {
                    xSpeed = moveSpeed;
                }

                direction = Global.DIR_RIGHT;
                sprite.Play("walkRight");
                sprite.FlippedX = false;

            }
            else
            {
                horizontalMovement = false;
            }
            // Check vertical movement
            if (Global.PlayerSession.Controller.Up.Down)
            {
                newY = Y - moveSpeed;
                if (!checkScene.grid.GetRect(X, newY, X + WIDTH, newY + HEIGHT, false))
                {
                    ySpeed = -moveSpeed;
                }

                direction = Global.DIR_UP; 
                sprite.Play("walkUp");
                sprite.FlippedX = false;

            }
            else if (Global.PlayerSession.Controller.Down.Down)
            {
                newY = Y + moveSpeed;
                if (!checkScene.grid.GetRect(X, newY, X + WIDTH, newY + HEIGHT, false))
                {
                    ySpeed = moveSpeed;
                }

                direction = Global.DIR_DOWN;
                sprite.Play("walkDown");
                sprite.FlippedX = false;

            }
            else
            {
                verticalMovement = false;
            }

            // If we are not moving play our idle animations
            // Currently our spritesheet lacks true idle
            // animations, but this helps get the idea across
            if (!horizontalMovement && !verticalMovement)
            {
                if (sprite.CurrentAnim.Equals("walkLeft"))
                {
                    sprite.Play("standLeft");
                }
                else if (sprite.CurrentAnim.Equals("walkRight"))
                {
                    sprite.Play("standRight");
                }
                else if (sprite.CurrentAnim.Equals("walkDown"))
                {
                    sprite.Play("standDown");
                }
                else if (sprite.CurrentAnim.Equals("walkUp"))
                {
                    sprite.Play("standUp");
                }
            }

            if (Global.PlayerSession.Controller.R1.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, direction));
            }else if (Global.PlayerSession.Controller.B.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, Global.DIR_RIGHT));
            }
            else if (Global.PlayerSession.Controller.X.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, Global.DIR_LEFT));
            }
            else if (Global.PlayerSession.Controller.Y.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, Global.DIR_UP));
            }
            else if (Global.PlayerSession.Controller.A.Pressed)
            {
                Global.TUTORIAL.Scene.Add(new Bullet(X, Y, Global.DIR_DOWN));
            }



            // Add particles if the player is moving in any direction
            if (verticalMovement || horizontalMovement)
            {
                // Add walking particles
                float particleXBuffer = 0;
                float particleYBuffer = 0;
                switch (direction)
                {
                    case Global.DIR_UP:
                        {
                            particleXBuffer = Otter.Rand.Float(8, 24);
                            particleYBuffer = Otter.Rand.Float(0, 5);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + particleXBuffer, Y + 40));
                            break;
                        }
                    case Global.DIR_DOWN:
                        {
                            particleXBuffer = Otter.Rand.Float(8, 24);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + particleXBuffer, Y));
                            break;
                        }
                    case Global.DIR_LEFT:
                        {
                            particleYBuffer = Otter.Rand.Float(-2, 2);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + 32 - 3, Y + 40 + particleYBuffer));
                            break;
                        }
                    case Global.DIR_RIGHT:
                        {
                            particleYBuffer = Otter.Rand.Float(-2, 2);
                            Global.TUTORIAL.Scene.Add(new WalkParticle(X + 3, Y + 40 + particleYBuffer));
                            break;
                        }
                }

                if (verticalMovement && horizontalMovement)
                {
                    X += xSpeed * diagonalSpeed;
                    Y += ySpeed * diagonalSpeed;
                }
                else
                {
                    X += xSpeed;
                    Y += ySpeed;
                }
            }
        }
    }
}