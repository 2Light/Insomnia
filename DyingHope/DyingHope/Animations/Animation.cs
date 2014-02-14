using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    class Animation
    {
        public Texture2D Texture;
        public bool Abgeschlossen;
        public bool Wiederholen;
        public float Zeit;
        public int Richtung;
        public int Spalte;
        public int Breite;
        public int Höhe;
        public float Skalierung;
        public float Geschwindigkeit;
        public int AktuelleSpalte;

        public Animation(Texture2D texture, bool wiederholen, int Spalte, int Breite, int Höhe, float skalierung, int Geschwindigkeit)
        {
            this.Texture = texture;
            this.Wiederholen = wiederholen;
            this.Spalte = Spalte;
            this.Breite = Breite;
            this.Höhe = Höhe;
            this.Skalierung = skalierung;
            this.Geschwindigkeit = (float)1 / Geschwindigkeit;

            Zeit = 0;
            AktuelleSpalte = 0;
            Abgeschlossen = false;
        }

        public void Update(float elapsed)
        {
            if (Abgeschlossen == false)
            {
                Zeit += elapsed;
                if (Zeit > Geschwindigkeit)
                {
                    Zeit -= Geschwindigkeit;

                    AktuelleSpalte += 1;
                    if (AktuelleSpalte >= Spalte)
                    {
                        if (Wiederholen == true) AktuelleSpalte = 0;
                        else Abgeschlossen = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(
                Texture,
                new Rectangle((int)position.X, (int)position.Y, (int)(Skalierung * Breite), (int)(Skalierung * Höhe)),
                new Rectangle(
                  AktuelleSpalte * Breite,
                  Richtung * Höhe,
                  Breite, Höhe),
                color
                );
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle feld, Color color, Richtung richtung)
        {
            switch (richtung)
            {
                case DyingHope.Richtung.Links: Richtung = 0; break;
                case DyingHope.Richtung.Rechts: Richtung = 1; break;
            }

            spriteBatch.Draw(
                Texture,
                new Rectangle((int)feld.X, (int)feld.Y, feld.Width, feld.Height),
                new Rectangle(
                  AktuelleSpalte * Breite,
                  Richtung * Höhe,
                  Breite, Höhe),
                color
                );
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, Richtung richtung, float skalierung)
        {
            switch (richtung)
            {
                case DyingHope.Richtung.Links: Richtung = 0; break;
                case DyingHope.Richtung.Rechts: Richtung = 1; break;
            }

            spriteBatch.Draw(
                Texture,
                new Rectangle((int)position.X, (int)position.Y, (int)(skalierung * Breite), (int)(skalierung * Höhe)),
                new Rectangle(
                  AktuelleSpalte * Breite,
                  Richtung * Höhe,
                  Breite, Höhe),
                color
                );
        }
    }
}
