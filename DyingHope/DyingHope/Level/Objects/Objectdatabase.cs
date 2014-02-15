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
    enum ObjectClass
    {
        Nichts,
        Lampen,
        Autos,
        Vegetation,
        Plattformen,
        Kiste,
        Gebaeude,
        Stadtdekoration,
        Schule,
        Schule2,
        Moebel,
        Schlucht,
        Funpark,
        Grastexturen1,
        Grastexturen2,
        Grasbloecke,
        Grasstreifen,
        Tempel,
        Tempel2,
        Blaetter,
        Grasschraegen1,
        Grasschraegen2,
    }

    class Objectdatabase
    {
        public List<ObjectType> Objektdaten = new List<ObjectType>(); //Liste der verschieden Objektarten
        private ContentManager Content;

        public Objectdatabase(ContentManager content)
        {
            this.Content = content;
            LoadObjektdatenbank();
        }

        public void AddObjekttyp(ObjectType typ)
        {
            Objektdaten.Add(typ);
        }

        public void AddObjekttyp(ObjectClass typ, string texturpfad)
        {
            Objektdaten.Add(new ObjectType(typ, texturpfad));
        }

        private void LoadObjektdatenbank()
        {
            int Zeiger = 0;
            ObjectType TempObjekttyp = new ObjectType();
            Objectvariation TempObjektvariante = new Objectvariation(null);
            Objectcollision TempCollision = new Objectcollision(new Rectangle(0,0,0,0));

            if (File.Exists(@"Objects.xml"))
            {
                XmlReader reader = XmlReader.Create(@"Objects.xml");
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.LocalName)
                            {
                                case "NeuesObjekt": TempObjekttyp = new ObjectType(); Objektdaten.Add(TempObjekttyp); break;
                                case "Typ": Zeiger = 1; break;
                                case "Texturpfad": Zeiger = 2; break;
                                case "NeueVariation": TempObjektvariante = new Objectvariation(TempObjekttyp.Textur); TempObjekttyp.Variante.Add(TempObjektvariante); break;
                                case "Name": Zeiger = 3; break;
                                case "BearbeitungsverschiebungX": Zeiger = 4; break;
                                case "BearbeitungsverschiebungY": Zeiger = 5; break;
                                case "TexturausschnittX": Zeiger = 6; break;
                                case "TexturausschnittY": Zeiger = 7; break;
                                case "TexturausschnittBreite": Zeiger = 8; break;
                                case "TexturausschnittHöhe": Zeiger = 9; break;
                                case "Zerstörbar": Zeiger = 10; break;
                                case "Animiert": Zeiger = 11; break;
                                case "Spalten": Zeiger = 12; break;
                                case "Geschwindigkeit": Zeiger = 13; break;
                                case "Wiederholen": Zeiger = 14; break;
                                case "NeueKollision": TempCollision = new Objectcollision(new Rectangle(0,0,0,0)); break;
                                case "PosX": Zeiger = 15; break;
                                case "PosY": Zeiger = 16; break;
                                case "Width": Zeiger = 17; break;
                                case "Height": Zeiger = 18; break;
                                default: Zeiger = 0; break;
                            }
                            break;
                        case XmlNodeType.Text:
                            switch (Zeiger)
                            {
                                case 1: TempObjekttyp.ObjectClass = (ObjectClass)Enum.Parse(typeof(ObjectClass), reader.Value); break;
                                case 2: TempObjekttyp.Texturpfad = reader.Value; TempObjekttyp.Textur = Content.Load<Texture2D>(@"Objects\" + reader.Value); break;
                                case 3: TempObjektvariante.Name = reader.Value; break;
                                case 4: TempObjektvariante.Bearbeitungsverschiebung = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                                case 5: TempObjektvariante.Bearbeitungsverschiebung = new Vector2(TempObjektvariante.Bearbeitungsverschiebung.X, Convert.ToInt32(reader.Value)); break;
                                case 6: TempObjektvariante.Texturausschnitt = new Rectangle(Convert.ToInt32(reader.Value), 0, 0, 0); break;
                                case 7: TempObjektvariante.Texturausschnitt = new Rectangle(TempObjektvariante.Texturausschnitt.X, Convert.ToInt32(reader.Value), 0, 0); break;
                                case 8: TempObjektvariante.Texturausschnitt = new Rectangle(TempObjektvariante.Texturausschnitt.X, TempObjektvariante.Texturausschnitt.Y, Convert.ToInt32(reader.Value), 0); break;
                                case 9: TempObjektvariante.Texturausschnitt = new Rectangle(TempObjektvariante.Texturausschnitt.X, TempObjektvariante.Texturausschnitt.Y, TempObjektvariante.Texturausschnitt.Width, Convert.ToInt32(reader.Value)); break;
                                case 10: TempObjektvariante.Zerstörbar = Convert.ToBoolean(reader.Value); break;
                                case 11: TempObjektvariante.Animiert = Convert.ToBoolean(reader.Value); break;
                                case 12: TempObjektvariante.Spalten = Convert.ToInt32(reader.Value); break;
                                case 13: TempObjektvariante.Geschwindigkeit = Convert.ToInt32(reader.Value); break;
                                case 14: TempObjektvariante.Wiederholen = Convert.ToBoolean(reader.Value); break;
                                case 15: TempCollision.Zone.X = Convert.ToInt32(reader.Value); break;
                                case 16: TempCollision.Zone.Y = Convert.ToInt32(reader.Value); break;
                                case 17: TempCollision.Zone.Width = Convert.ToInt32(reader.Value); break;
                                case 18: TempCollision.Zone.Height = Convert.ToInt32(reader.Value); break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            switch (reader.LocalName)
                            {
                                case "NeueKollision": TempObjektvariante.Kollision.Add(TempCollision); break;
                            }
                            break;
                    }
                }
                reader.Close();
            }
        }

        //public void SaveObjektdatenbank()
        //{
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.Indent = true;
        //    settings.IndentChars = "  ";
        //    XmlWriter writer = XmlWriter.Create(@"Objects.xml", settings);
        //    writer.WriteStartDocument();
        //    writer.WriteStartElement("Objektdaten");
        //    for (int i = 0; i < Objektdaten.Count(); i++)
        //    {
        //        writer.WriteStartElement("NeuesObjekt");
        //        writer.WriteStartElement("Typ");
        //        writer.WriteValue(Objektdaten[i].ObjectClass.ToString());
        //        writer.WriteEndElement();
        //        writer.WriteStartElement("Texturpfad");
        //        writer.WriteValue(Objektdaten[i].Texturpfad);
        //        writer.WriteEndElement();
        //        for (int x = 0; x < Objektdaten[i].Variante.Count(); x++)
        //        {
        //            writer.WriteStartElement("NeueVariation");
        //            if (Objektdaten[i].Variante[x].Bearbeitungsverschiebung.X != 0)
        //            {
        //                writer.WriteStartElement("BearbeitungsverschiebungX");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Bearbeitungsverschiebung.X);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Bearbeitungsverschiebung.Y != 0)
        //            {
        //                writer.WriteStartElement("BearbeitungsverschiebungY");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Bearbeitungsverschiebung.Y);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Texturausschnitt.X != 0)
        //            {
        //                writer.WriteStartElement("TexturausschnittX");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Texturausschnitt.X);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Texturausschnitt.Y != 0)
        //            {
        //                writer.WriteStartElement("TexturausschnittY");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Texturausschnitt.Y);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Texturausschnitt.Width != 0)
        //            {
        //                writer.WriteStartElement("TexturausschnittBreite");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Texturausschnitt.Width);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Texturausschnitt.Height != 0)
        //            {
        //                writer.WriteStartElement("TexturausschnittHöhe");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Texturausschnitt.Height);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Zerstörbar != false)
        //            {
        //                writer.WriteStartElement("Zerstörbar");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Zerstörbar);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Animiert != false)
        //            {
        //                writer.WriteStartElement("Animiert");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Animiert);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Spalten != 0)
        //            {
        //                writer.WriteStartElement("Spalten");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Spalten);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Geschwindigkeit != 0)
        //            {
        //                writer.WriteStartElement("Geschwindigkeit");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Geschwindigkeit);
        //                writer.WriteEndElement();
        //            }
        //            if (Objektdaten[i].Variante[x].Wiederholen != false)
        //            {
        //                writer.WriteStartElement("Wiederholen");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Wiederholen);
        //                writer.WriteEndElement();
        //            }
        //            for (int y = 0; y < Objektdaten[i].Variante[x].Kollision.Count(); y++)
        //            {
        //                writer.WriteStartElement("NeueKollision");
        //                writer.WriteStartElement("PosX");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Kollision[y].Zone.X);
        //                writer.WriteEndElement();
        //                writer.WriteStartElement("PosY");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Kollision[y].Zone.Y);
        //                writer.WriteEndElement();
        //                writer.WriteStartElement("Width");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Kollision[y].Zone.Width);
        //                writer.WriteEndElement();
        //                writer.WriteStartElement("Height");
        //                writer.WriteValue(Objektdaten[i].Variante[x].Kollision[y].Zone.Height);
        //                writer.WriteEndElement();
        //                writer.WriteEndElement();
        //            }
        //            writer.WriteEndElement();   //Variation
        //        }
        //        writer.WriteEndElement();   //Objekt
        //    }
        //    writer.WriteEndElement();
        //    writer.WriteEndDocument();
        //    writer.Close();
        //}

        public ObjectType Auslesen(ObjectClass objectclass)
        {
            for (int i = 0; i < Objektdaten.Count(); i++)
            {
                if (Objektdaten[i].ObjectClass == objectclass)
                {
                    return Objektdaten[i];
                }
            }
            return Objektdaten[0];
        }

        public Objectvariation Auslesen(ObjectClass typ, int variante)
        {
            for (int i = 0; i < Objektdaten.Count(); i++)
            {
                if (Objektdaten[i].ObjectClass == typ)
                {
                    return Objektdaten[i].Variante[variante];
                }
            }
            return null;
            //return Objektdaten[0].Variante[variante];
        }
    }
}
