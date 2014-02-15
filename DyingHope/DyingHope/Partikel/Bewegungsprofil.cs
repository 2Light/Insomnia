using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace DyingHope
{
    public enum Bewegungsprofile
    {
        Linear,
        Random,
        LinearRandom
    }

    class BewegungsHandler
    {
        Random r = new Random();

        public  Vector2 move(Bewegungsprofile profil, float power,Vector2 velocity)
        {
            switch (profil)
            {
                case Bewegungsprofile.Linear:
                   return linear(power,velocity);
                   
                case Bewegungsprofile.Random:
                   return random(power,velocity);

                case Bewegungsprofile.LinearRandom:
                   return linearRandom(power, velocity);
            }
            return Vector2.Zero;
        }

        private  Vector2 linear(float power, Vector2 velocity)
        {
            return velocity;
        }

        private  Vector2 random(float power, Vector2 velocity)
        {
           
            double angle = Math.PI * r.Next(0,360) / 180.0;
            float particelX = (float)Math.Cos(angle);
            float particelY = -(float)Math.Sin(angle);

            
            return new Vector2(particelX,particelY);
        }

        public Vector2 linearRandom(float power, Vector2 velocity)
        {
            double angle = Math.PI * r.Next(0, 360) / 180.0;
            float particelX = (float)Math.Cos(angle);
            float particelY = -(float)Math.Sin(angle);
           Vector2 rand = new Vector2(particelX,particelY);

            return rand+velocity;
        }
    }
}
