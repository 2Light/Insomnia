using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Emitter : IdebugObjekt
    {
        public bool go;
        public bool constantSpawnRate;

        public bool particelNeverDie;
        public Vector2 particelPerUpdate;
        public float startParticel;

        public Vector2 Position;
        public Vector2 Origin;

        public List<Vector2> EmitterAngel; // 
        public Vector2 EmitterRange;
        public float EmitterUpdate;
        public float EmitterMaxParticel;

        public bool ParticelRotate;
        public Vector2 PerticelPower; //x min , y max power;
        public Vector2 ParticelSize; // x min size, y max size
        public Vector2 ParticelLifeDrain;
        public Vector2 ParticelLifeTime;
        public Vector2 ParticelStayOnMax;

        public List<Texture2D> ParticelTexture;
        public Texture2D EmitterTexture;

        public Random r;

        Effect color;
        public int percent;
        float makeR;
        public List<Particel> particels;

        public Bewegungsprofile bewegungsprofil;
        public BewegungsHandler bewegung;

        public Spawnprofile spawnprofile;




        #region debugStuff

        public String debugString { get; set; }

        public StringBuilder stringBuilder { get; set; }
        public DebugFlag debugFlag { get; set; }

        public void addString(String text)
        {
            this.stringBuilder.AppendLine(text);
        }

        public void clearString()
        {
            this.stringBuilder.Clear();
            this.debugString = String.Empty;
        }

        public void DrawExtraDebug(DebugFlag extraFlag, SpriteBatch batch, Camera cam, Contents content)
        {
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.ViewMatrix);

            batch.Draw(EmitterTexture, Position, null, Color.White, 0, this.Origin, 1.0f, SpriteEffects.None, 1f);
            foreach (Vector2 v in EmitterAngel)
            {
                drawPartCircel(50, v.X, v.Y, Position, batch, content.Pixel);
            }
            drawLine(Position, new Vector2(Position.X + EmitterRange.X, 0 + Position.Y), content.Pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X - EmitterRange.X, 0 + Position.Y), content.Pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X, Position.Y + EmitterRange.Y), content.Pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X, Position.Y - EmitterRange.Y), content.Pixel, 1, batch);

            batch.End();
        }

        public void handelDebug()
        {
            addString(EmitterAngel.ToString());
            addString(particels.Count.ToString());
        }

        #endregion



       
        public Emitter(List<Texture2D> texture, Vector2 pos, Texture2D EmitterTexture, Effect color, Vector2 particelSize, Vector2 particelPower, List<Vector2> emitterAngel, Vector2 emitterRange, float particelAmount, float spawnTime, float startParticel, Vector2 lifeDrain, Vector2 lifeTime, Vector2 stayOnMax, Bewegungsprofile bewegung, Spawnprofile spawnprofile)
        {


            this.ParticelStayOnMax = stayOnMax;
            this.spawnprofile = spawnprofile;
            this.startParticel = startParticel;
            this.bewegung = new BewegungsHandler();
            this.ParticelLifeTime = lifeTime;
            this.ParticelLifeDrain = lifeDrain;
            this.EmitterAngel = emitterAngel;
            this.EmitterRange = emitterRange;
            this.EmitterTexture = EmitterTexture;
            this.Position = pos;
            this.ParticelSize = particelSize;
            this.ParticelTexture = texture;
            this.particels = new List<Particel>();
            this.PerticelPower = particelPower;
            this.ParticelSize = particelSize;
            this.r = new Random();
            this.color = color;
            this.EmitterMaxParticel = particelAmount;
            this.EmitterUpdate = spawnTime;
            this.Origin = new Vector2(this.EmitterTexture.Width / 2, this.EmitterTexture.Height / 2);
            this.bewegungsprofil = bewegung;
            this.go = false;
            Start();
        }

        public Emitter(Vector2 pos,DebugFlag flag)
        {
            this.debugFlag = flag;
            this.stringBuilder = new StringBuilder();
            this.particelPerUpdate = Vector2.Zero;
            this.ParticelStayOnMax = Vector2.Zero;
            this.bewegung = new BewegungsHandler();
            this.ParticelLifeTime = Vector2.Zero;
            this.ParticelLifeDrain = Vector2.Zero;
            this.EmitterAngel = new List<Vector2>() { Vector2.Zero };
            this.EmitterRange = Vector2.Zero;
            this.EmitterTexture = null;
            this.Position = pos;
            this.ParticelSize = Vector2.Zero;
            this.ParticelTexture = new List<Texture2D>();
            this.particels = new List<Particel>();
            this.PerticelPower = Vector2.Zero;
            this.ParticelSize = Vector2.Zero;
            this.r = new Random();
            this.color = null;
            this.EmitterMaxParticel = 0f;
            this.EmitterUpdate = 0f;
            //  this.Origin = new Vector2(this.EmitterTexture.Width / 2, this.EmitterTexture.Height / 2);
            this.bewegungsprofil = Bewegungsprofile.Linear;
            this.go = false;
        }

        public void Start()
        {
            this.go = true;

            for (int i = 0; i < this.startParticel; i++)
            {
                generateParticel(r);
            }
        }

        public void Stop()
        {
            this.go = false;


        }



        private void generateParticel(Random r)
        {
               
            //Console.WriteLine("gen");
            float stayOnMax = (float)r.NextDouble() * (ParticelStayOnMax.Y - ParticelStayOnMax.X) + ParticelStayOnMax.X;

            float angularVelocity = 0.00f;
            int spawnX = r.Next((int)(Position.X - EmitterRange.X), (int)(Position.X + EmitterRange.X));
            int spawnY = r.Next((int)(Position.Y - EmitterRange.Y), (int)(Position.Y + EmitterRange.Y));

            Vector2 spawnPos = new Vector2(spawnX, spawnY);

            int pos = r.Next(0, EmitterAngel.Count);
            Vector2 angel = EmitterAngel.ElementAt(pos);

            if (angel.X > angel.Y) angel.Y = 360 + angel.Y;

            makeR = r.Next((int)angel.X, (int)angel.Y);

            double angle = Math.PI * makeR / 180.0;
            float particelX = (float)Math.Cos(angle);
            float particelY = -(float)Math.Sin(angle);



            float power = (float)r.NextDouble() * (PerticelPower.Y - PerticelPower.X) + PerticelPower.X;

            Vector2 particelVelocity = new Vector2(particelX, particelY);

            float particelSize = (float)r.NextDouble() * (ParticelSize.Y - ParticelSize.X) + ParticelSize.X;

            float drainLife = (float)r.NextDouble() * (ParticelLifeDrain.Y - ParticelLifeDrain.X) + ParticelLifeDrain.X;
            float lifeTime = (float)r.NextDouble() * (ParticelLifeTime.Y - ParticelLifeTime.X) + ParticelLifeTime.X;
            Texture2D partText = ParticelTexture.ElementAt(r.Next(0, ParticelTexture.Count - 1));

            particels.Add(new Particel(partText, particelSize, particelVelocity, power, angularVelocity, spawnPos, lifeTime, drainLife, stayOnMax, bewegungsprofil, spawnprofile));
        }


        public void Update(GameTime time)
        {
            float rand = (float)r.NextDouble() * (particelPerUpdate.Y - particelPerUpdate.X) + particelPerUpdate.X;
            
            if (this.go)
            {
                if (time.TotalGameTime.Milliseconds % EmitterUpdate == 0 && particels.Count < EmitterMaxParticel)
                {
                     Console.WriteLine(go + " Anzahl partikel " + particels.Count + " Max part: " + EmitterMaxParticel);
           
                    for (int i = 0; i <= rand; i++)
                    {
                        generateParticel(r);
                    }

                }

            }
            for (int particle = 0; particle < this.particels.Count; particle++)
            {
                this.particels[particle].Update(time, bewegung);

                if (this.particels[particle].currentLife <= 0 && !particelNeverDie)
                {
                    this.particels.RemoveAt(particle);
                    particle--;
                }

                else if (this.particels[particle].currentLife <= 0 && particelNeverDie) { this.particels[particle].up = true; this.particels[particle].down = false; }
                else if (this.particels[particle].currentLife >= this.particels[particle].lifeTime) { this.particels[particle].up = false; this.particels[particle].down = true; }

            }

        }

        #region RenderStuff

        public void Draw(SpriteBatch batch,Player player)
        {


            foreach (Particel p in particels)
            {
                p.Draw(batch,player);
            }
        }

        public void Draw(SpriteBatch batch)
        {


            foreach (Particel p in particels)
            {
                p.Draw(batch);
            }
        }

        public void DrawDebug(SpriteBatch batch, Texture2D pixel, Texture2D power, SpriteFont font)
        {
            batch.Draw(EmitterTexture, Position, null, Color.White, 0, this.Origin, 1.0f, SpriteEffects.None, 1f);

            foreach (Particel p in particels)
            {
                p.DrawDebug(batch, font);
            }
            //color.Parameters["percent"].SetValue(percent);
            batch.DrawString(font, EmitterAngel.ToString(), new Vector2(100, 160), Color.White);
            // batch.DrawString(font, makeR.ToString(), new Vector2(100, 160), Color.White);
            batch.DrawString(font, particels.Count.ToString(), new Vector2(100, 180), Color.White);
            foreach (Vector2 v in EmitterAngel)
            {
                drawPartCircel(50, v.X, v.Y, Position, batch, pixel);
            }

            drawLine(Position, new Vector2(Position.X + EmitterRange.X, 0 + Position.Y), pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X - EmitterRange.X, 0 + Position.Y), pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X, Position.Y + EmitterRange.Y), pixel, 1, batch);
            drawLine(Position, new Vector2(Position.X, Position.Y - EmitterRange.Y), pixel, 1, batch);


        }

        public void drawPartCircel(float radius, float startAngel, float endAngel, Vector2 pos, SpriteBatch batch, Texture2D texture)
        {

            for (float i = startAngel; i <= endAngel; i += 0.1f)
            {
                float x = (float)(pos.X + Math.Cos(i * Math.PI / 180) * radius);
                float y = (float)(pos.Y - Math.Sin(i * Math.PI / 180) * radius);

                Vector2 pixelpos = new Vector2(x, y);
                batch.Draw(texture, pixelpos, Color.White);
            }
        }

        public void drawLine(Vector2 from, Vector2 to, Texture2D texture, float size, SpriteBatch spriteBatch)
        {

            double degress = Math.Atan2((to - from).Y, (to - from).X);
            float length = Vector2.Distance(from, to);
            spriteBatch.Draw(texture, from, new Rectangle(0, 0, 1, 1), Color.White, (float)degress, Vector2.Zero, new Vector2(length, size), SpriteEffects.None, 0);

        }
        #endregion
    }
}
