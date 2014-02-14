using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DyingHope
{
    class Background
    {
        public string Name;
        public Texture2D Textur;
        public Vector2 Scrollgeschwindigkeit;
        public float Transparenz;
        public Vector2 Position;   //Aktuelle Verschiebung
        private Vector2 Verschiebung;   //Verschiebung bei Bewegung
        public int versatzY;
        public int Startposition;
        public int Endposition;

        //Alte Lademethode 
        //public Background(Texture2D texture, Vector2 scroll, float transparenz, int versatz)
        //{
        //    this.Textur = texture;
        //    this.Scrollgeschwindigkeit = scroll;
        //    this.Transparenz = transparenz;
        //    this.Position = new Vector2(0, 0);
        //    this.Verschiebung = new Vector2(0, 0);
        //    this.versatzY = versatz;
        //}

        //Neue Lademethode für Manager
        public Background(string name, Vector2 scroll, float transparenz, int versatz, int start, int ende, ContentManager contentmanager)
        {
            this.Name = name;
            if (name != "Neu") this.Textur = contentmanager.Load<Texture2D>(@"Backgrounds\" + name);
            this.Scrollgeschwindigkeit = scroll;
            this.Transparenz = transparenz;
            this.Position = new Vector2(0, 0);
            this.Verschiebung = new Vector2(0, 0);
            this.versatzY = versatz;
            this.Startposition = start;
            if (Startposition == 0) Startposition = 1;
            this.Endposition = ende;
            if ((Endposition == 0) || (Endposition == 10000)) Endposition = 20000;
        }

        //Neue Lademethode für Datenbank
        public Background(string name, ContentManager contentmanager)
        {
            this.Name = name;
            this.Textur = contentmanager.Load<Texture2D>(@"Backgrounds\" + name);
        }

        public Background() { } //Dummy für Manager

        public void Update(Vector2 spieler)
        {
            if (Name != "Neu")
            {
                Position.X = ((-spieler.X * Scrollgeschwindigkeit.X) % Textur.Width);
                Position.Y = versatzY + ((-spieler.Y * Scrollgeschwindigkeit.Y) % Textur.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device, Color color, Vector2 spieler)
        {
            if (Textur != null)
            {
                if ((spieler.X * Scrollgeschwindigkeit.X) > Endposition)
                {
                    spriteBatch.Draw(Textur, new Vector2(Startposition + (((Endposition - Startposition) / Textur.Width) * Textur.Width) - (spieler.X * Scrollgeschwindigkeit.X), Position.Y), color * Transparenz);
                    spriteBatch.Draw(Textur, new Vector2(Startposition + (((Endposition - Startposition) / Textur.Width) * Textur.Width) + Textur.Width - (spieler.X * Scrollgeschwindigkeit.X), Position.Y), color * Transparenz);
                }
                else if ((spieler.X * Scrollgeschwindigkeit.X) > Startposition)
                {
                    spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width), Position.Y), color * Transparenz);
                    if (Position.X + Textur.Width > 0) spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width) + Textur.Width, Position.Y), color * Transparenz);
                    if (Position.X - Textur.Width < 0) spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width) - Textur.Width, Position.Y), color * Transparenz);
                }
                else if ((spieler.X * Scrollgeschwindigkeit.X) + 1280 > Startposition)
                {
                    spriteBatch.Draw(Textur, new Vector2(Startposition - (spieler.X * Scrollgeschwindigkeit.X), Position.Y), color * Transparenz);
                }
            }
        }

        //public void Draw(SpriteBatch spriteBatch, GraphicsDevice device, Color color, Vector2 spieler)
        //{
        //    if (Textur != null)
        //    {
        //        if ((spieler.X * Scrollgeschwindigkeit.X) > Startposition)
        //        {
        //            spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width), Position.Y), color * Transparenz);
        //            if (Position.X + Textur.Width > 0) spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width) + Textur.Width, Position.Y), Color.Red * Transparenz);
        //            if (Position.X - Textur.Width < 0) spriteBatch.Draw(Textur, new Vector2(Position.X + (Startposition % Textur.Width) - Textur.Width, Position.Y), Color.Blue * Transparenz);
        //        }
        //        else if ((spieler.X * Scrollgeschwindigkeit.X) + 1280 > Startposition)
        //        {
        //            spriteBatch.Draw(Textur, new Vector2(Startposition - (spieler.X * Scrollgeschwindigkeit.X), Position.Y), Color.Blue * Transparenz);
        //        }
        //    }
        //}

        //public void Draw(SpriteBatch spriteBatch, GraphicsDevice device, Color color)
        //{
        //    if (Textur != null)
        //    {
        //        spriteBatch.Draw(Textur, Position, color * Transparenz);
        //        if (Position.X + Textur.Width > 0) spriteBatch.Draw(Textur, new Vector2(Position.X + Textur.Width, Position.Y), color * Transparenz);
        //        if (Position.X > 0) spriteBatch.Draw(Textur, new Vector2(Position.X - Textur.Width, Position.Y), color * Transparenz);
        //    }
        //}
    }
}
