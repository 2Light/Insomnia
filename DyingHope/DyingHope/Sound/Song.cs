using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DyingHope
{
    class MSong
    {
        public SoundEffect SongWav;
        public SoundEffectInstance SongWaveInstance;

        public int looped;
        public int totalLoops;
        public static int id = 1;
        public int sid;
        public bool unLimietLoop;

        public MSong(SoundEffect song, int totalLoops)
        {

            this.totalLoops = totalLoops;
            if (totalLoops == -1) this.unLimietLoop = true;
            else this.unLimietLoop = false;
            this.looped = -1;

            this.SongWav = song;
            SongWaveInstance = SongWav.CreateInstance();
            this.SongWaveInstance.IsLooped = unLimietLoop;
            this.sid = id;
            id++;
        }


        public void Play()
        {
            this.looped++;
            SongWaveInstance.Play();
        }

        public void breakLoop()
        {
            this.looped = totalLoops;
        }
    }
}
