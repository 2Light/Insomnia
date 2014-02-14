using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Settings
    {
        private GraphicsDeviceManager graphics;
        public bool Music;
        public bool Sound;
        public bool Fullscreen;
        public bool Widescreen;
        public bool ModeSupported;

        public Settings(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            LoadConfig();

            foreach (DisplayMode mode in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                if ((mode.Width == 1280) && (mode.Height == 1024)) ModeSupported = true;
            }
            if (ModeSupported)
            {
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 1024;
            }
            else
            {
                if (graphics.GraphicsDevice.Adapter.IsWideScreen) Widescreen = true;
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 720;
            }
            if (Fullscreen) graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            //if (Fullscreen)
            //{
                //if (graphics.GraphicsDevice.Adapter.IsWideScreen) Widescreen = true;
                //if (Widescreen == true)
                //{
                //    graphics.PreferredBackBufferWidth = 1280;
                //    graphics.PreferredBackBufferHeight = 720;
                //    graphics.ApplyChanges();
                //}
                //else
                //{
                //    graphics.PreferredBackBufferWidth = 1280;
                //    graphics.PreferredBackBufferHeight = 1024;
                //    graphics.ApplyChanges();
                //}
                //graphics.IsFullScreen = true;
                //graphics.ApplyChanges();
            //}
        }

        public void LoadConfig()    //Config aus Datei laden
        {
            int Zeiger = 0;

            XmlReader reader = XmlReader.Create(@"Settings.xml");

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "Music": Zeiger = 1; ; break;
                            case "Sound": Zeiger = 2; ; break;
                            case "Fullscreen": Zeiger = 3; ; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: if (reader.Value == "false") Music = false; else Music = true; break;
                            case 2: if (reader.Value == "false") Sound = false; else Sound = true; break;
                            case 3: if (reader.Value == "false") Fullscreen = false; else Fullscreen = true; break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveConfig()    //Config in Datei schreiben (bei Exit)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(@"Settings.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");
            writer.WriteStartElement("Music");
            writer.WriteValue(Music);
            writer.WriteEndElement();
            writer.WriteStartElement("Sound");
            writer.WriteValue(Sound);
            writer.WriteEndElement();
            writer.WriteStartElement("Fullscreen");
            writer.WriteValue(Fullscreen);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
