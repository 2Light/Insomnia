using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Window
    {
        public Contents Contents;
        public Windowtype Windowtype;   //Art des Fensters
        public Rectangle Feld;  //Position und Größe des Fensters
        public List<Windowtype> Schließen = new List<Windowtype>(); //Liste der zu schließenden Fenster
        public List<Button> Buttons = new List<Button>();   //Liste der Buttons des Fensters
        public List<Windowelement> Elemente = new List<Windowelement>();

        public Window(Contents contents, Windowtype type, Rectangle feld, List<Windowtype> schließen, List<Windowelement> elemente, List<Button> buttons)  //Erstellung eines Grundfenster
        {
            this.Contents = contents;
            this.Windowtype = type;
            this.Feld = feld;
            this.Schließen = schließen;
            this.Elemente = elemente;
            this.Buttons = buttons;
        }

        public Window(Window grundfenster)
        {
            this.Contents = grundfenster.Contents;
            this.Windowtype = grundfenster.Windowtype;
            this.Feld = grundfenster.Feld;
            foreach (Windowtype schließen in grundfenster.Schließen) this.Schließen.Add(schließen);
            foreach (Button button in grundfenster.Buttons) this.Buttons.Add(button);
            foreach (Windowelement element in grundfenster.Elemente) this.Elemente.Add(element);
        }

        public void AddButton(Button button)
        {
            Buttons.Add(button);
        }

        public void RemoveButton(Button button)
        {
            Buttons.Remove(button);
        }

        public int Update(Cursor mouse)
        {
            int Befehl = 0;
            int TempInt = 0;
            for (int i = 0; i < Buttons.Count; i++)
            {
                TempInt = Buttons[i].Update(Feld, mouse);
                if (TempInt != 0) Befehl = TempInt;
            }
            for (int i = 0; i < Elemente.Count(); i++)
            {
                TempInt = Elemente[i].Update(Feld, mouse);
                if (TempInt != 0) Befehl = TempInt;
            }
            return Befehl;
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font)
        {
            foreach (Windowelement Element in Elemente) Element.Draw(spritebatch, Feld);   //Texturen zeichnen
            for (int i = 0; i < Buttons.Count; i++) Buttons[i].Draw(Feld, spritebatch);   //Buttons zeichnen
            //spritebatch.DrawString(font, Windowtype.ToString(), new Vector2(Feld.X + 5, Feld.Y + 5), Color.Red);
        }
    }
}
