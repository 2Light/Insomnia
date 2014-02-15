using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Object
    {
        public ObjectClass Typ;                     //Typ des Objekts
        public int Variante;                        //Variation innerhalb des Typs
        public Vector2 PositionCurrent;
        public Vector2 PositionLast;
        public int BearbeitungsID;                  //Platz in der Draw-Reihenfolge
        public float Skalierung = 1;
        public int Layer;                           //Layerposition
        public Objectvariation Objektvariante;      //Daten der Variante
        public Animation Animation;
        public Objektebene Objektebene;
        public bool Statisch = true;   //Nicht statisch = wird von Gravitation beeinflusst, kann vom Spieler bewegt werden
        public Vector2 Geschwindigkeit;
        public Vector2 MaxSpeed;      //Maximale Geschwindigkeit

        public Object() { }

        public Object(Objectdatabase objektdatenbank, Animationsmanager animationsmanager, ObjectClass typ, int variante , Vector2 position, float skalierung, int layer, Objektebene objektebene, bool statisch)
        {
            this.Statisch = statisch;
            this.MaxSpeed = new Vector2(6, 12);
            this.Typ = typ;
            this.PositionCurrent = position;
            this.Skalierung = skalierung;
            this.Layer = layer;
            this.Variante = variante;
            this.Objektvariante = objektdatenbank.Auslesen(Typ, Variante);
            if (this.Objektvariante == null) return;
            this.Objektebene = objektebene;
            this.BearbeitungsID = (int)PositionCurrent.Y + (int)Objektvariante.Bearbeitungsverschiebung.Y + (Layer * 1000);
            if (Objektvariante.Animiert == true)
            {
                Animation = new Animation(Objektvariante.Textur, Objektvariante.Wiederholen, Objektvariante.Spalten, Objektvariante.Texturausschnitt.Width, Objektvariante.Texturausschnitt.Height, skalierung, Objektvariante.Geschwindigkeit);
                animationsmanager.AddAnimation(Animation);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Player player, Color color)
        {
            if (Animation != null) Animation.Draw(spriteBatch, new Vector2(PositionCurrent.X - player.PositionCurrent.X, PositionCurrent.Y), color);
            else spriteBatch.Draw(Objektvariante.Textur, new Rectangle((int)(PositionCurrent.X - player.PositionCurrent.X), (int)(PositionCurrent.Y), (int)(Skalierung * Objektvariante.Texturausschnitt.Width), (int)(Skalierung * Objektvariante.Texturausschnitt.Height)), new Rectangle(Objektvariante.Texturausschnitt.X, Objektvariante.Texturausschnitt.Y, Objektvariante.Texturausschnitt.Width, Objektvariante.Texturausschnitt.Height), color);
        }
    }
}
