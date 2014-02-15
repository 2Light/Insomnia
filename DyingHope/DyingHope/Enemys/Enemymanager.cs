using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace DyingHope
{
    class Enemymanager
    {
        public List<Enemy> Enemys = new List<Enemy>();
        public List<Enemy> Delete = new List<Enemy>();
        private Objectmanager Objectmanager;
        public Enemydatabase Enemydatabase;
        private Mathe Mathe;
        private Player Player;
        private ContentManager ContentManager;

        public Enemymanager(Objectmanager objectmanager, Mathe mathe, ContentManager contentManager, Enemydatabase enemydatabase, Player player)
        {
            this.Objectmanager = objectmanager;
            this.Mathe = mathe;
            this.ContentManager = contentManager;
            this.Enemydatabase = enemydatabase;
            this.Player = player;
            //AddEnemy(Enemytype.Wolf, new Vector2(6300, 714), Richtung.Links, 1f);
            //AddEnemy(Enemytype.Skelett, new Vector2(900, 735), Richtung.Links, 1f);
        }

        public void Update(Gamestate gamestate, float elapsed)
        {
            foreach (Enemy enemy in Delete)
            {
                Enemys.Remove(enemy);
            }
            Delete.Clear();
            if (gamestate == Gamestate.Game)
            {
                foreach (Enemy enemy in Enemys)
                {
                    enemy.Update(elapsed, Player);
                }
            }
        }

        public void AddEnemy(Enemytype typ, Vector2 position, Richtung blickrichtung, float skalierung)
        {
            Enemys.Add(new Enemy(Objectmanager, ContentManager, Mathe, Enemydatabase, typ, position, blickrichtung, skalierung));
        }

        public void RemoveEnemy(Enemy enemy)
        {
            if (enemy != null)  //Fehlgeschlagene Auswahl via PickEnemy abfangen
            {
                Delete.Add(enemy);
            }
        }

        public Enemy PickEnemy(Vector2 position)    //Auswahl eines unbestimmten Gegners (Eventaktion)
        {
            foreach (Enemy enemy in Enemys)
            {
                if (Mathe.Distanzberechnung(new Vector2(enemy.PositionCurrent.X + (enemy.Texturmaße.X / 2), enemy.PositionCurrent.Y + (enemy.Texturmaße.Y / 2)), position) < 100)
                {
                    return enemy;
                }
            }
            return null;
        }

        public Enemy PickEnemy(Enemytype typ, Vector2 position)    //Auswahl eines unbestimmten Gegners (Eventaktion)
        {
            foreach (Enemy enemy in Enemys)
            {
                if ((enemy.Typ == typ) && (Mathe.Distanzberechnung(new Vector2(enemy.PositionCurrent.X + (enemy.Texturmaße.X / 2), enemy.PositionCurrent.Y + (enemy.Texturmaße.Y / 2)), position) < 100))
                {
                    return enemy;
                }
            }
            return null;
        }

        public void LoadEnemys(string filename)
        {
            int Zeiger = 0;
            Enemys.Clear();
            Enemy TempEnemy = new Enemy();
            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "NeuerGegner": TempEnemy = new Enemy(); break;
                            case "PositionX": Zeiger = 1; break;
                            case "PositionY": Zeiger = 2; break;
                            case "Typ": Zeiger = 3; break;
                            case "Skalierung": Zeiger = 4; break;
                            case "Blickrichtung": Zeiger = 5; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: TempEnemy.PositionCurrent = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                            case 2: TempEnemy.PositionCurrent = new Vector2(TempEnemy.PositionCurrent.X, Convert.ToInt32(reader.Value)); break;
                            case 3: TempEnemy.Typ = (Enemytype)Enum.Parse(typeof(Enemytype), reader.Value); break;
                            case 4: TempEnemy.Skalierung = 0f + (Convert.ToInt32(reader.Value) / 100f); break;
                            case 5: TempEnemy.Blickrichtung = (Richtung)Enum.Parse(typeof(Richtung), reader.Value); AddEnemy(TempEnemy.Typ, TempEnemy.PositionCurrent, TempEnemy.Blickrichtung, TempEnemy.Skalierung); break;
                            default: break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveEnemys(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Enemys");

            for (int i = 0; i < Enemys.Count; i++)
            {
                writer.WriteStartElement("NeuerGegner");
                writer.WriteStartElement("PositionX");
                writer.WriteValue(Math.Round(Enemys[i].PositionCurrent.X));
                writer.WriteEndElement();
                writer.WriteStartElement("PositionY");
                writer.WriteValue(Math.Round(Enemys[i].PositionCurrent.Y));
                writer.WriteEndElement();
                writer.WriteStartElement("Typ");
                writer.WriteValue(Enemys[i].Typ.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Skalierung");
                writer.WriteValue(Math.Round(Enemys[i].Skalierung * 100));
                writer.WriteEndElement();
                writer.WriteStartElement("Blickrichtung");
                writer.WriteValue(Enemys[i].Blickrichtung.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Enemys)
            {
                enemy.Draw(spriteBatch, Player);
            }
        }
    }
}
