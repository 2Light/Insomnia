using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DyingHope
{
    class Keymanager : IdebugObjekt
    {
        public KeyboardState Tastatur;
        private Contents Contents;
        public List<Key> Tasten = new List<Key>();
        public List<int> Befehle = new List<int>();
        public string Texteingabe = "";
        public bool Eingabe = false;
        public Windowmanager Windowmanager;
        public Window FensterEingabezelle;
        public Windowelement Eingabezelle;
        private int BlinkTimer;
        private bool BlinkCursor;

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
        }

        public void handelDebug()
        {
           addString("Tasteneingabe: " + this.Eingabe.ToString());
           addString( "Eingabe: " + this.Texteingabe.ToString());
           addString("Tasten: " + this.Tasten.Count().ToString());
           if (this.Befehle.Count() > 0) addString("Key Befehl: " + this.Befehle[0].ToString());
        }

        #endregion

        public Keymanager(Contents contents,DebugFlag flag)   //Initialisieren des Tastaturobjekts und der Tasten
        {
            this.debugFlag = flag;
            this.stringBuilder = new StringBuilder();
            this.Contents = contents;
            Tastatur = new KeyboardState();
            TastenMenü();
            //TastenSpiel();
        }

        public void TastenMenü()
        {
            Tasten.Clear();
            Tasten.Add(new Key(11, 0, Keys.Escape));  //Spiel verlassen
            Tasten.Add(new Key(997, 0, Keys.F12));    //Editor
        }

        public void TastenSpiel()
        {
            Tasten.Clear();
            //Movement
            Tasten.Add(new Key(1, 1, Keys.Up));
            Tasten.Add(new Key(2, 2, Keys.Down));
            Tasten.Add(new Key(3, 3, Keys.Left));
            Tasten.Add(new Key(4, 4, Keys.Right));
            Tasten.Add(new Key(5, 0, Keys.LeftControl));
            Tasten.Add(new Key(6, 0, Keys.B));

            //Camera
            //Tasten.Add(new Key(5, 5, Keys.D));
            //Tasten.Add(new Key(6, 6, Keys.A));
            //Tasten.Add(new Key(7, 7, Keys.W));
            //Tasten.Add(new Key(8, 8, Keys.S));
            Tasten.Add(new Key(4, 4, Keys.D));
            Tasten.Add(new Key(3, 3, Keys.A));
            Tasten.Add(new Key(1, 1, Keys.W));
            Tasten.Add(new Key(2, 2, Keys.S));

            //Debug Toggle
            Tasten.Add(new Key(20, 0,  Keys.F1)); //Playerstats
            Tasten.Add(new Key(21, 0, Keys.F2)); // CollisonShape
            Tasten.Add(new Key(22, 0, Keys.F3)); // Objekt Information
            Tasten.Add(new Key(23, 0, Keys.F4)); // Objekt Manager Information
            Tasten.Add(new Key(24, 0, Keys.F5)); // Editor information
            Tasten.Add(new Key(25, 0, Keys.F6)); // Input Information
            Tasten.Add(new Key(26, 0, Keys.F7)); // Kamera Information
            Tasten.Add(new Key(27, 0, Keys.F8)); // Physik Debug
            Tasten.Add(new Key(28, 0, Keys.F9)); // BackgroundManager 
            Tasten.Add(new Key(29, 0, Keys.F9)); // WindowManager 

            //Depression
            Tasten.Add(new Key(40, 0, Keys.D1));
            Tasten.Add(new Key(41, 0, Keys.D2));
            Tasten.Add(new Key(42, 0, Keys.D3));
            Tasten.Add(new Key(43, 0, Keys.D4));
            Tasten.Add(new Key(44, 0, Keys.D5));
            Tasten.Add(new Key(45, 0, Keys.D6));
            Tasten.Add(new Key(46, 0, Keys.D7));

            Tasten.Add(new Key(11, 0, Keys.Escape));  //Hauptmenü
            Tasten.Add(new Key(997, 0, Keys.F12));    //Editor
            Tasten.Add(new Key(998, 0, Keys.F11));    //Toggle Debug
            Tasten.Add(new Key(999, 0, Keys.F10));    //Toggle Collision
        }

        public void TastenEditor()
        {
            Tasten.Clear();
            Tasten.Add(new Key(11, 0, Keys.Escape));  //Fenster schließen / Hauptmenü
            Tasten.Add(new Key(2000, 2000, Keys.Left));
            Tasten.Add(new Key(2001, 2001, Keys.Right));
            Tasten.Add(new Key(2002, 0, Keys.F12));   //Ingame wechseln
        }

        public void TastenTexteingabe()
        {
            Tasten.Clear();
            Tasten.Add(new Key(1, 0, Keys.Q));
            Tasten.Add(new Key(1, 0, Keys.W));
            Tasten.Add(new Key(1, 0, Keys.E));
            Tasten.Add(new Key(1, 0, Keys.R));
            Tasten.Add(new Key(1, 0, Keys.T));
            Tasten.Add(new Key(1, 0, Keys.Z));
            Tasten.Add(new Key(1, 0, Keys.U));
            Tasten.Add(new Key(1, 0, Keys.I));
            Tasten.Add(new Key(1, 0, Keys.O));
            Tasten.Add(new Key(1, 0, Keys.P));
            Tasten.Add(new Key(1, 0, Keys.A));
            Tasten.Add(new Key(1, 0, Keys.S));
            Tasten.Add(new Key(1, 0, Keys.D));
            Tasten.Add(new Key(1, 0, Keys.F));
            Tasten.Add(new Key(1, 0, Keys.G));
            Tasten.Add(new Key(1, 0, Keys.H));
            Tasten.Add(new Key(1, 0, Keys.J));
            Tasten.Add(new Key(1, 0, Keys.K));
            Tasten.Add(new Key(1, 0, Keys.L));
            Tasten.Add(new Key(1, 0, Keys.Y));
            Tasten.Add(new Key(1, 0, Keys.X));
            Tasten.Add(new Key(1, 0, Keys.C));
            Tasten.Add(new Key(1, 0, Keys.V));
            Tasten.Add(new Key(1, 0, Keys.B));
            Tasten.Add(new Key(1, 0, Keys.N));
            Tasten.Add(new Key(1, 0, Keys.M));
            Tasten.Add(new Key(2, 0, Keys.D1));
            Tasten.Add(new Key(2, 0, Keys.D2));
            Tasten.Add(new Key(2, 0, Keys.D3));
            Tasten.Add(new Key(2, 0, Keys.D4));
            Tasten.Add(new Key(2, 0, Keys.D5));
            Tasten.Add(new Key(2, 0, Keys.D6));
            Tasten.Add(new Key(2, 0, Keys.D7));
            Tasten.Add(new Key(2, 0, Keys.D8));
            Tasten.Add(new Key(2, 0, Keys.D9));
            Tasten.Add(new Key(2, 0, Keys.D0));
            Tasten.Add(new Key(3, 0, Keys.NumPad1));
            Tasten.Add(new Key(3, 0, Keys.NumPad2));
            Tasten.Add(new Key(3, 0, Keys.NumPad3));
            Tasten.Add(new Key(3, 0, Keys.NumPad4));
            Tasten.Add(new Key(3, 0, Keys.NumPad5));
            Tasten.Add(new Key(3, 0, Keys.NumPad6));
            Tasten.Add(new Key(3, 0, Keys.NumPad7));
            Tasten.Add(new Key(3, 0, Keys.NumPad8));
            Tasten.Add(new Key(3, 0, Keys.NumPad9));
            Tasten.Add(new Key(3, 0, Keys.NumPad0));
            Tasten.Add(new Key(4, 0, Keys.Back));
            Tasten.Add(new Key(5, 0, Keys.OemPeriod));
            Tasten.Add(new Key(6, 0, Keys.OemComma));
            Tasten.Add(new Key(7, 0, Keys.Space));
            Tasten.Add(new Key(8, 0, Keys.Enter));
            Tasten.Add(new Key(9, 0, Keys.Escape));
        }

        private void AddBefehl(int befehl)  //Neuen Tastaturbefehl hinzufügen
        {
            if (befehl != 0) Befehle.Add(befehl);
        }

        private void ClearBefehl()    //Alle Befehle löschen (jeden Frame)
        {
            Befehle.Clear();
        }

        public void NeueEingabe(Window window, int index)
        {
            if (Eingabe) EingabeAbbrechen();
            this.FensterEingabezelle = window;
            this.Eingabezelle = FensterEingabezelle.Elemente[index];
            Eingabezelle.Anzeigen = false;
            Eingabe = true;
            Texteingabe = "";
            TastenTexteingabe();
        }

        public void EingabeBeenden()
        {
            TastenEditor();
            Eingabezelle.Anzeigen = true;
            Eingabe = false;
            if (Eingabezelle.Zahleneingabe)
                foreach (char c in Texteingabe)
                {
                    //if ((!char.IsDigit(c)) && !(c.ToString() == ",")) return;
                    if (!char.IsDigit(c)) return;
                }
            if (Texteingabe == "") return;
            AddBefehl(Eingabezelle.IDSchreiben);
        }

        public void EingabeAbbrechen()
        {
            if (Eingabe)
            {
                Eingabe = false;
                Eingabezelle.Anzeigen = true;
                TastenEditor();
            }
        }

        public void Update()    //Zyklische Bearbeitung
        {
            Tastatur = Keyboard.GetState(); //Tastatur abfragen
            ClearBefehl();  //Liste leeren
            if (Eingabe)
            {
                if (!Windowmanager.IsWindowActive(FensterEingabezelle.Windowtype)) EingabeAbbrechen();  //Abbrechen wenn Fenster geschlossen
                BlinkTimer++;
                if (BlinkTimer == 10)
                {
                    BlinkCursor = !BlinkCursor;
                    BlinkTimer = 0;
                }
                for (int i = 0; i < Tasten.Count; i++) //Wenn Texteingabe gestartet Tasten aufnehmen
                {
                    switch (Tasten[i].Update(Tastatur.IsKeyDown(Tasten[i].Taste)))
                    {
                        case 1: if (Texteingabe.Length < Eingabezelle.MaxStellen) if (Tastatur.IsKeyDown(Keys.LeftShift)) Texteingabe += Tasten[i].Taste.ToString(); else Texteingabe += Tasten[i].Taste.ToString().ToLower(); break;
                        case 2: if (Texteingabe.Length < Eingabezelle.MaxStellen) Texteingabe += Tasten[i].Taste.ToString().Substring(1); break;
                        case 3: if (Texteingabe.Length < Eingabezelle.MaxStellen) Texteingabe += Tasten[i].Taste.ToString().Substring(6); break;
                        case 4: if (Texteingabe.Length > 0) Texteingabe = Texteingabe.Substring(0, Texteingabe.Length - 1); break;
                        case 5: if (Texteingabe.Length < Eingabezelle.MaxStellen) Texteingabe += "."; break;
                        case 6: if (Texteingabe.Length < Eingabezelle.MaxStellen) Texteingabe += ","; break;
                        case 7: if (Texteingabe.Length < Eingabezelle.MaxStellen) Texteingabe += " "; break;
                        case 8: EingabeBeenden(); break;
                        case 9: EingabeAbbrechen(); break;
                    }
                }
            }
            else for (int i = 0; i < Tasten.Count; i++) { AddBefehl(Tasten[i].Update(Tastatur.IsKeyDown(Tasten[i].Taste))); }  //Für jede existende Taste Zustand aktualisieren und evtl Befehl erstellen
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (Eingabe)
            {
                if (BlinkCursor)
                {
                    TextZentriert(spritebatch, Texteingabe, new Vector2(FensterEingabezelle.Feld.X + Eingabezelle.Field.X + (Eingabezelle.Field.Width / 2), FensterEingabezelle.Feld.Y + Eingabezelle.Field.Y + (Eingabezelle.Field.Height / 2)), Eingabezelle.Color);
                    if (Texteingabe.Length == 0) TextZentriert(spritebatch, "_", new Vector2(FensterEingabezelle.Feld.X + Eingabezelle.Field.X + (Eingabezelle.Field.Width / 2) + Contents.Meiryo8.MeasureString(Texteingabe).X, FensterEingabezelle.Feld.Y + Eingabezelle.Field.Y + (Eingabezelle.Field.Height / 2) - 1), Eingabezelle.Color);
                    else TextZentriert(spritebatch, "_", new Vector2(FensterEingabezelle.Feld.X + Eingabezelle.Field.X + (Eingabezelle.Field.Width / 2) + (Contents.Meiryo8.MeasureString(Texteingabe).X / 2) + 6, FensterEingabezelle.Feld.Y + Eingabezelle.Field.Y + (Eingabezelle.Field.Height / 2) - 1), Eingabezelle.Color);
                }
                else TextZentriert(spritebatch, Texteingabe, new Vector2(FensterEingabezelle.Feld.X + Eingabezelle.Field.X + (Eingabezelle.Field.Width / 2), FensterEingabezelle.Feld.Y + Eingabezelle.Field.Y + (Eingabezelle.Field.Height / 2)), Eingabezelle.Color);
                
            }
        }

        private void TextZentriert(SpriteBatch spritebatch, string text, Vector2 position, Color color)
        {
            spritebatch.DrawString(Contents.Meiryo8, text, new Vector2((int)position.X - (int)(Contents.Meiryo8.MeasureString(text).X / 2), (int)position.Y - (int)(Contents.Meiryo8.MeasureString(text).Y / 2)), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
