using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace DyingHope
{
    class Level
    {
        public string Name;
        public Vector2 Startposition;
        public float Helligkeit;
        public float Walkline = 715;
        public float DepressionRate;
        public int VordergrundAbdunkelung = 255;
        //Wetter
        //Musik

        public Level(string name)
        {
            this.Name = name;
        }

        public void LoadLevel(string filename)
        {
            int TempInt = 0;
            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "PositionX": TempInt = 1; break;
                            case "PositionY": TempInt = 2; break;
                            case "Helligkeit": TempInt = 3; break;
                            case "Walkline": TempInt = 4; break;
                            case "DepressionRate": TempInt = 5; break;
                            case "VordergrundAbdunkelung": TempInt = 6; break;
                            default: TempInt = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (TempInt)
                        {
                            case 1: Startposition = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                            case 2: Startposition = new Vector2(Startposition.X, Convert.ToInt32(reader.Value)); break;
                            case 3: Helligkeit = Convert.ToInt32(reader.Value); break;
                            case 4: Walkline = Convert.ToInt32(reader.Value); break;
                            case 5: DepressionRate = 0f + (Convert.ToInt32(reader.Value) / 100f); break;
                            case 6: VordergrundAbdunkelung = Convert.ToInt32(reader.Value); break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveLevel(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Level");
                writer.WriteStartElement("PositionX");
                writer.WriteValue(Math.Round(Startposition.X).ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("PositionY");
                writer.WriteValue(Math.Round(Startposition.Y).ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Helligkeit");
                writer.WriteValue(Math.Round(Helligkeit).ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Walkline");
                writer.WriteValue(Math.Round(Walkline).ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("DepressionRate");
                writer.WriteValue(Math.Round(DepressionRate * 100).ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("VordergrundAbdunkelung");
                writer.WriteValue(VordergrundAbdunkelung.ToString());
                writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
