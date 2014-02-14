using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;


namespace DyingHope
{
    class BackgroundMusic
    {
        private Settings Settings;
        public List<MSong> songs;
        public MSong currentSong;
        int index;

        public BackgroundMusic(Settings settings, List<MSong> songList)
        {
            this.Settings = settings;
            this.songs = songList;
            this.index = 0;
            // currentSong = songs.ElementAt(index);
            init();
            Start();
        }

        public void init()
        {
            foreach (MSong m in songs)
            {
                m.SongWaveInstance.Play();
                m.SongWaveInstance.Pause();

            }
        }

        public void Update()
        {
            //Console.WriteLine("Update Sound" + currentSong.SongWav.Duration + " index:" + index+" Size: "+songs.Count);
            if (Settings.Music)
            {
                if (isStooped())
                {
                    if (index + 1 == songs.Count) currentSong.SongWaveInstance.Pause();
                    else
                    {
                        if (currentSong.looped < currentSong.totalLoops) Play();
                        else
                        {

                            index++;
                            Start();
                        }

                    }
                }

            }

        }


        public void changeTrackList(List<MSong> tracklist)
        {
            currentSong.SongWaveInstance.Stop();

            currentSong = null;
            this.songs = null;
            this.songs = new List<MSong>();
            foreach (MSong s in tracklist)
            {
                this.songs.Add(s);
            }


            this.index = 0;
            //init();
            Start();
        }
        public bool isStooped()
        {
            return currentSong.SongWaveInstance.State == Microsoft.Xna.Framework.Audio.SoundState.Stopped;
        }

        public void Start()
        {
            currentSong = songs[index];
            Play();
        }

        public void Play()
        {
            currentSong.Play();
        }

        public void Stop()
        {
            currentSong.SongWaveInstance.Pause();
        }

        public void Resume()
        {
            currentSong.SongWaveInstance.Resume();
        }
        public void nextTrack()
        {
            index++;
            currentSong.SongWaveInstance.Stop();
            Start();

        }

        public void DrawDebug(SpriteBatch batch, SpriteFont font)
        {
            TimeSpan time = MediaPlayer.PlayPosition;
            TimeSpan songTime = currentSong.SongWav.Duration;

            batch.DrawString(font, "Songs: " + songs.Count, new Vector2(0, 0), Color.White);
            batch.DrawString(font, "CurrentSong loops: " + currentSong.totalLoops, new Vector2(0, 20), Color.White);
            batch.DrawString(font, "CurrentSong Looped: " + currentSong.looped, new Vector2(0, 40), Color.White);

            batch.DrawString(font, "CurrentSong ID: " + currentSong.sid, new Vector2(0, 60), Color.White);
            batch.DrawString(font, GetHumanReadableTime(time) + " / " + GetHumanReadableTime(songTime), new Vector2(100, 80), Color.Black);
        }

        public string GetHumanReadableTime(TimeSpan timeSpan)
        {
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            if (seconds < 10)
                return minutes + ":0" + seconds;
            else
                return minutes + ":" + seconds;
        }
    }
}
