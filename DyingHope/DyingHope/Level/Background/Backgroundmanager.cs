using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace DyingHope
{
    class Backgroundmanager : IdebugObjekt
    {
        public Editor Editor;
        public List<Background> Backgrounds = new List<Background>();
        private Backgrounddatabase Backgrounddatabase;
        Player Player;
        Contents Contents;

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

        public void DrawExtraDebug(DebugFlag extraFlag, SpriteBatch batch, Camera cam, Contents content) { }

        public void handelDebug()
        {
           //addString( "Background1 Pos. " + this.Backgrounds[0].Position.ToString());
           //addString( "Background2 Pos. " + this.Backgrounds[1].Position.ToString());
           //addString( "Background3 Pos. " + this.Backgrounds[2].Position.ToString());
           //addString("Background1 X " + (this.Backgrounds[0].Texture.Width - this.Backgrounds[0].Position.X).ToString());
           //addString( "Background2 X " + (this.Backgrounds[1].Texture.Width - this.Backgrounds[1].Position.X).ToString());
           //addString( "Background3 X " + (this.Backgrounds[2].Texture.Width - this.Backgrounds[2].Position.X).ToString());
                   
        }

        #endregion

        public Backgroundmanager(Player player, Backgrounddatabase backgrounddatabase, DebugFlag flag ,Contents contents)
        {
            this.Backgrounddatabase = backgrounddatabase;
            this.debugFlag = flag;
            this.stringBuilder = new StringBuilder();
            this.Contents = contents;
            this.Player = player;
        }

        public void LoadBackgrounds(string filename)
        {
            int Zeiger = 0;
            Backgrounds.Clear();
            Background TempBackground = new Background();

            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "NewBackground": TempBackground = new Background(); break;
                            case "Textur": Zeiger = 1; break;
                            case "ScrollX": Zeiger = 2; break;
                            case "ScrollY": Zeiger = 3; break;
                            case "Transparenz": Zeiger = 4; break;
                            case "VersatzY": Zeiger = 5; break;
                            case "Startposition": Zeiger = 6; break;
                            case "Endposition": Zeiger = 7; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: TempBackground.Name = reader.Value; break;
                            case 2: TempBackground.Scrollgeschwindigkeit = new Vector2(0f + (Convert.ToInt32(reader.Value) / 100f), 0); break;
                            case 3: TempBackground.Scrollgeschwindigkeit = new Vector2(TempBackground.Scrollgeschwindigkeit.X, 0f + (Convert.ToInt32(reader.Value) / 100f)); break;
                            case 4: TempBackground.Transparenz = Convert.ToInt32(reader.Value) / 100; break;
                            case 5: TempBackground.versatzY = Convert.ToInt32(reader.Value); break;
                            case 6: TempBackground.Startposition = Convert.ToInt32(reader.Value); break;
                            case 7: TempBackground.Endposition = Convert.ToInt32(reader.Value); break;
                            default: break;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        switch (reader.LocalName)
                        {
                            case "NewBackground": Backgrounds.Add(new Background(TempBackground.Name, TempBackground.Scrollgeschwindigkeit, TempBackground.Transparenz, TempBackground.versatzY, TempBackground.Startposition, TempBackground.Endposition, Contents.Contentmanager)); break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveBackgrounds(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Backgrounds");

            for (int i = 0; i < Backgrounds.Count; i++)
            {
                writer.WriteStartElement("NewBackground");
                    writer.WriteStartElement("Textur");
                    writer.WriteValue(Backgrounds[i].Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("ScrollX");
                    writer.WriteValue((Backgrounds[i].Scrollgeschwindigkeit.X * 100).ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("ScrollY");
                    writer.WriteValue((Backgrounds[i].Scrollgeschwindigkeit.Y * 100).ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Transparenz");
                    writer.WriteValue((Backgrounds[i].Transparenz * 100).ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("VersatzY");
                    writer.WriteValue(Backgrounds[i].versatzY.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Startposition");
                    writer.WriteValue(Backgrounds[i].Startposition.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Endposition");
                    writer.WriteValue(Backgrounds[i].Endposition.ToString());
                    writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Update()
        {
            foreach (Background backround in Backgrounds)
            {
                backround.Update(Player.PositionCurrent);
            }
        }

        public void Draw(Gamestate gamestate, SpriteBatch spriteBatch, GraphicsDevice device)
        {
            if (gamestate == Gamestate.Game)
            {
                foreach (Background background in Backgrounds)
                {
                    background.Draw(spriteBatch, device, Color.White, Player.PositionCurrent);
                }
            }
            else if (gamestate == Gamestate.Editor)
            {
                foreach (Background background in Backgrounds)
                {
                    if (background == Editor.AuswahlHintergrund) background.Draw(spriteBatch, device, new Color(255, 60, 60), Player.PositionCurrent);
                    else background.Draw(spriteBatch, device, Color.White, Player.PositionCurrent);
                }
            }
        }
    }
}
