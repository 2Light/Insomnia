using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Lever
    {
        public string Name = "Neuer Schalter";
        public bool Betätigt;
        public bool BetätigtFlanke;
        public bool Flankenmerker;
        public bool Einmalig = true;
        public bool Rücksetzen;
        public Vector2 Position;
        public Vector2 Maße;
        public Vector2 Mittelpunkt;
        public Animation Animation;

        //public Lever() { }  //Konstruktor Ladevorgang

        public Lever(Vector2 maße, Texture2D texture)  //Konstruktor Editor
        {
            Maße = maße;
            Animation = new Animation(texture, false, 5, new Rectangle(0, 0, (int)maße.X, (int)maße.Y), 1, 7);
            Mittelpunkt = new Vector2(maße.X / 2, maße.Y / 2);
        }

        public void Update(Animationsmanager animationsmanager)
        {
            if (Animation.Abgeschlossen) 
            {
                animationsmanager.RemoveAnimation(Animation);
                Betätigt = !Betätigt;
                if (!Einmalig)
                {
                    Animation.Abgeschlossen = false;
                    Animation.AktuelleSpalte = 0;
                    if ((Betätigt) && (Rücksetzen)) //Wenn Schalter selbstständig rücksetzen soll
                    {
                        animationsmanager.AddAnimation(Animation); //Animation zur Dauerbearbeitung übergeben
                    }
                }
            }
            if (Betätigt)
            {
                BetätigtFlanke = true;
                if (Flankenmerker) BetätigtFlanke = false;
                Flankenmerker = true;
            }
            else Flankenmerker = false;
        }

        public void Draw(SpriteBatch spritebatch, Player player)
        {
            if (Betätigt) Animation.Draw(spritebatch, new Rectangle((int)(Position.X - player.PositionCurrent.X), (int)Position.Y, (int)Maße.X, (int)Maße.Y), Color.White, Richtung.Links);
            else Animation.Draw(spritebatch, new Rectangle((int)(Position.X - player.PositionCurrent.X), (int)Position.Y, (int)Maße.X, (int)Maße.Y), Color.White, Richtung.Rechts);
        }
    }
}
