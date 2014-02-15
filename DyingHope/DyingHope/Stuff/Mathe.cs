using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    class Mathe
    {
        Random random = new Random();

        public bool Chancenberechnung(int wahrscheinlichkeit)
        {
            float Zufall = Zufallszahl((float)0.0, (float)100.0);    //Zufallszahl zur Chancenberechnung
            if (Zufall > wahrscheinlichkeit) return false;
            return true;
        }

        public bool ZahlImBereich(int untergrenze, int obergrenze, int zahl)
        {
            if ((zahl <= obergrenze) && (zahl >= untergrenze)) return true;
            else return false;
        }

        public float Distanzberechnung(Vector2 Punkt1, Vector2 Punkt2)    //Berechnung der Distanz zwischen 2 Vector2 Punkten (immer Positiv)
        {
            float Value1 = Math.Max((Punkt1.X), Punkt2.X);
            float Value2 = Math.Min((Punkt1.X), Punkt2.X);
            float DistanzX = Value1 - Value2;
            Value1 = Math.Max((Punkt1.Y), Punkt2.Y);
            Value2 = Math.Min((Punkt1.Y), Punkt2.Y);
            float DistanzY = Value1 - Value2;
            Value1 = DistanzX * DistanzX;
            Value2 = DistanzY * DistanzY;
            float Distanz = (float)Math.Sqrt((Value1 + Value2));

            return Distanz;
        }

        public bool PunktAufGeraden(Vector2 Startpunkt, Vector2 Endpunkt, Vector2 Prüfpunkt)
        {
            Vector2 VerhältnissGerade = Distanzverhältniss(Startpunkt, Endpunkt);
            if (Distanzverhältniss(Startpunkt, Prüfpunkt) == VerhältnissGerade) return true;
            return false;
        }

        public Vector2 Distanzverhältniss(Vector2 Punkt1, Vector2 Punkt2) //Berechnet das Verhältniss der X und Y Distanz zwischen 2 Punkten
        {
            int Value1 = Math.Max((int)(Punkt1.X), (int)Punkt2.X);
            int Value2 = Math.Min((int)(Punkt1.X), (int)Punkt2.X);
            int DistanzX = Value1 - Value2;
            Value1 = Math.Max((int)(Punkt1.Y), (int)Punkt2.Y);
            Value2 = Math.Min((int)(Punkt1.Y), (int)Punkt2.Y);
            int DistanzY = Value1 - Value2;

            return new Vector2(((float)1 / (DistanzX + DistanzY) * DistanzX), ((float)1 / (DistanzX + DistanzY) * DistanzY));
        }

        public double Winkelberechnung(int Ankathete, int Gegenkathete)     //Berechnung des Winkels Alpha bei unbekannter Hypotenuse
        {
            double Winkel = Math.Atan2(Gegenkathete, Ankathete);
            Winkel = Winkel * (180.0 / Math.PI);

            return Winkel;
        }

        public int Zufallszahl(int min, int max)    //Erstellen eines zufälligen Integers
        {
            return random.Next(min, max);
        }

        public float Zufallszahl(float min, float max)    //Erstellen eines zufälligen Floats
        {
            return (float)MathHelper.Lerp(min, max, (float)random.NextDouble());
        }

        public int Differenz(int zahl1, int zahl2)
        {
            return (Math.Max(zahl1, zahl2) - Math.Min(zahl1, zahl2));
        }

        public float Differenz(float zahl1, float zahl2)
        {
            return (Math.Max(zahl1, zahl2) - Math.Min(zahl1, zahl2));
        }
    }
}
