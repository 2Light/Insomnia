using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Button
    {
        public Rectangle Field;   //Position und Maße des Buttons
        private Texture2D Textur;
        private Rectangle Dateiauschnitt;
        private Rectangle DateiausschnittMouseover;
        Color Buttoncolor;
        public List<string> Textzeilen = new List<string>();
        public bool Zentriert;
        Color Textcolor;
        Color MouseoverColor;
        SpriteFont Font;
        bool Eindrücken;   //Button wird bei Betätigung eingedrückt
        private bool Eingedrückt;
        public bool Mouseover;
        int Mouseoverbefehl;     //ID der Funktion bei Mouseover
        int Linksklickbefehl;     //ID der Funktion bei Linksklick
        int Rechtsklickbefehl;    //ID der Funktion bei Rechtsklick

        public Button(Rectangle field, int leftclick, int rightclick, int mouseover)    //Konstruktor Button ohne Grafik
        {
            this.Field = field;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
        }

        public Button(Rectangle field, SpriteFont font, string text, bool zentriert, Color textcolor, int leftclick, int rightclick, int mouseover)    //Konstruktor Button mit Text
        {
            this.Field = field;
            this.Font = font;
            this.Textzeilen.Add(text);
            this.Zentriert = zentriert;
            this.Textcolor = textcolor;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
        }

        public Button(Rectangle field, SpriteFont font, List<string> textzeilen, Color textcolor, int leftclick, int rightclick, int mouseover)    //Konstruktor Button mit Textblock
        {
            this.Field = field;
            this.Font = font;
            this.Textzeilen = textzeilen;
            this.Textcolor = textcolor;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
        }

        public Button(Rectangle field, Texture2D textur, Rectangle dateiausschnitt, Color buttoncolor, int leftclick, int rightclick, int mouseover, bool eindrücken)    //Konstruktor Button mit Grafik (Dateiausschnitt ohne Mouseovervariante)
        {
            this.Field = field;
            this.Textur = textur;
            this.Dateiauschnitt = dateiausschnitt;
            this.DateiausschnittMouseover = dateiausschnitt;
            this.Buttoncolor = buttoncolor;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
            this.Eindrücken = eindrücken;
        }

        public Button(Rectangle field, Texture2D textur, Rectangle dateiausschnitt, Rectangle dateiausschnittmouseover, Color buttoncolor, int leftclick, int rightclick, int mouseover, bool eindrücken)    //Konstruktor Button mit Grafik (Dateiausschnitt mit Mouseovervariante)
        {
            this.Field = field;
            this.Textur = textur;
            this.Dateiauschnitt = dateiausschnitt;
            this.DateiausschnittMouseover = dateiausschnittmouseover;
            this.Buttoncolor = buttoncolor;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
            this.Eindrücken = eindrücken;
        }

        public Button(Rectangle field, SpriteFont font, string text, Color textcolor, Texture2D textur, Rectangle dateiausschnitt, Rectangle dateiausschnittmouseover, Color buttoncolor, int leftclick, int rightclick, int mouseover, bool eindrücken)    //Konstruktor Button mit Grafik (Dateiausschnitt mit Mouseovervariante und Textinhalt)
        {
            this.Field = field;
            this.Font = font;
            this.Textzeilen.Add(text);
            this.Zentriert = true;
            this.Textcolor = textcolor;
            this.Textur = textur;
            this.Dateiauschnitt = dateiausschnitt;
            this.DateiausschnittMouseover = dateiausschnittmouseover;
            this.Buttoncolor = buttoncolor;
            this.Linksklickbefehl = leftclick;
            this.Rechtsklickbefehl = rightclick;
            this.Mouseoverbefehl = mouseover;
            this.Eindrücken = eindrücken;
        }

        public int Update(Rectangle fenster, Cursor mouse)
        {
            Eingedrückt = false;    //Eingedrückte Buttons zurücksetzen
            if (new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height).Intersects(new Rectangle((int)mouse.Mouseposition.X, (int)mouse.Mouseposition.Y, 1, 1)))
            {
                Mouseover = true;
                if (mouse.Leftclick == true)
                {
                    if (Eindrücken == true) Eingedrückt = true; //Button eindrücken wenn aktiviert
                    return Linksklickbefehl;   //ID des Befehls bei Linksklick zurückgeben
                }
                if (mouse.Rightclick == true)
                {
                    return Rechtsklickbefehl;   //ID des Befehls bei Rechtsklick zurückgeben
                }
                return Mouseoverbefehl;   //ID des Befehls bei Mouseover zurückgeben
            }
            else Mouseover = false;
            return 0;
        }

        public void Draw(Rectangle fenster, SpriteBatch spritebatch)
        {
            if (Textur != null)
            {
                if (Eingedrückt) spritebatch.Draw(Textur, new Rectangle(fenster.X + Field.X + 1, fenster.Y + Field.Y + 1, Field.Width, Field.Height), DateiausschnittMouseover, Buttoncolor);   //Button eingedrückt zeichnen
                else if (Mouseover) spritebatch.Draw(Textur, new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height), DateiausschnittMouseover, Buttoncolor);   //Button Mouseovervariante zeichnen
                else spritebatch.Draw(Textur, new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height), Dateiauschnitt, Buttoncolor);   //Button normal zeichnen
            }
            if (Textzeilen.Count() > 0) //Button hat Beschriftung die nicht in Textur enthalten ist
            {
                foreach (string Zeile in Textzeilen)
                {
                    if (Zentriert == false)
                    {
                        MouseoverColor = Color.Red; if (Textcolor == MouseoverColor) MouseoverColor = Color.DarkRed;
                        if (Mouseover == true) spritebatch.DrawString(Font, Zeile, new Vector2(fenster.X + Field.X, fenster.Y + Field.Y + (Textzeilen.IndexOf(Zeile) * 13)), MouseoverColor);
                        else spritebatch.DrawString(Font, Zeile, new Vector2(fenster.X + Field.X, fenster.Y + Field.Y + (Textzeilen.IndexOf(Zeile) * 13)), Textcolor);
                    }
                    else
                    {
                        MouseoverColor = Color.Red; if (Textcolor == MouseoverColor) MouseoverColor = Color.DarkRed;
                        if (Mouseover == true) TextZentriert(spritebatch, Zeile, new Vector2(fenster.X + Field.X + (Field.Width / 2), fenster.Y + Field.Y + (Field.Height / 2)), MouseoverColor);
                        else TextZentriert(spritebatch, Zeile, new Vector2(fenster.X + Field.X + (Field.Width / 2), fenster.Y + Field.Y + (Field.Height / 2)), Textcolor);
                    }
                }
            }
        }

        private void TextZentriert(SpriteBatch spritebatch, string text, Vector2 position, Color color)
        {
            spritebatch.DrawString(Font, text, new Vector2((int)position.X - (int)(Font.MeasureString(text).X / 2), (int)position.Y - (int)(Font.MeasureString(text).Y / 2)), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
