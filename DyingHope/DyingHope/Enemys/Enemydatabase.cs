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
    class Enemydatabase
    {
        public List<Enemy> Enemys = new List<Enemy>();
        public ContentManager Content;

        public Enemydatabase(ContentManager content)
        {
            this.Content = content;
            LoadEnemydatenbank();
        }

        private void LoadEnemydatenbank()
        {
            int Zeiger = 0;
            int Animation = 0;
            Enemy TempEnemy = new Enemy();

            if (File.Exists(@"Enemys.xml"))
            {
                XmlReader reader = XmlReader.Create(@"Enemys.xml");
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.LocalName)
                            {
                                case "NewEnemy": TempEnemy = new Enemy(); AddEnemy(TempEnemy); Animation = 0; break;
                                case "Typ": Zeiger = 1; break;
                                //case "TexturX": Zeiger = 2; break;
                                //case "TexturY": Zeiger = 3; break;
                                case "NewAnimation": Animation++; break;
                                case "TexturName": Zeiger = 4; break;
                                case "Spalten": Zeiger = 5; break;
                                case "Animationsgeschwindigkeit": Zeiger = 6; break;
                                case "Wiederholen": Zeiger = 7; break;
                                default: Zeiger = 0; break;
                            }
                            break;
                        case XmlNodeType.Text:
                            switch (Zeiger)
                            {
                                case 1: TempEnemy.Typ = (Enemytype)Enum.Parse(typeof(Enemytype), reader.Value); break;
                                //case 2: TempEnemy.Texturmaße.X = Convert.ToInt32(reader.Value); break;
                                //case 3: TempEnemy.Texturmaße.Y = Convert.ToInt32(reader.Value); break;
                                case 4: TempEnemy.Texturname[Animation - 1] = reader.Value; 
                                    TempEnemy.Textur[Animation - 1] = Content.Load<Texture2D>(@"Enemys\" + TempEnemy.Typ.ToString() + @"\" + reader.Value); break;
                                case 5: TempEnemy.Spalten[Animation - 1] = Convert.ToInt32(reader.Value);
                                    break;
                                case 6: TempEnemy.AniGeschwindigkeit[Animation - 1] = Convert.ToInt32(reader.Value); break;
                                case 7: TempEnemy.Wiederholen[Animation - 1] = Convert.ToBoolean(reader.Value); break;
                                default: break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            switch (reader.LocalName)
                            {
                                case "NewEnemy":
                                    TempEnemy.Texturmaße.X = TempEnemy.Textur[0].Width / (TempEnemy.Spalten[0] + 1);
                                    TempEnemy.Texturmaße.Y = TempEnemy.Textur[0].Height / 2;
                                    break;
                            }
                            break;
                    }
                }
            }
        }
       
        public Enemy Read(Enemytype typ)
        {
            for (int i = 0; i < Enemys.Count(); i++)
            {
                if (Enemys[i].Typ == typ) return Enemys[i];
            }
            return null;
        }

        private void AddEnemy(Enemy enemy)
        {
            Enemys.Add(enemy);
        }
    }
}
