using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DyingHope
{
    class Particel
    {
        Texture2D texture;
        float scale;
        Vector2 velocity;
        float angularVelocity;
        float angular;
        Vector2 pos;
        Vector2 origin;
        Bewegungsprofile profil;
        Spawnprofile spawnprofile;

        float power;
        public float lifeTime;
        public float currentLife;
        Vector2 currentVelocity;
        float drainLife;
        float percent;
        float stayOnMax;
        Random r = new Random();
        int randome;
        public bool up, down;

        public Particel(Texture2D ParticelTexture, float particelSize, Vector2 particelVelocity, float power, float angularVelocity, Vector2 spawnPos, float lifeTime, float drainLife, float stayOnMax, Bewegungsprofile profil, Spawnprofile spwanprofile)
        {
            this.spawnprofile = spwanprofile;
            if (this.spawnprofile == Spawnprofile.FadeIn)
            {
                this.lifeTime = lifeTime / 2;
                currentLife = 0;
                up = true;
                down = false;
            }
            else if (this.spawnprofile == Spawnprofile.Instant)
            {
                this.lifeTime = lifeTime;
                this.currentLife = this.lifeTime;
                up = false;
                down = true;
            }


            this.stayOnMax = stayOnMax;
            this.power = power;
            this.profil = profil;
            this.drainLife = drainLife;

            this.texture = ParticelTexture;
            this.scale = particelSize;
            this.velocity = particelVelocity;
            this.angularVelocity = angularVelocity;
            this.pos = spawnPos;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.currentVelocity = velocity;
        }

        public void Update(GameTime time, BewegungsHandler bewegung)
        {

            // Console.WriteLine(lifeTime);
            randome = r.Next(100, 500);
            if ((time.TotalGameTime.Milliseconds % randome) == 0) currentVelocity = bewegung.move(profil, power, velocity);
            if (currentLife == lifeTime && stayOnMax > 0) { down = false; up = false; }
            if (!down && !up) stayOnMax -= time.ElapsedGameTime.Milliseconds * drainLife;
            if (stayOnMax <= 0 && !down && !up) down = true;

            if (down) currentLife -= time.ElapsedGameTime.Milliseconds * drainLife;
            else if (up) currentLife += time.ElapsedGameTime.Milliseconds * drainLife;


            /* Console.WriteLine("Stayon:"+stayOnMax);
             Console.WriteLine("down:" + down);
             Console.WriteLine("up:" + up);
             Console.WriteLine("percent:" + percent);
             Console.WriteLine();*/


            this.angular += angularVelocity;
            this.pos += currentVelocity * power;
            percent = currentLife / lifeTime;
        }

        public void Draw(SpriteBatch batch,Player player)
        {

            batch.Draw(texture,new Vector2( pos.X - player.PositionCurrent.X,pos.Y), null, (Color.White) * percent, angular, origin, scale, SpriteEffects.None, 1f);
        }

        public void Draw(SpriteBatch batch)
        {

            batch.Draw(texture, new Vector2(pos.X , pos.Y), null, (Color.White) * percent, angular, origin, scale, SpriteEffects.None, 1f);
        }


        public void DrawDebug(SpriteBatch batch, SpriteFont font)
        {
            batch.DrawString(font, this.velocity.ToString(), new Vector2(100, 100), Color.White);
            // batch.DrawString(font, this.scale.ToString(), new Vector2(100, 120), Color.White);

        }
    }
}
