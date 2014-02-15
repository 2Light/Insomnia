using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    enum WindowElementType
    {
        Texture,
        Text,
        Eingabezelle,
    }

    class Windowelement
    {
        public WindowElementType Type;
        private Vector2 Position;
        public Rectangle Field;
        private Texture2D Texture;
        public string Text;
        private SpriteFont Font;
        public Color Color;
        private bool Zentriert;

        //Eingabezelle
        public int IDLesen;
        public int IDSchreiben;
        public bool Zahleneingabe;
        public int MaxStellen;
        public Color Zellenfarbe;
        public bool Anzeigen;   //Anzeige ausblenden während Eingabe
        public bool Mouseover;

        public Windowelement(Vector2 position, WindowElementType type, Rectangle quelltexturfeld, Texture2D textur) //Texturen
        {
            this.Position = position;
            this.Type = type;
            this.Field = quelltexturfeld;
            this.Texture = textur;
        }

        public Windowelement(Vector2 position, WindowElementType type, string text, SpriteFont font, Color color, bool zentriert) //Text
        {
            this.Position = position;
            this.Type = type;
            this.Text = text;
            this.Font = font;
            this.Color = color;
            this.Zentriert = zentriert;
        }

        public Windowelement(Rectangle feld, WindowElementType type, int lesen, int schreiben, Texture2D texture, SpriteFont font, Color textcolor, Color boxcolor, bool zahleneingabe, int maxstellen) //Eingabezelle
        {
            this.Field = feld;
            this.Type = type;
            this.IDLesen = lesen;
            this.IDSchreiben = schreiben;
            this.Texture = texture;
            this.Font = font;
            this.Color = textcolor;
            this.Zellenfarbe = boxcolor;
            this.Zahleneingabe = zahleneingabe;
            this.MaxStellen = maxstellen;
            this.Anzeigen = true;
            this.Text = "";
        }

        public void Draw(SpriteBatch spritebatch, Rectangle fenster)
        {
            switch (Type)
            {
                case WindowElementType.Texture: spritebatch.Draw(Texture, new Vector2(fenster.X + Position.X, fenster.Y + Position.Y), new Rectangle(Field.X, Field.Y, Field.Width, Field.Height), Color.White); break;
                case WindowElementType.Text:
                    if (Zentriert) TextZentriert(spritebatch, Text, Position + new Vector2(fenster.X, fenster.Y), Font, Color);
                    else spritebatch.DrawString(Font, Text, Position + new Vector2(fenster.X, fenster.Y), Color);
                    break;
                case WindowElementType.Eingabezelle: 
                    if (Mouseover) spritebatch.Draw(Texture, new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height), Color.Red);
                    else if (IDSchreiben == 0) spritebatch.Draw(Texture, new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height), Zellenfarbe);
                    else spritebatch.Draw(Texture, new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height), Color.LawnGreen);
                    if (Anzeigen) TextZentriert(spritebatch, Text, new Vector2(fenster.X + Field.X + (Field.Width / 2), fenster.Y + Field.Y), Font, Color);
                    break;
            }
        }

        public int Update(Rectangle fenster, Cursor cursor)
        {
            if ((Type == WindowElementType.Eingabezelle) && (IDSchreiben != 0))
            {
                if (new Rectangle(fenster.X + Field.X, fenster.Y + Field.Y, Field.Width, Field.Height).Intersects(new Rectangle((int)cursor.Mouseposition.X, (int)cursor.Mouseposition.Y, 1, 1)))
                {
                    Mouseover = true;
                    if (cursor.Leftclick == true) return IDSchreiben;   //ID des Befehls bei Linksklick zurückgeben
                }
                else Mouseover = false;
            }
            return 0;
        }

        private void TextZentriert(SpriteBatch spritebatch, string text, Vector2 position, SpriteFont font, Color color)
        {
            spritebatch.DrawString(font, text, new Vector2(position.X - (int)(font.MeasureString(text).X / 2), position.Y), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
