using Otter;
using OtterTutorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OtterTutorial.Scenes;

namespace OtterTutorial.Entities
{
    public class Enemy : Entity
    {
        // Default speed an enemy will move in
        public const float DEFAULT_SPEED = 3.4f;

        // Default health points our enemy starts with
        public const int DEFAULT_HEALTH = 4;

        public int health = 1;
        public float speed = 1f;

        public Spritemap<string> sprite;

        public const float MOVE_DISTANCE = 900;

        // left = true, right = false
        public int direction = 0;
        // Used to keep track of the enemy distance moved
        public float distMoved = 0f;
        
        public float newX;
        public float newY;

        public Sound hurt = new Sound(Assets.SND_ENEMY_HURT);

        public Enemy(float x, float y)
            : base(x, y)
        {
            health = DEFAULT_HEALTH;
            speed = DEFAULT_SPEED;

            // Set up the Spritemap in the same manner we did for the player
            sprite = new Spritemap<string>(Assets.ENEMY_SPRITE, 32, 40);
            sprite.Add("standLeft", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("standRight", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("standDown", new int[] { 3, 4 }, new float[] { 10f, 10f });
            sprite.Add("standUp", new int[] { 6, 7 }, new float[] { 10f, 10f });
            sprite.Add("walkLeft", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("walkRight", new int[] { 0, 1 }, new float[] { 10f, 10f });
            sprite.Add("walkDown", new int[] { 3, 4 }, new float[] { 10f, 10f });
            sprite.Add("walkUp", new int[] { 6, 7 }, new float[] { 10f, 10f });
            sprite.Play("standLeft");

            Graphic = sprite;

            // Set our Enemy hitbox to be 32 x 40. This goes in our Enemy class
            SetHitbox(32, 40, (int)Global.Type.ENEMY);
        }

        public override void Update()
        {
            base.Update();

            GameScene checkScene = (GameScene)Scene;

            // Access the Enemy's Collider to check collision
            var collb = Collider.Collide(X, Y, (int)Global.Type.BULLET);
            if (collb != null)
            {
                Bullet b = (Bullet)collb.Entity;
                b.Destroy();

                // Shake the camera when a Bullet hits an Enemy
                // and then add a new DamageText object with the
                // abritary string "1234". Lastly add it to our Scene
                Global.camShaker.ShakeCamera();
                DamageText dt = new DamageText(X, Y, "1234");
                Global.TUTORIAL.Scene.Add(dt);

                hurt.Play();

                health--; // Decrement the health by 1 for each Bullet that hits
                if (health <= 0)
                {
                    // Add a new Explosion and remove self from the Scene if out of health
                    Global.TUTORIAL.Scene.Add(new Explosion(X, Y));
                    RemoveSelf();
                }
            }

            // If going left, flip the spritesheet
            if (direction < 2)
            {
                sprite.FlippedX = false;
            }
            else
            {
                sprite.FlippedX = true;
            }

            // if moving left then go left, otherwise go right
            if (direction == 0)
            {
                X -= speed;

                newX = X - speed;

                if (checkScene.grid.GetRect(newX, Y, newX + sprite.Width, Y + sprite.Height, false))
                {
                    if (direction == 3)
                    {
                        direction = 0;
                    }
                    else
                    {
                        direction = direction + 1;
                    }
                }
            }
            else if(direction == 1)
            {
                Y += speed;

                if (checkScene.grid.GetRect(newX, Y, newX + sprite.Width, Y + sprite.Height, false))
                {
                    if (direction == 3)
                    {
                        direction = 0;
                    }
                    else
                    {
                        direction = direction + 1;
                    }
                }
            }
            else if (direction == 2)
            {
                X += speed;

                newX = X + speed;

                if (checkScene.grid.GetRect(newX, Y, newX + sprite.Width, Y + sprite.Height, false))
                {
                    if (direction == 3)
                    {
                        direction = 0;
                    }
                    else
                    {
                        direction = direction + 1;
                    }
                }
            }
            else if (direction == 3)
            {
                Y -= speed;

               
                if (checkScene.grid.GetRect(newX, Y, newX + sprite.Width, Y + sprite.Height, false))
                {
                    if (direction == 3)
                    {
                        direction = 0;
                    }
                    else
                    {
                        direction = direction + 1;
                    }
                }

            }

            // Update distance moved, and check if we should flip directions
            distMoved += speed;
            if (distMoved >= MOVE_DISTANCE)
            {
                if (direction == 3)
                {
                    direction = 0;
                }
                else
                {
                    direction = direction + 1;
                } 
                distMoved = 0f;
            }
        }
    }
}