using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DyingHope
{
    class Objectvariation
    {
        public string Name;
        public Vector2 Bearbeitungsverschiebung;    //Verschiebung in der Drawschleife für Z-Achse
        public Rectangle Texturausschnitt;          //Position und Größe der Textur innerhalb der Datei
        public Vector2 Pixelverschiebung;           //Position im 32x32 Pixel-Feld und Größe
        public bool Zerstörbar;                     //Kann zerstört werden
        public bool Animiert;                       //Objekt ist animiert
        public int Spalten;                         //Anzahl der Einzelbilder
        public int Geschwindigkeit;                 //Animationsgeschwindigkeit     
        public bool Wiederholen;                    //Daueranimation?  
        public Texture2D Textur;
        public List<Objectcollision> Kollision;

        public Objectvariation(Texture2D textur) 
        {
            this.Textur = textur;
            this.Kollision = new List<Objectcollision>();
        }
    }
}
