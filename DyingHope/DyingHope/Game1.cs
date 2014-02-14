using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.DebugView;

namespace DyingHope
{
    public enum Gamestate
    {
        Menue,
        Game,
        Editor,
        //Exit,
    }

    public enum Menuestate
    {
        Started,
        Pause,
        New,
        Continue,
        Load,
        Save,
        Options,
        Controls,
        Credits,
        Exit,
        StartEditor,
        LeaveEditor,
    }

    public enum Richtung
    {
        Links,
        Rechts,
        Hoch,
        Runter,
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Grafik-----------------------------------------------------------------------------------
        PrimitiveBatch primBatch;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        //public Vector2 Auflösung = new Vector2(1280, 720);
        RenderTarget2D SpielGesamt;
        //Klassen----------------------------------------------------------------------------------
        Objectdatabase Objectdatabase;
        Backgrounddatabase Backgrounddatabase;
        Enemydatabase Enemydatabase;
        Animationsmanager Animationsmanager;
        Editor Editor;
        Enemymanager Enemymanager;
        Keymanager Keymanager;
        Cursor Cursor;
        Inputmanager Inputmanager;
        Windowmanager Windowmanager;
        Itemmanager Itemmanager;
        Backgroundmanager Backgroundmanager;
        Eventmanager Eventmanager;
        Levermanager Levermanager;
        Objectmanager Objectmanager;
        Levelmanager Levelmanager;
        Player Player;
        Camera Camera;
        Contents Contents;
        Mathe Mathe;
        Settings Settings;
        //Sonstige Variablen---------------------------------------------------------------------
        Gamestate Gamestate = Gamestate.Menue;
        Menuestate Menuestate;
       //public static Boolean exitState = false;
        World World;
        DebugViewXNA Debugview;
        DebugView Debug;
        DepressionHandler DepressionHandler;

        //Musik-----------------------------------------------------------------------------------
        BackgroundMusic BackgroundMusic;
        List<MSong> levelTheme;
        List<MSong> menueTheme;
        SoundObjekt light, rain;

        //Shader-----------------------------------------------------------------------------------
        Effect grayScale;
        Emitter testEmitter, rainEmitter, leavEmitter,menue;

        double noiseTime;
        public static float glitch = 0.0001f;

        public static int onGo = 0;
        public static float trashholNoise = 0.05f;
        public static float noiseAmount = trashholNoise;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferHeight = (int)Auflösung.Y;
            //graphics.PreferredBackBufferWidth = (int)Auflösung.X;
            //Settings = new Settings(graphics);
            device = graphics.GraphicsDevice;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //primBatch = new PrimitiveBatch(GraphicsDevice);
            World = new World(new Vector2(0, 9.81f));
            Mathe = new Mathe();
            Settings = new Settings(graphics);
            Camera = new Camera(GraphicsDevice.Viewport, Settings.Widescreen);
            Contents = new Contents(Content, GraphicsDevice);
            Debug = new DebugView(Contents,Camera);
            Objectdatabase = new Objectdatabase(Content);
            Backgrounddatabase = new Backgrounddatabase(Content);
            Enemydatabase = new Enemydatabase(Content);
            Animationsmanager = new Animationsmanager();  
            Keymanager = new Keymanager(Contents, DebugFlag.Inputstats);
            Player = new Player(Mathe, Contents, World, DebugFlag.PlayerStats);
            Cursor = new Cursor(Camera, Keymanager, Contents, Settings);
            Windowmanager = new Windowmanager(Settings, Cursor, DebugFlag.WindowStats, Contents, GraphicsDevice);
            DepressionHandler = new DyingHope.DepressionHandler();
            Itemmanager = new Itemmanager(Mathe, DepressionHandler, Content, Player);
            Backgroundmanager = new Backgroundmanager(Player, Backgrounddatabase, DebugFlag.BackgrundStats, Contents);
            Levermanager = new Levermanager(Contents.Meiryo8, Player, Cursor, Mathe, Animationsmanager, Contents);
            Objectmanager = new Objectmanager(Objectdatabase, Animationsmanager, Player, (DebugFlag.ObjectInformation | DebugFlag.CollisonShape | DebugFlag.ObjectmanagerStats));
            Enemymanager = new Enemymanager(Objectmanager, Mathe, Content, Enemydatabase, Player);
            Windowmanager.Windowdatabase = new Windowdatabase(Contents, Player, Inputmanager);
            Windowmanager.AddWindow(Windowtype.MainMenue);
            Eventmanager = new Eventmanager(Mathe, Player, Enemymanager, DepressionHandler, Objectmanager, Objectdatabase, Levermanager);
            Levelmanager = new Levelmanager(Player, Objectmanager, Animationsmanager, Backgroundmanager, Enemymanager, Itemmanager, Levermanager, Eventmanager, Contents);
            Editor = new Editor(Windowmanager, Enemymanager, Backgroundmanager, Backgrounddatabase, Objectmanager, Objectdatabase, Eventmanager, Contents, Cursor, Player, Keymanager, Levelmanager, Itemmanager, Levermanager);
            Debugview = new DebugViewXNA(World);
            Console.WriteLine(((DebugFlag.ObjectInformation | DebugFlag.CollisonShape | DebugFlag.ObjectmanagerStats)& DebugFlag.ObjectmanagerStats));
            light = new SoundObjekt(Settings, Contents.Emitter, Contents.Light, new Vector2(3500 - 576, 700), new Vector2(100, 0), 200);
            rain = new SoundObjekt(Settings, Contents.Emitter, Contents.Rain, new Vector2(6000 - 576, 0), new Vector2(1000, 0), 400);
            levelTheme = new List<MSong> { new MSong(Contents.StadtAnfang, 0), new MSong(Contents.StadtLoop, -1), new MSong(Contents.StadtEnde, 0), new MSong(Contents.ParkAnfang, 0), new MSong(Contents.ParkLoop, -1), new MSong(Contents.ParkEnde, 0), new MSong(Contents.SchuleAnfang, 0), new MSong(Contents.SchuleLoop, -1), new MSong(Contents.StadtEnde, 0), new MSong(Contents.MonsterAnfang, 0), new MSong(Contents.MonsterLoop, -1), new MSong(Contents.Ende, 0) };
            menueTheme = new List<MSong> { new MSong(Contents.Menue_Theme, 99) };
            BackgroundMusic = new BackgroundMusic(Settings, menueTheme);
            Inputmanager = new Inputmanager(Contents, Camera, Player, Keymanager, Cursor, Enemymanager, Windowmanager, Backgroundmanager, Backgrounddatabase, Objectmanager, Objectdatabase, Eventmanager, Levelmanager, Levermanager, Content, Editor, Debug, DepressionHandler, Mathe, Itemmanager, BackgroundMusic);

            //Testemitter-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //testEmitter = new Emitter(new Vector2(3000, 850), DebugFlag.Pertikle);
            //testEmitter.startParticel = 50;
            //testEmitter.EmitterAngel = new List<Vector2> { new Vector2(180, 180) };
            //testEmitter.EmitterRange = new Vector2(1000, 50);
            //testEmitter.EmitterMaxParticel = 100;
            //testEmitter.EmitterUpdate = 100f;
            //testEmitter.EmitterTexture = Contents.Emitter;
            //testEmitter.ParticelStayOnMax = new Vector2(0, 0);
            //testEmitter.ParticelLifeDrain = new Vector2(1, 1);
            //testEmitter.ParticelLifeTime = new Vector2(5000f, 50000f);
            //testEmitter.ParticelSize = new Vector2(0.1f, 0.5f);
            //testEmitter.ParticelTexture = new List<Texture2D> { Contents.Nebel, Contents.Nebel2 };
            //testEmitter.Origin = new Vector2(Contents.Emitter.Width / 2, Contents.Emitter.Height / 2);
            //testEmitter.PerticelPower = new Vector2(0.50f, 0.8f);
            //testEmitter.particelNeverDie = false;
            //testEmitter.spawnprofile = Spawnprofile.FadeIn;
            //testEmitter.bewegungsprofil = Bewegungsprofile.Linear;
            //testEmitter.Start();

            //rainEmitter = new Emitter(new Vector2(6000, -10), DebugFlag.Pertikle);
            //rainEmitter.startParticel = 0;
            //rainEmitter.particelPerUpdate = new Vector2(1, 100);
            //rainEmitter.EmitterAngel = new List<Vector2> { new Vector2(270, 270) };
            //rainEmitter.EmitterRange = new Vector2(1000, 0);
            //rainEmitter.EmitterMaxParticel = 60000;
            //rainEmitter.EmitterUpdate = 10f;
            //rainEmitter.EmitterTexture = Contents.Emitter;
            //rainEmitter.ParticelStayOnMax = new Vector2(0, 0);
            //rainEmitter.ParticelLifeDrain = new Vector2(1, 1);
            //rainEmitter.ParticelLifeTime = new Vector2(500f, 2000f);
            //rainEmitter.ParticelSize = new Vector2(0.5f, 1.5f);
            //rainEmitter.ParticelTexture = new List<Texture2D> { Contents.Rain1 };
            //rainEmitter.Origin = new Vector2(Contents.Emitter.Width / 2, Contents.Emitter.Height / 2);
            //rainEmitter.PerticelPower = new Vector2(10.50f, 20.8f);
            //rainEmitter.particelNeverDie = false;
            //rainEmitter.spawnprofile = Spawnprofile.Instant;
            //rainEmitter.bewegungsprofil = Bewegungsprofile.Linear;
            //rainEmitter.Start();

            menue = new Emitter(new Vector2(GraphicsDevice.Viewport.Width / 2, (GraphicsDevice.Viewport.Height / 2)+20), DebugFlag.Pertikle);      
            menue.startParticel = 0;
            menue.particelPerUpdate = new Vector2(1, 5);
            menue.EmitterAngel = new List<Vector2> { new Vector2(0, 360) };
            menue.EmitterRange = new Vector2(250, 50);
            menue.EmitterMaxParticel = 6000;
            menue.EmitterUpdate = 100f;
            menue.EmitterTexture = Contents.Emitter;
            menue.ParticelStayOnMax = new Vector2(0, 100);
            menue.ParticelLifeDrain = new Vector2(1, 1);
            menue.ParticelLifeTime = new Vector2(1000f, 3000f);
            menue.ParticelSize = new Vector2(0.065f, 0.09f);
            menue.ParticelTexture = new List<Texture2D> { Contents.Partikel1, Contents.Partikel2, Contents.Partikel3 };
            menue.Origin = new Vector2(Contents.Emitter.Width / 2, Contents.Emitter.Height / 2);
            menue.PerticelPower = new Vector2(0.2f, 0.9f);
            menue.particelNeverDie = false;
            menue.spawnprofile = Spawnprofile.FadeIn;
            menue.bewegungsprofil = Bewegungsprofile.LinearRandom;
            menue.Start();

            //Debug-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Debug.Add(Player);
            Debug.Add(Objectmanager);
            Debug.Add(Keymanager);
            Debug.Add(Backgroundmanager);
            Debug.Add(Windowmanager);
            Debug.Add(DepressionHandler);
            //Debug.Add(testEmitter);
            //Debug.AppendFlag(DebugFlag.PlayerStats, false);
            //Debug.AppendFlag(DebugFlag.ObjectmanagerStats,false);
            //Debug.AppendFlag(DebugFlag.ObjectInformation, true);
            //Debug.AppendFlag(DebugFlag.CollisonShape, true);
            Debugview.DebugPanelPosition = new Vector2(500, 100);
            Debugview.AppendFlags(DebugViewFlags.DebugPanel);
            Debugview.LoadContent(GraphicsDevice, Content);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpielGesamt = new RenderTarget2D(GraphicsDevice, 1280, 1024, false, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            Menuestate TempMenuestate = Inputmanager.Update(Gamestate, Menuestate); //Input verarbeiten und resultierenden Menüzustand speichern
            if (Menuestate != TempMenuestate)   //Vergleich mit aktuellem Menüzustand -> Bei Änderung Menü starten und Zustand übernehmen
            {
                Gamestate = Gamestate.Menue;
                Menuestate = TempMenuestate;
            }

            switch (Gamestate)
            {
                case Gamestate.Menue:
                    switch (Menuestate)
                    {
                        case Menuestate.Started:
                            menue.Update(gameTime);
                            if (Settings.Music)
                            {
                                if (BackgroundMusic.isStooped()) BackgroundMusic.changeTrackList(menueTheme);
                                BackgroundMusic.Update();
                            }
                            else BackgroundMusic.Stop();
                            break;
                        case Menuestate.Pause:
                            menue.Update(gameTime);
                            if (Settings.Music)
                            {
                                if (BackgroundMusic.isStooped()) BackgroundMusic.changeTrackList(menueTheme);
                                BackgroundMusic.Update();
                            }
                            break;
                        case Menuestate.New: 
                            Gamestate = Gamestate.Game; 
                            Windowmanager.ClearWindows();
                            Keymanager.TastenSpiel();
                            if (Settings.Music)
                            {
                                BackgroundMusic.Stop();
                                BackgroundMusic.changeTrackList(levelTheme);
                            }
                            break;
                        case Menuestate.Continue: 
                            Gamestate = Gamestate.Game; 
                            Windowmanager.ClearWindows();
                            Keymanager.TastenSpiel();
                            if (Settings.Music) BackgroundMusic.Resume();
                            break;
                        case Menuestate.Load: break;
                        case Menuestate.Save: break;
                        case Menuestate.Options: break;
                        case Menuestate.Controls: break;
                        case Menuestate.Credits: break;
                        case Menuestate.Exit: 
                            this.Exit(); 
                            break;
                        case Menuestate.StartEditor: 
                            Gamestate = Gamestate.Editor;
                            Editor.ChangeTool(Editortool.Start);
                            Windowmanager.ClearWindows();
                            Windowmanager.AddWindow(Windowtype.Topbar);
                            Windowmanager.AddWindow(Windowtype.Editor);
                            Keymanager.TastenEditor();
                            Cursor.Kamera.Zoom = 1;
                            break;
                        case Menuestate.LeaveEditor: 
                            Gamestate = Gamestate.Game; 
                            Windowmanager.ClearWindows();
                            Keymanager.TastenSpiel(); 
                            break;
                    }
                    break;

                case Gamestate.Game:
                    //IsMouseVisible = false;
                    DepressionHandler.Update();
                    if (onGo > 0) modShader();
                    Animationsmanager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                    Player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                    Backgroundmanager.Update();
                    Objectmanager.Update();
                    Levermanager.Update();
                    Itemmanager.Update(Gamestate, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    Enemymanager.Update(Gamestate, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (Player.Playerstate != Playerstates.Dead) Eventmanager.Update();
                    //testEmitter.Update(gameTime);
                    //rainEmitter.Update(gameTime);
                    if (Settings.Sound) light.checkDistanze(Player.PositionCurrent);
                    //rain.checkDistanze(Player.PositionCurrent);
                    if (Settings.Music) BackgroundMusic.Update();
                    break;
                case Gamestate.Editor:
                    //IsMouseVisible = true;
                    Animationsmanager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                    Editor.Update();
                    Itemmanager.Update(Gamestate, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    Enemymanager.Update(Gamestate, (float)gameTime.ElapsedGameTime.TotalSeconds);
                    Backgroundmanager.Update();
                    Objectmanager.Update();
                    break;
                //case Gamestate.Exit:
                //    this.Exit();
                //    break;
            }
            Debug.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Spielwelt // Editor // Hauptmenü-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(SpielGesamt);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null,null, Camera.ViewMatrix);
            switch (Gamestate)
            {
                case Gamestate.Menue:
                    if (Settings.Widescreen)
                    {
                        Windowmanager.Draw(spriteBatch, Contents.Meiryo8);
                        Cursor.Draw(spriteBatch);
                    }
                    break;
                case Gamestate.Game:
                    Backgroundmanager.Draw(Gamestate, spriteBatch, GraphicsDevice);
                    Objectmanager.Draw(spriteBatch);
                    Itemmanager.Draw(spriteBatch);
                    Enemymanager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    Levermanager.Draw(spriteBatch);
                    Objectmanager.DrawVordergrund(spriteBatch);
                    //testEmitter.Draw(spriteBatch, Player);
                    //rainEmitter.Draw(spriteBatch, Player);
                    Player.DrawDepressionsmaske(spriteBatch);
                    break;
                case Gamestate.Editor:
                    Backgroundmanager.Draw(Gamestate, spriteBatch, GraphicsDevice);
                    Objectmanager.Draw(spriteBatch);
                    Itemmanager.Draw(spriteBatch);
                    Enemymanager.Draw(spriteBatch);
                    Levermanager.Draw(spriteBatch);
                    Objectmanager.DrawVordergrund(spriteBatch);
                    Editor.Draw(spriteBatch);
                    if (Settings.Widescreen)
                    {
                        Windowmanager.Draw(spriteBatch, Contents.Meiryo8);
                        Keymanager.Draw(spriteBatch);
                        Cursor.Draw(spriteBatch);
                    }
                    break;
            }
            spriteBatch.End();
            if (Settings.Widescreen) if (Inputmanager.Debug) Debug.Draw(spriteBatch);

            //Shader--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (DepressionHandler.isSet(DepressionState.GrayScal))  applyShader(spriteBatch,SpielGesamt,Contents.GrayScale) ;
            if (DepressionHandler.isSet(DepressionState.InvertScreen))  applyShader(spriteBatch, SpielGesamt, Contents.Invert);

            //noiseTime = gameTime.TotalGameTime.Milliseconds + 1;
            ////Contents.FOV.Parameters["OuterVig"].SetValue(1.5+0.3*Math.Sin(noiseTime + 5.0*Math.Cos(noiseTime*3.0)));
            //Contents.FOV.Parameters["OuterVig"].SetValue((float)(0.7f+0.03*Math.Sin(noiseTime+3.0f*Math.Cos(noiseTime*5.0f))));

            //applyShader(spriteBatch, SpielGesamt, Contents.FOV);

            //noiseTime = gameTime.TotalGameTime.Milliseconds + 12;
            //Contents.Noise.Parameters["time"].SetValue((float)noiseTime);
            //Contents.Noise.Parameters["noiseInterpolation"].SetValue(noiseAmount);
            //applyShader(spriteBatch, SpielGesamt, Contents.Noise);

            //Contents.Glitchy.Parameters["time"].SetValue((float)noiseTime);
            //Contents.Glitchy.Parameters["glitchIntensity"].SetValue(glitch);
            //applyShader(spriteBatch, SpielGesamt, Contents.Glitchy);

            //Skalierung -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            if (Settings.Widescreen) spriteBatch.Draw(SpielGesamt, new Rectangle(190, 0, 900, 720), Color.White);
            else spriteBatch.Draw(SpielGesamt, Vector2.Zero, Color.White);
            //Matrix view = Matrix.CreateTranslation(new Vector3((CoordinatenManager.toWorldCoordinat(Camera.Position) - CoordinatenManager.toWorldCoordinat(Camera.Origin)), 0f)) * Matrix.CreateTranslation(new Vector3(CoordinatenManager.toWorldCoordinat(Camera.Origin), 0f));

            //User Interface-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (!Settings.Widescreen)
            {
                //
                if (Gamestate != Gamestate.Game)
                {
                    Windowmanager.Draw(spriteBatch, Contents.Meiryo8);
                    Keymanager.Draw(spriteBatch);
                    Cursor.Draw(spriteBatch);
                }
            }
            if (Gamestate == Gamestate.Menue) menue.Draw(spriteBatch);
            else if (Gamestate == Gamestate.Editor) Editor.DrawVorschau(spriteBatch);

            //spriteBatch.DrawString(Contents.Meiryo8, Editor.Autosaveintervall.ToString(), new Vector2(10, 10), Color.Red);
            //spriteBatch.DrawString(Contents.Meiryo8, Editor.AutosaveRest.ToString(), new Vector2(10, 25), Color.Red);

            //if (Levermanager.Levers.Count != 0)
            //{
            //    spriteBatch.DrawString(Contents.Meiryo8, (Mathe.Distanzberechnung(Levermanager.Levers[0].Position + Levermanager.Levers[0].Mittelpunkt, new Vector2(Player.PositionCurrent.X + 640, Player.PositionCurrent.Y + Player.Mittelpunkt.Y))).ToString(), new Vector2(10, 25), Color.Red);
            //    spriteBatch.DrawString(Contents.Meiryo8, Levermanager.Levers[0].Mittelpunkt.ToString(), new Vector2(10, 40), Color.Red);
            //    spriteBatch.DrawString(Contents.Meiryo8, Levermanager.Levers[0].Animation.AktuelleSpalte.ToString(), new Vector2(10, 55), Color.Red);
            //    spriteBatch.DrawString(Contents.Meiryo8, Levermanager.Levers[0].Betätigt.ToString(), new Vector2(10, 100), Color.Red);
            //    spriteBatch.DrawString(Contents.Meiryo8, Levermanager.Levers[0].BetätigtFlanke.ToString(), new Vector2(10, 115), Color.Red);
            //    spriteBatch.DrawString(Contents.Meiryo8, Levermanager.Levers[0].Flankenmerker.ToString(), new Vector2(10, 130), Color.Red);
            //}
            //spriteBatch.DrawString(Contents.Meiryo8, Player.Mittelpunkt.ToString(), new Vector2(10, 70), Color.Red);
            //spriteBatch.DrawString(Contents.Meiryo8, Player.Animation.AktuelleSpalte.ToString(), new Vector2(10, 85), Color.Red);

            spriteBatch.End();
            if (Inputmanager.Debug) Debug.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        public void modShader()
        {
            if (onGo == 1)
            {
                noiseAmount += 0.005f;
                if (noiseAmount >= 0.1f)
                {
                    onGo = 0;
                }
            }
            else if (onGo == 2)
            {
                noiseAmount -= 0.005f;
                if (noiseAmount <= 0.05f)
                {
                    onGo = 0;
                }
            }
        }
        public void applyShader(SpriteBatch batch, RenderTarget2D target, Effect effect)
        {
            RenderTarget2D bufferTarget = new RenderTarget2D(GraphicsDevice, target.Width, target.Height, false, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            GraphicsDevice.SetRenderTarget(bufferTarget);
            GraphicsDevice.Clear(Color.Transparent);
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);
            batch.Draw(target, Vector2.Zero, Color.White);
            batch.End();

            Texture2D bufferTargetTex = bufferTarget;

            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Black);
            batch.Begin();
            batch.Draw(bufferTargetTex, Vector2.Zero, Color.White);
            batch.End();

            GraphicsDevice.SetRenderTarget(null);
            bufferTarget.Dispose();
        }
    }
}

/*      
    Debugview.RenderDebugData(Camera.projection, Camera.ViewMatrix);
                 
    spriteBatch.DrawString(Contents.Meiryo8, "Gamestate " + Gamestate.ToString(), new Vector2(10, 10), Color.White);
                        
    spriteBatch.DrawString(Contents.Meiryo8, "Mausposition: " + Cursor.CurrentMousestate.X.ToString() + " " + Cursor.CurrentMousestate.Y.ToString(), new Vector2(10, 470), Color.White);
    spriteBatch.DrawString(Contents.Meiryo8, "UI Befehl: " + Windowmanager.Befehl.ToString(), new Vector2(10, 480), Color.White);

    spriteBatch.DrawString(Contents.Meiryo8, "Kamera Zoom: " + Camera.Zoom.ToString(), new Vector2(10, 500), Color.White);
             
    if (Cursor.Fenster != null) spriteBatch.DrawString(Contents.Meiryo8, "Cursorfenster: " + Cursor.Fenster.Windowtype.ToString(), new Vector2(10, 490), Color.White);

    spriteBatch.DrawString(Contents.Meiryo8, "Editor Scrolling: " + Editor.AuswahlScrolling.ToString(), new Vector2(10, 640), Color.White);
    spriteBatch.DrawString(Contents.Meiryo8, "Editor Skalierung: " + Editor.AuswahlSkalierung.ToString(), new Vector2(10, 650), Color.White);

    if (Editor.AuswahlObjektkategorie != null) spriteBatch.DrawString(Contents.Meiryo8, "Objektkategorie " + Editor.AuswahlObjektkategorie.ObjectClass.ToString(), new Vector2(10, 550), Color.White);
    if (Editor.AuswahlObjektvariation != null) spriteBatch.DrawString(Contents.Meiryo8, "Objektvariante " + (Editor.AuswahlObjektkategorie.Variante.IndexOf(Editor.AuswahlObjektvariation) + 1).ToString(), new Vector2(10, 560), Color.White);
*/