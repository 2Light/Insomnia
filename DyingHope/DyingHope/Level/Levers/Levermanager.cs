using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using System.IO;
using System.Xml;

namespace DyingHope
{
    class Levermanager
    {
        public List<Lever> Levers = new List<Lever>();
        public Lever LeverInRange;
        private SpriteFont Font;
        private Player Player;
        private Cursor Cursor;
        private Mathe Mathe;
        private Animationsmanager Animationsmanager;
        private Contents Contents;
        private int InteractionRange = 100;

        public Levermanager(SpriteFont font, Player player, Cursor cursor, Mathe mathe, Animationsmanager animationsmanager, Contents contents)
        {
            this.Font = font;
            this.Player = player;
            this.Cursor = cursor;
            this.Mathe = mathe;
            this.Animationsmanager = animationsmanager;
            this.Contents = contents;
        }

        public void AddLever()
        {
            Levers.Add(new Lever(new Vector2(280, 201), Contents.Schalter));
        }

        public void Update()
        {
            LeverInRange = null;
            foreach (Lever lever in Levers)
            {
                if (Mathe.Distanzberechnung(lever.Position + lever.Mittelpunkt, new Vector2(Player.PositionCurrent.X + 640, Player.PositionCurrent.Y + Player.Kollision.Height)) < InteractionRange)
                {
                    LeverInRange = lever;
                    if (!(LeverInRange.Betätigt && LeverInRange.Einmalig)) lever.Update(Animationsmanager);
                }
            }
        }

        public void Use()
        {
            if ((LeverInRange != null) && !((LeverInRange.Betätigt) && (LeverInRange.Einmalig)))
            {
                Animationsmanager.AddAnimation(LeverInRange.Animation); //Animation zur Dauerbearbeitung übergeben
                Player.Statuswechsel(Playerstates.Lever);   //Schalter bewegen Animation starten
                Player.PositionCurrent = new Vector2(LeverInRange.Position.X + LeverInRange.Mittelpunkt.X - 640, LeverInRange.Position.Y + LeverInRange.Animation.Frame.Height - Player.Animation.Frame.Height);    //Spieler zum Schalter bewegen
                if (LeverInRange.Betätigt) Player.Blickrichtung = Richtung.Links; else Player.Blickrichtung = Richtung.Rechts;
                if (Player.Blickrichtung == Richtung.Links) Player.PositionCurrent.X += 35; else Player.PositionCurrent.X -= 35;
            }
        }

        public Lever GetLever(string name)
        {
            foreach (Lever lever in Levers)
            {
                if (lever.Name == name) return lever;
            }
            return null;
        }

        public Lever GetLever(Vector2 position)
        {
            foreach (Lever lever in Levers)
            {
                if (new Rectangle((int)(lever.Position.X - Player.PositionCurrent.X), (int)lever.Position.Y, (int)lever.Maße.X, (int)lever.Maße.Y).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))
                {
                    return lever;
                }
            }
            return null;
        }

        public void LoadLevers(string filename)
        {
            int Zeiger = 0;
            Levers.Clear();
            Lever TempLever = new Lever(new Vector2(280, 201), Contents.Schalter);
            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "NeuerSchalter": TempLever = new Lever(new Vector2(280, 201), Contents.Schalter); Levers.Add(TempLever); break;
                            case "Name": Zeiger = 1; break;
                            case "Einmalig": Zeiger = 2; break;
                            case "Rücksetzen": Zeiger = 3; break;
                            case "PositionX": Zeiger = 4; break;
                            case "PositionY": Zeiger = 5; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: TempLever.Name = reader.Value; break;
                            case 2: TempLever.Einmalig = Convert.ToBoolean(reader.Value); break;
                            case 3: TempLever.Rücksetzen = Convert.ToBoolean(reader.Value); break;
                            case 4: TempLever.Position = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                            case 5: TempLever.Position = new Vector2(TempLever.Position.X, Convert.ToInt32(reader.Value)); break;
                            default: break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveLevers(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Schalter");
            for (int i = 0; i < Levers.Count; i++)
            {
                writer.WriteStartElement("NeuerSchalter");
                  writer.WriteStartElement("Name");
                  writer.WriteValue(Levers[i].Name);
                  writer.WriteEndElement();
                  writer.WriteStartElement("Einmalig");
                  writer.WriteValue(Levers[i].Einmalig);
                  writer.WriteEndElement();
                  writer.WriteStartElement("Rücksetzen");
                  writer.WriteValue(Levers[i].Rücksetzen);
                  writer.WriteEndElement();
                  writer.WriteStartElement("PositionX");
                  writer.WriteValue(Math.Round(Levers[i].Position.X));
                  writer.WriteEndElement();
                  writer.WriteStartElement("PositionY");
                  writer.WriteValue(Math.Round(Levers[i].Position.Y));
                  writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Lever lever in Levers)
            {
                lever.Draw(spritebatch, Player);
            }
            if (LeverInRange != null) TextZentriert(spritebatch, Font, "Interagieren (Strg)", new Vector2(LeverInRange.Position.X + LeverInRange.Mittelpunkt.X - Player.PositionCurrent.X, LeverInRange.Position.Y + LeverInRange.Mittelpunkt.Y), Color.Red); 
        }

        private void TextZentriert(SpriteBatch spritebatch, SpriteFont spritefont, string text, Vector2 position, Color color)
        {
            spritebatch.DrawString(spritefont, text, new Vector2((int)position.X - (int)(spritefont.MeasureString(text).X / 2), (int)position.Y - (int)(spritefont.MeasureString(text).Y / 2)), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
