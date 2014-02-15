using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace DyingHope
{
    class SoundObjekt
    {
        private Settings Settings;
        public Vector2 position;
        public Texture2D texture;
        public SoundEffect soundeffect;
        public SoundEffectInstance soundEngine;
        Vector2 origin;
        public Vector2 Zone;
        public float radius;
        public float abstand;
        public float abstandZone;
        public float abstandRadius;
        public float vol;

        public SoundObjekt(Settings settings, Texture2D texture,SoundEffect effect, Vector2 position,Vector2 Zone, float radius)
        {
            this.Settings = settings;
            this.radius = radius;
            this.Zone = Zone;
            this.soundeffect = effect;
            this.position = position;
            this.texture = texture;
            this.soundEngine = soundeffect.CreateInstance();
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);

        }

        public void checkDistanze(Vector2 toCheck)
        {
            if (Settings.Sound)
            {
                abstand = Math.Abs(position.X - toCheck.X);
                abstandZone = abstand - Zone.X;
                abstandRadius = abstandZone - radius;

                if (abstandRadius <= 0)
                {
                    vol = (Math.Abs(abstandRadius) / radius);
                    vol = vol > 1 ? 1 : vol;
                    if (this.isPause() == SoundState.Paused) this.Resume();
                    else if (this.isPause() == SoundState.Stopped) this.PlaySound();

                    this.setVol(vol);
                }
            }
        }

        public void PlaySound()
        {
            this.soundEngine.Play();
        }

        public void Resume()
        {
            this.soundEngine.Resume();
        }

        public void Stop()
        {
            this.soundEngine.Stop();
           
        }

        public void Pause()
        {
            this.soundEngine.Pause();
        }


        public SoundState isPause()
        {
            return this.soundEngine.State;
        }

        public void setVol(float Vol)
        {
            this.soundEngine.Volume = Vol;
        }
       
        public void incressVol(float Vol)
        {
            //this.soundEngine.Volume + 0.1f;
        }

        public void decresVol(float Vol)
        {
            this.soundEngine.Volume = this.soundEngine.Volume - 0.1f;
        }


        public void Draw(SpriteBatch batch)
        {
            batch.Draw(this.texture, this.position, null, Color.White, 0f, this.origin, 1f, SpriteEffects.None, 0);
        }



    }
}
