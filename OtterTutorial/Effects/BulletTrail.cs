﻿using Otter;
using OtterTutorial;
using OtterTutorial.Effects;
using System;
namespace OtterTutorial.Effects
{
    public class BulletTrail : BulletParticle
    {
        public const int DESTROY_FRAME = 3;
        public BulletTrail(float x, float y)
            : base(x, y)
        {
            destroyFrame = DESTROY_FRAME;
            sprite = new Spritemap<string>(Assets.BULLET_PARTICLE, 32, 40);
            sprite.Add("Emit", new int[] { 0, 1, 2, 3 }, new float[] { 10f, 10f, 10f, 10f });
            sprite.Play("Emit");
            Graphic = sprite;
        }
    }
}
