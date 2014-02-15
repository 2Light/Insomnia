using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DyingHope
{
    class Contents
    {
        public ContentManager Contentmanager;
        private GraphicsDevice GraphicDevice;

        //Spritefonts---------------------------------------------------------------------------------------
        public SpriteFont Meiryo8;

        //Sound---------------------------------------------------------------------------------------------
        
        public SoundEffect Anfang;

        public SoundEffect StadtAnfang;
        public SoundEffect StadtLoop;
        public SoundEffect StadtEnde;

        public SoundEffect ParkAnfang;
        public SoundEffect ParkLoop;
        public SoundEffect ParkEnde;

        public SoundEffect SchuleAnfang;
        public SoundEffect SchuleLoop;

        public SoundEffect MonsterAnfang;
        public SoundEffect MonsterLoop;

        public SoundEffect Ende;


        public SoundEffect Menue_Theme;

        public SoundEffect Light;
        public SoundEffect Rain;

        //Backgrounds---------------------------------------------------------------------------------------
        public Texture2D Menue;
        public Texture2D Background1;
        public Texture2D Background2;
        public Texture2D Background3;
        public Texture2D Background4;
        public Texture2D Background5;
        public Texture2D Asphalt;

        //Spielersprites------------------------------------------------------------------------------------
        public Texture2D Player1;
        public Texture2D Player2;
        public Texture2D Player3;
        public Texture2D Player4;
        public Texture2D Player5;
        public Texture2D Player6;

        //Partikel------------------------------------------------------------------------------------------
        public Texture2D Emitter;
        public Texture2D Partikel1;
        public Texture2D Partikel2;
        public Texture2D Partikel3;
        public Texture2D Partikel4;
        public Texture2D Nebel;
        public Texture2D Nebel2;
        public Texture2D Rain1;

        //Schalter------------------------------------------------------------------------------------------
        public Texture2D Schalter;

        //UI------------------------------------------------------------------------------------------------
        public Texture2D Window;
        public Texture2D EventWindow;
        public Texture2D GUIParts;
        public Texture2D Bar;
        public Texture2D Maus;
        public Texture2D MenueButtons;

        //Platzhalter---------------------------------------------------------------------------------------
        public Texture2D Objekt;
        public Texture2D Block;
        public Texture2D Maske;

        //Dummy Texuren-------------------------------------------------------------------------------------
        public Texture2D Pixel;

        //Shader---------------------------------------------------------------------------------------------
        public Effect GrayScale;
        public Effect Invert;
        public Effect InvertHori;
        public Effect FOV;
        public Effect Noise;
        public Effect Glitchy;

        public Contents(ContentManager contentmanager, GraphicsDevice graphicsdevice)
        {
            this.Contentmanager = contentmanager;
            this.GraphicDevice = graphicsdevice;

            //Spritefonts---------------------------------------------------------------------------------------
            Meiryo8 = contentmanager.Load<SpriteFont>("Meiryo8");

            //Sound--------------------------------------------------------------------------------------------
            Menue_Theme = contentmanager.Load<SoundEffect>(@"Musik\Pressure");

            StadtAnfang = contentmanager.Load<SoundEffect>(@"Musik\1-2(Stadt) Anfang");
            StadtLoop = contentmanager.Load<SoundEffect>(@"Musik\1-2(Stadt) Loop"); ;
            StadtEnde = contentmanager.Load<SoundEffect>(@"Musik\1-2(Stadt) Ende"); ;

            ParkAnfang = contentmanager.Load<SoundEffect>(@"Musik\1-3(Park) Anfang"); ;
            ParkLoop = contentmanager.Load<SoundEffect>(@"Musik\1-3(Park) Loop"); ;
            ParkEnde = contentmanager.Load<SoundEffect>(@"Musik\1-3(Park) Ende"); ;

            SchuleAnfang = contentmanager.Load<SoundEffect>(@"Musik\1-4(Schule) Anfang"); ;
            SchuleLoop = contentmanager.Load<SoundEffect>(@"Musik\1-4(Schule) Loop"); ;

            MonsterAnfang = contentmanager.Load<SoundEffect>(@"Musik\1-5(Monster) Anfang"); ;
            MonsterLoop = contentmanager.Load<SoundEffect>(@"Musik\1-5(Monster) Anfang"); ;

            Ende = contentmanager.Load<SoundEffect>(@"Musik\1-6(Ende)"); ;

            Light = contentmanager.Load<SoundEffect>(@"Sound Effects\light");
            Rain = contentmanager.Load<SoundEffect>(@"Sound Effects\rain");

            //Backgrounds---------------------------------------------------------------------------------------
            Menue = contentmanager.Load<Texture2D>(@"Backgrounds\Menue");
            //Background1 = contentmanager.Load<Texture2D>(@"Backgrounds\Seamless Backround");
            //Background1.Name = "Seamless Backround";
            //Background2 = contentmanager.Load<Texture2D>(@"Backgrounds\Baum1_Set");
            //Background2.Name = "Baum1_Set";
            //Background3 = contentmanager.Load<Texture2D>(@"Backgrounds\Baum2_Set");
            //Background3.Name = "Baum2_Set";
            //Background4 = contentmanager.Load<Texture2D>(@"Backgrounds\Baum3_Set");
            //Background4.Name = "Baum3_Set";
            //Background5 = contentmanager.Load<Texture2D>(@"Backgrounds\ground");
            //Background5.Name = "ground";
            Asphalt = contentmanager.Load<Texture2D>(@"Backgrounds\Asphalt");
            Asphalt.Name = "Asphalt";

            //Spielersprites------------------------------------------------------------------------------------
            Player1 = contentmanager.Load<Texture2D>(@"Player\Idle");
            Player2 = contentmanager.Load<Texture2D>(@"Player\Walk");
            Player3 = contentmanager.Load<Texture2D>(@"Player\Jump");
            Player4 = contentmanager.Load<Texture2D>(@"Player\Climb");
            Player5 = contentmanager.Load<Texture2D>(@"Player\Climb2");
            Player6 = contentmanager.Load<Texture2D>(@"Player\Lever");

            //Partikel------------------------------------------------------------------------------------------
            Emitter = contentmanager.Load<Texture2D>(@"Partikel\box");
            Partikel1 = contentmanager.Load<Texture2D>(@"Partikel\p1");
            Partikel2 = contentmanager.Load<Texture2D>(@"Partikel\p2");
            Partikel3 = contentmanager.Load<Texture2D>(@"Partikel\p3");
            Partikel4 = contentmanager.Load<Texture2D>(@"Partikel\particel");
            Nebel = contentmanager.Load<Texture2D>(@"Partikel\Nebel");
            Nebel2 = contentmanager.Load<Texture2D>(@"Partikel\Nebel2");
            Rain1 = contentmanager.Load<Texture2D>(@"Partikel\Regentropfen");
            
            //Schalter------------------------------------------------------------------------------------------
            Schalter = contentmanager.Load<Texture2D>(@"Objects\Schalter");

            //UI------------------------------------------------------------------------------------------------
            Window = contentmanager.Load<Texture2D>(@"UI\Window");
            EventWindow = contentmanager.Load<Texture2D>(@"UI\EventWindow");
            GUIParts = contentmanager.Load<Texture2D>(@"UI\GUIParts");
            Bar = contentmanager.Load<Texture2D>(@"UI\Bar");
            Maus = contentmanager.Load<Texture2D>(@"UI\Maus");
            MenueButtons = contentmanager.Load<Texture2D>(@"UI\MenueButtons");

            //Platzhalter---------------------------------------------------------------------------------------
            Objekt = contentmanager.Load<Texture2D>(@"Objects\Testobjekt");
            Block = contentmanager.Load<Texture2D>(@"Teststuff\Block");
            Maske = contentmanager.Load<Texture2D>(@"Effects\Masks\Mask");

            //Dummy Texuren-------------------------------------------------------------------------------------
            Pixel = new Texture2D(graphicsdevice, 1, 1, false, SurfaceFormat.Color);
            this.Pixel.SetData(new[] { Color.Black });

            //Shader---------------------------------------------------------------------------------------------
            GrayScale = contentmanager.Load<Effect>(@"Effects\Shader\GrayScale");
            Invert = contentmanager.Load<Effect>(@"Effects\Shader\Invert");
            Invert = contentmanager.Load<Effect>(@"Effects\Shader\InvertHori");
            FOV = contentmanager.Load<Effect>(@"Effects\Shader\FOV");
            Noise = contentmanager.Load<Effect>(@"Effects\Shader\Noise");
            Glitchy = contentmanager.Load<Effect>(@"Effects\Shader\Glitchy");
        
        }
    }
}
