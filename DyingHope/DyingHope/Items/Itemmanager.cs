using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.IO;

namespace DyingHope
{
    class Itemmanager
    {
        public List<Item> Datenbank = new List<Item>(); //Liste der Existenden Items für Editor
        public List<Item> Items = new List<Item>();
        public List<Item> Delete = new List<Item>();
        private Mathe Mathe;
        private DepressionHandler DepressionHandler;
        private Player Player;
        private ContentManager ContentManager;
        private string Quellpfad = @"Content\Items\";

        public Itemmanager(Mathe mathe, DepressionHandler depressionHandler, ContentManager contentManager, Player player)
        {
            this.Mathe = mathe;
            this.DepressionHandler = depressionHandler;
            this.ContentManager = contentManager;
            this.Player = player;

            if (System.IO.Directory.Exists(Quellpfad))
            {
                string[] folders = System.IO.Directory.GetFiles(Quellpfad);
                foreach (string s in folders)
                {
                    Datenbank.Add(new Item(ContentManager, s.Substring(14, s.Length - 18)));
                }
            }
        }

        public void Update(Gamestate gamestate, float elapsed)
        {
            if (gamestate == Gamestate.Game)
            {
                foreach (Item item in Items)
                {
                    if (item.Update()) Delete.Add(item);
                }
            }
            foreach (Item item in Delete)
            {
                Items.Remove(item);
            }
            Delete.Clear();
        }

        public void AddItem(string name, Vector2 position, bool kuriert)
        {
            Item TempItem = new Item(Player);
            foreach (Item item in Datenbank)
            {
                if (item.Name == name)
                {
                    TempItem = item;
                    break;
                }
            }
            Items.Add(new Item(Player, DepressionHandler, name, TempItem.Textur, position, kuriert));
        }

        public void RemoveItem(Item item)
        {
            Delete.Add(item);
        }

        public void LoadItems(string filename)
        {
            int Zeiger = 0;
            Items.Clear();
            Item TempItem = new Item(Player);
            if (File.Exists(filename))
            {
                XmlReader reader = XmlReader.Create(filename);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.LocalName)
                            {
                                case "NeuesItem": TempItem = new Item(Player); break;
                                case "Name": Zeiger = 1; break;
                                case "PositionX": Zeiger = 2; break;
                                case "PositionY": Zeiger = 3; break;
                                case "Kuriert": Zeiger = 4; break;
                                default: Zeiger = 0; break;
                            }
                            break;
                        case XmlNodeType.Text:
                            switch (Zeiger)
                            {
                                case 1: TempItem.Name = reader.Value; break;
                                case 2: TempItem.Position = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                                case 3: TempItem.Position = new Vector2(TempItem.Position.X, Convert.ToInt32(reader.Value)); break;
                                case 4: TempItem.Kuriert = Convert.ToBoolean(reader.Value); AddItem(TempItem.Name, TempItem.Position, TempItem.Kuriert); break;
                                default: break;
                            }
                            break;
                    }
                }
                reader.Close();
            }
        }

        public void SaveItems(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Items");

            for (int i = 0; i < Items.Count; i++)
            {
                writer.WriteStartElement("NeuesItem");
                writer.WriteStartElement("Name");
                writer.WriteValue(Items[i].Name);
                writer.WriteEndElement();
                writer.WriteStartElement("PositionX");
                writer.WriteValue(Math.Round(Items[i].Position.X));
                writer.WriteEndElement();
                writer.WriteStartElement("PositionY");
                writer.WriteValue(Math.Round(Items[i].Position.Y));
                writer.WriteEndElement();
                writer.WriteStartElement("Kuriert");
                writer.WriteValue(Items[i].Kuriert.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Item item in Items)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}
