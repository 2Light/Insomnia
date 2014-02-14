using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Windowmanager : IdebugObjekt
    {
        public Windowdatabase Windowdatabase;
        private Settings Settings;
        private Cursor Mouse;
        private Contents Contents;
        public int Befehl;
        public bool MouseOverGUI = false;
        public List<Window> Windows = new List<Window>();  //Liste der aktiven Userinterface Fenster
        public List<Windowelement> Eingabezellen = new List<Windowelement>();   //Liste der aktiven Eingabezellen
        private   SpriteBatch windowBatch;

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

           addString( "Fenster geoeffnet: " + this.Windows.Count().ToString());
           if (this.Windows.Count() != 0)
           {
               addString( "Buttons in Fenster 1: " + this.Windows[0].Buttons.Count().ToString());
               //addString( "Button 1 Zustand: " + this.Windows[0].Buttons[0].Mouseover.ToString());
               addString( "Fenster Feld: " + this.Windows[0].Feld.ToString());
               //addString( "Button 1 Feld: " + this.Windows[0].Buttons[0].Field.ToString());
           }
       }

       #endregion

        public Windowmanager(Settings settings, Cursor mouse,DebugFlag flag, Contents contents, GraphicsDevice graphicsDevice)
        {
            this.debugFlag = flag;
            this.stringBuilder = new StringBuilder();
            this.Settings = settings;
            this.Mouse = mouse;
            this.Contents = contents;
            this.windowBatch = new SpriteBatch(graphicsDevice);
        }

        public void AddWindow(Windowtype windowtype)    //Neues GUI Fenster erstellen
        {
            if (IsWindowActive(windowtype) == false)
            {
                Window NeuesFenster = Windowdatabase.GetWindowOfType(windowtype);
                foreach (Windowtype Fenstertyp in NeuesFenster.Schließen) RemoveWindow(Fenstertyp); //Fenster schließen (zB für Folgefenster: Questslog -> Questdetail)
                Windows.Add(new Window(NeuesFenster));    //Wenn Fenster nicht bereits offen
                foreach (Windowelement Element in NeuesFenster.Elemente)
                {
                    if (Element.Type == WindowElementType.Eingabezelle) Eingabezellen.Add(Element); //Eingabezellen für zyklisches lesen listen
                }
            }
        }

        public void AddTextCell(Windowtype windowtype, Windowelement eingabezelle, Vector2 position)  //Eingabezelle bestehendem Fenster hinzufügen
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Windowtype == windowtype)
                {
                    Windows[i].Elemente.Add(eingabezelle);  //Zelle dem Fenster hinzufügen
                    Eingabezellen.Add(eingabezelle);    //Zelle der Leseliste hinzufügen
                    break;
                }
            }
        }

        public void RemoveWindow(Windowtype windowtype) //Fenster schließen (löschen)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Windowtype == windowtype)
                {
                    foreach (Windowelement Element in Windows[i].Elemente)
                    {
                        if (Element.Type == WindowElementType.Eingabezelle) Eingabezellen.Remove(Element);  //Eingabezellen aus Leseliste entfernen
                    }
                    Windowdatabase.GetWindowOfType(windowtype).Feld = Windows[i].Feld; //Position speichern für Neuaufruf
                    Windows.Remove(Windows[i]);
                    break;
                }
            }
        }

        public void ClearWindows()  //Alle Fenster schließen und Zellen entfernen
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                foreach (Windowelement Element in Windows[i].Elemente)
                {
                    if (Element.Type == WindowElementType.Eingabezelle) Eingabezellen.Remove(Element);
                }
                Windowdatabase.GetWindowOfType(Windows[i].Windowtype).Feld = Windows[i].Feld; //Position speichern für Neuaufruf
                //Windows.Remove(Windows[i]);
            }
            Windows.Clear();
        }

        public bool IsWindowActive(Windowtype windowtype)   //Abfrage ob Fenster eines bestimmten Typs aktiv ist
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Windowtype == windowtype)
                {
                    return true;
                }
            }
            return false;
        }

        public Window GetWindowOfType(Windowtype windowtype) //Fenster eines bestimmten Typs abfragen
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Windowtype == windowtype)
                {
                    return Windows[i];
                }
            }
            return null;
        }

        public Rectangle GetWindowPosition(Windowtype windowtype) //Position eines nicht geöffneten Fenster abfragen (zum gleichsetzen bei Folgefenstern)
        {
            return Windowdatabase.GetWindowOfType(windowtype).Feld;
        }

        public int Update()
        {
            Befehl = 0; //Befehl zurücksetzen falls kein GUI Element mehr aktiv ist
            MouseOverGUI = false;
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                Befehl = Windows[i].Update(Mouse);
                if (new Rectangle((int)Mouse.Mouseposition.X, (int)Mouse.Mouseposition.Y, 1, 1).Intersects(Windows[i].Feld))
                {
                    MouseOverGUI = true;
                    if ((i != 0) && (Mouse.Leftclick)) //Angeklicktes Fenster nicht im Vordergrund
                    {
                        Window Zwischenspeicher = Windows[i];
                        for (int z = i; z < Windows.Count - 1; z++)
                        {
                            Windows[z] = Windows[z + 1];
                        }
                        Windows[Windows.Count - 1] = Zwischenspeicher;
                    }
                    return Befehl;
                }
            }
            return Befehl;  //Keinen Befehl ausführen
        }

        public void Draw(SpriteBatch spritbatch, SpriteFont font)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                Windows[i].Draw(spritbatch, font);
            }
        }
    }
}
