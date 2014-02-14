using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Cursor
    {
        private Contents Contents;
        private Settings Settings;
        private Keymanager Keymanager;
        public Camera Kamera;
        private Mathe Mathe;
        public Vector2 Mouseposition;
        public MouseState CurrentMousestate;
        public MouseState OldMousestate;
        public bool Leftclick;
        public bool Rightclick;
        public bool SperreLinks = false;
        public bool SperreRechts = false;
        private bool Flankenmerker1;
        private bool Flankenmerker2;
        public Window Fenster;   //Fenster am Mauszeiger -> verschieben
        public Vector2 Bezugspunkt; //Die Position des Mauszeigers beim starten eines Verschiebevorgangs
        public float ScrollWheelChange;     //Mausradwert im letzten Frame
        public Editor Editor;

        public Cursor(Camera camera, Keymanager keymanager, Contents contents, Settings settings)
        {
            this.Contents = contents;
            this.Settings = settings;
            this.Keymanager = keymanager;
            this.Kamera = camera;
            this.Mathe = new Mathe();
        }

        public void Update(Vector2 spieler, bool mouseovergui, Gamestate gamestate)
        {
            CurrentMousestate = Mouse.GetState();
            if (gamestate != Gamestate.Game)    //Im Spiel nur Zoom benötigt
            {
                if (Settings.Widescreen) Mouseposition = new Vector2(CurrentMousestate.X, ((float)CurrentMousestate.Y / 720) * 1024);
                else Mouseposition = new Vector2(CurrentMousestate.X, CurrentMousestate.Y);
                FensterVerschieben();
                Buttons(mouseovergui);
            }
            Scrollwheel(gamestate);
            OldMousestate = CurrentMousestate;
        }

        private void Buttons(bool mouseovergui)
        {
            if (CurrentMousestate.LeftButton == ButtonState.Pressed)   //Linksklick
            {
                if (Leftclick == false)
                {
                    Leftclick = true;
                    if (mouseovergui == true) SperreLinks = true; //Button sperren wenn Objekt am Zeiger oder Zeiger über Interface
                    else Keymanager.EingabeAbbrechen();
                }
                if (Flankenmerker1 == true) Leftclick = false;
                Flankenmerker1 = true;
            }
            else
            {
                SperreLinks = false;
                Leftclick = false;
                Flankenmerker1 = false;
                Fenster = null;
            }

            if (CurrentMousestate.RightButton == ButtonState.Pressed)  //Rechtsklick
            {
                if (Rightclick == false)
                {
                    Rightclick = true;
                    if (mouseovergui == true) SperreRechts = true; //Button sperren wenn Zeiger über Interface
                }
                if (Flankenmerker2 == true) Rightclick = false;
                Flankenmerker2 = true;
            }
            else
            {
                SperreRechts = false;
                Rightclick = false;
                Flankenmerker2 = false;
            }
        }

        private void Scrollwheel(Gamestate gamestate)
        {
            ScrollWheelChange = (float)(OldMousestate.ScrollWheelValue - CurrentMousestate.ScrollWheelValue);
            if (ScrollWheelChange < 0)
            {
                if (gamestate == Gamestate.Game)
                {
                    Kamera.Zoom += 0.01f;
                    if (Kamera.Zoom > 2.0f) Kamera.Zoom = 2.0f;
                }
                else if (gamestate == Gamestate.Editor && Editor.AuswahlLayer < 9) Editor.AuswahlLayer++;
            }
            else if (ScrollWheelChange > 0)
            {
                if (gamestate == Gamestate.Game)
                {
                    Kamera.Zoom -= 0.01f;
                    if (Kamera.Zoom < 1.0f) Kamera.Zoom = 1.0f;
                }
                else if (gamestate == Gamestate.Editor && Editor.AuswahlLayer > 1) Editor.AuswahlLayer--;
            }
        }

        private void FensterVerschieben()
        {
            if (Fenster != null) Fenster.Feld = new Rectangle((int)(CurrentMousestate.X - Bezugspunkt.X), (int)(CurrentMousestate.Y - Bezugspunkt.Y), Fenster.Feld.Width, Fenster.Feld.Height);
        }

        public void Draw(SpriteBatch spritebatch) //Zeiger zeichnen
        {
            spritebatch.Draw(Contents.Maus, new Vector2(Mouseposition.X, Mouseposition.Y), Color.White);
        }
    }
}
