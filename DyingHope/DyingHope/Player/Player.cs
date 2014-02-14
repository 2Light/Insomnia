using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;


namespace DyingHope
{
    enum Playerstates
    {
        Idle,
        Walk,
        Jump,
        Climb1,
        Climb2,
        Lever,
        Run,
        Dead,
        Spawn,
    }
    enum PlayerDepression
    {
        InvertMovement,
        srink,
        Slow

    }

    class Player : IdebugObjekt
    {
        private Mathe Mathe;
        public Levelmanager Levelmanager;
        public Texture2D[] Texturen = new Texture2D[6];
        public Vector2 Mittelpunkt = new Vector2(100, 125);
        public Animation Animation;
        public Richtung Blickrichtung;
        public int Skalierung = 1;
        public Playerstates Playerstate;

        public bool DepressionEnabled = true;
        public float DepressionRate = 0f;
        public float DepressionIst = 0f;  //Depressionsausprägung
        public float DepressionSoll;    //Sollwert für langsame Anpassung des Ist-Wertes
        public float GameOverMaske = 0f;    //Transparenz der schwarzen Maske bei Spielertot
        public Vector2 PositionCurrent;
        public Vector2 PositionLast;
        public Vector2 Geschwindigkeit;
        public Vector2 MaxSpeed;      //Maximale Geschwindigkeit
        public Vector2 Beschleunigung;    //Beschleunigung bei Tastendruck
        public float Reibung;
        public float Gravitation;
        public Vector2 Initialgeschwindigkeit;  //Ausgangsgeschwindigkeit bei Sprungstart
        public Boolean isFalling = false;   //Maximale Sprungdauer erreicht oder Sprungbefehl unterbrochen
        public Boolean isPressingUp = false;   //Sprung/Klettern betätigt in aktuellem Frame
        public Boolean isPressingDown = false;
        public int jumpCounter; //Aktuelle Sprungzeit
        public int jumpCounterMax;  //Maximale Sprungdauer
        public int jumpCooldown;    //Aktualwert der Verzögerung bis erneuter Sprung möglich
        public int jumpCooldownBase;    //Startwert der Verzögerung bis erneuter Sprung möglich
        public float Ground;    //Grundkollsionshöhe
        public bool Groundcollision;    //Kollision mit Boden an/aus
        public bool ClimbEnabled;   //Klettern an dieser Stelle möglich
        public Objectmanager Objectmanager;
        private Contents Contents;
        public Rectangle Kollision = new Rectangle(74, 47, 61, 153);
        public Body body;

        public bool Kollidiert;

        #region debugStuff

           public String debugString { get; set; }

           public  StringBuilder stringBuilder { get; set; }
           public  DebugFlag debugFlag { get; set; }

           public void addString(String text)
           {
               this.stringBuilder.AppendLine(text);
           }
           
            public  void clearString()
           {
               this.stringBuilder.Clear();
               this.debugString = String.Empty;
           }

           public void DrawExtraDebug(DebugFlag extraFlag,SpriteBatch batch,Camera cam,Contents content)
           {
               batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.ViewMatrix);
                 batch.DrawString(content.Meiryo8, "Test Extra Debug", new Vector2(100, 200) -new Vector2(this.PositionCurrent.X,0), Color.White);
               batch.End();
           }

           public void handelDebug()
           {
               addString("Playerstate " + Playerstate.ToString());
               addString("Playerposition " + PositionCurrent.ToString());
               addString("Playerspeed " + Geschwindigkeit.ToString());
               addString("Playeracceleration " + Beschleunigung.ToString());
               addString("Animationsframe " + Animation.AktuelleSpalte.ToString());
               addString("Animationsspeed " + Animation.Geschwindigkeit.ToString());
               addString("isFalling " + isFalling.ToString());
               addString("jumpCounter " + jumpCounter.ToString());
               addString("jumpCounterMax " + jumpCounterMax.ToString());
               addString("Depression " + (DepressionIst * 100).ToString());
               addString("RateAktiv " + DepressionEnabled.ToString());
           }

        #endregion

        public Player(Mathe mathe, Contents contents, World world, DebugFlag flag)
        {
            this.Mathe = mathe;
            debugFlag = flag;
            this.stringBuilder = new StringBuilder();
            this.Contents = contents;

            PositionCurrent = new Vector2(0, 715);
            Texturen[0] = contents.Player1;
            Texturen[1] = contents.Player2;
            Texturen[2] = contents.Player3;
            Texturen[3] = contents.Player4;
            Texturen[4] = contents.Player5;
            Texturen[5] = contents.Player6;
            Animation = new Animation(Texturen[0], true, 1, 200, 200, 1, 6);
            Blickrichtung = Richtung.Rechts;
            Playerstate = Playerstates.Idle;
            MaxSpeed = new Vector2(6, 12);
            Beschleunigung = new Vector2(0.25f, 0.60f);
            Initialgeschwindigkeit = new Vector2(2.0f, 5.5f);
            Reibung = 0.35f;
            Gravitation = 0.40f;
            jumpCounter = 0;
            jumpCounterMax = 11;
            jumpCooldown = 0;
            jumpCooldownBase = 20;
            Ground = 715;
            Groundcollision = true;

            body = BodyFactory.CreateRectangle(world, 50, 50, 1.0f, PositionCurrent);
        }

        public void Update(float elapsed)
        {
            Kollidiert = false;
            if ((Playerstate != Playerstates.Dead) && (Playerstate != Playerstates.Spawn) && (Playerstate != Playerstates.Lever))
            {
                PositionLast = new Vector2(PositionCurrent.X, PositionCurrent.Y);   //Alte Position speichern für Kollisionsrücksetzung
                PositionCurrent += Geschwindigkeit; //Geschwindigkeit verarbeiten
                //if (Playerstate != Playerstates.Climb) Physik();   //Reibung, Gravitation, Bodenschwelle (Landung)
                Physik();   //Reibung, Gravitation, Bodenschwelle (Landung)
            }
            if ((Playerstate != Playerstates.Climb1) || (isPressingUp) || (isPressingDown)) Animation.Update(elapsed);   //Animation verarbeiten (Klettern nur bei Bewegung)
            switch (Playerstate)
            {
                case Playerstates.Idle: if (jumpCooldown != 0) jumpCooldown--; break;
                case Playerstates.Walk: if (jumpCooldown != 0) jumpCooldown--; if (Geschwindigkeit.X == 0) Statuswechsel(Playerstates.Idle); break;
                case Playerstates.Jump: Jump(); break;
                case Playerstates.Climb1: isPressingUp = false; isPressingDown = false; Geschwindigkeit.Y = 0; break;
                case Playerstates.Climb2:
                    if (Animation.AktuelleSpalte <= 5) PositionCurrent.Y -= 1.8f;
                    else if (Animation.AktuelleSpalte > 12) PositionCurrent.Y -= 1.7f;
                    else if (Animation.AktuelleSpalte >= 7) PositionCurrent.Y -= 1.2f;
                    if (Animation.AktuelleSpalte >= 12)
                    {
                        if (Blickrichtung == Richtung.Links) PositionCurrent.X -= 1.8f;
                        if (Blickrichtung == Richtung.Rechts) PositionCurrent.X += 1.8f;
                    }
                    else if (Animation.AktuelleSpalte < 10)
                    {
                        if (Blickrichtung == Richtung.Links) PositionCurrent.X -= 0.25f;
                        if (Blickrichtung == Richtung.Rechts) PositionCurrent.X += 0.25f;
                    }
                    if (Animation.Abgeschlossen) Statuswechsel(Playerstates.Idle); 
                    break;
                case Playerstates.Lever: if (Animation.Abgeschlossen) Statuswechsel(Playerstates.Idle); break;
                case Playerstates.Dead:
                    if (GameOverMaske <= 1) GameOverMaske += 0.02f;
                    else
                    {
                        DepressionIst = 0f; DepressionSoll = 0f;
                        Geschwindigkeit = new Vector2(0, 0);
                        Statuswechsel(Playerstates.Spawn);
                        PositionCurrent = new Vector2(Levelmanager.Checkpoint.X, Levelmanager.Checkpoint.Y);
                        PositionLast = new Vector2(PositionCurrent.X, PositionCurrent.Y);
                    }
                    break;
                case Playerstates.Spawn:
                    if (GameOverMaske <= 0) Statuswechsel(Playerstates.Idle);
                    else GameOverMaske -= 0.02f;
                    break;
            }
            if ((Playerstate != Playerstates.Dead) && (Playerstate != Playerstates.Spawn))
            {
                if (Playerstate != Playerstates.Climb2) Collisioncheck();
                Depression();
            }
            ClimbEnabled = false;   //Klettern deaktvieren (Wird per Eventaktion dauerhaft aktiviert)
            //if ((Playerstate != Playerstates.Climb) || (isPressingUp)) Animation.Update(elapsed);   //Animation verarbeiten (Klettern nur bei Bewegung)
        }

        public void Depression()    //Langsame Annäherung von Ist zum Sollwert für Effektdarstellung
        {
            if (DepressionEnabled) DepressionSoll += (DepressionRate / 600);
            if (Mathe.Differenz(DepressionIst, DepressionSoll) > 0.02)
            {
                if (DepressionIst > DepressionSoll)
                {
                    if (DepressionSoll < 0) DepressionSoll = 0;
                    DepressionIst -= 0.004f;
                }
                else
                {
                    DepressionIst += 0.004f;
                    if (DepressionIst >= 1) Statuswechsel(Playerstates.Dead);
                }
            }
        }

        public void Statuswechsel(Playerstates playerstate)
        {
            if (Playerstate != playerstate)
            {
                Playerstate = playerstate;
                switch (Playerstate)
                {
                    case Playerstates.Idle: Animation.Texture = Texturen[0]; Animation.Spalte = 1; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                    case Playerstates.Walk: Animation.Texture = Texturen[1]; Animation.Spalte = 9; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                    case Playerstates.Jump: Animation.Texture = Texturen[2]; Animation.Spalte = 6; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = false; break;
                    case Playerstates.Climb1: Animation.Texture = Texturen[3]; Animation.Spalte = 5; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                    case Playerstates.Climb2: Animation.Texture = Texturen[4]; Animation.Spalte = 14; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = false; break;
                    case Playerstates.Lever: Animation.Texture = Texturen[5]; Animation.Spalte = 5; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = false; break;
                    case Playerstates.Run: Animation.Texture = Texturen[1]; Animation.Spalte = 9; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                    case Playerstates.Dead: Animation.Texture = Texturen[0]; Animation.Spalte = 1; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                    case Playerstates.Spawn: Animation.Texture = Texturen[0]; Animation.Spalte = 1; Animation.Geschwindigkeit = ((float)1 / 7); Animation.Wiederholen = true; break;
                }
                Animation.Abgeschlossen = false;
                Animation.AktuelleSpalte = 0;
            }
        }

        public void Physik()
        {
            //Reibung/Luftwiderstand--------------------------------------------------------------------------------
            if (Geschwindigkeit.X > 0)
            {
                Geschwindigkeit.X -= Reibung;
                if (Geschwindigkeit.X < 0) Geschwindigkeit.X = 0;
            }
            else if (Geschwindigkeit.X < 0)
            {
                Geschwindigkeit.X += Reibung;
                if (Geschwindigkeit.X > 0) Geschwindigkeit.X = 0;
            }
            //if (Playerstate != Playerstates.Jump) if (Geschwindigkeit.X == 0) Statuswechsel(Playerstates.Idle);

            if ((Playerstate != Playerstates.Jump) || (Animation.AktuelleSpalte >= 2))
            {
                //Graviation--------------------------------------------------------------------------------------------
                if ((Playerstate != Playerstates.Climb1) && (Playerstate != Playerstates.Climb2))
                {
                    Geschwindigkeit.Y += Gravitation;   //Graviation erhöht Fallgeschwindigkeit
                    if (Geschwindigkeit.Y > MaxSpeed.Y) Geschwindigkeit.Y = MaxSpeed.Y; //Maximale Fallgeschwindigkeit
                    if (Geschwindigkeit.Y >= 1) isFalling = true;
                }
                //Boden-------------------------------------------------------------------------------------------------
                if (Groundcollision)
                {
                    if (PositionCurrent.Y >= Ground)   //Landung
                    {
                        PositionCurrent.Y = Ground;
                        Geschwindigkeit.Y = 0;
                        if (Geschwindigkeit.X == 0) Statuswechsel(Playerstates.Idle);
                        else Statuswechsel(Playerstates.Walk);
                        isFalling = false;
                        jumpCounter = 0;
                    }
                }
            }
            if (PositionCurrent.Y > 1024)  //Sturz in Abgrund -> Tod
            {
                Groundcollision = true;
                Statuswechsel(Playerstates.Dead);
            }
            Groundcollision = true;
        }

        public void Jump()
        {
            if ((Playerstate == Playerstates.Jump) && (!isFalling) && ((jumpCounter > jumpCounterMax) || (!isPressingUp)))    //Spieler fällt noch nicht und maximale Sprungdauer erreicht oder Sprung unterbrochen
            {
                {
                    isFalling = true;
                    Geschwindigkeit.Y = Geschwindigkeit.Y / 2;  //Sprung abbremsen
                    jumpCooldown = jumpCooldownBase;
                }
            }
            isPressingUp = false;  //Rücksetzen für erneute Abfrage nächsten Frame
        }

        public void Collisioncheck()
        {
            if (Objectmanager.Kollisionsabfrage(new Rectangle((int)(PositionCurrent.X + Kollision.X + 540), (int)(PositionLast.Y + Kollision.Y), Kollision.Width, Kollision.Height), true) != null)
            {
                PositionCurrent.X = PositionLast.X;
                Geschwindigkeit.X = 0.1f;
            }
            Objectcollision TempCollision = Objectmanager.Kollisionsabfrage(new Rectangle((int)(PositionLast.X + Kollision.X + 540), (int)(PositionCurrent.Y + Kollision.Y), Kollision.Width, Kollision.Height), false);
            if (TempCollision != null)
            {
                Kollidiert = true;
                if (Geschwindigkeit.X == 0) Statuswechsel(Playerstates.Idle);
                else Statuswechsel(Playerstates.Walk);
                if (PositionCurrent.Y > PositionLast.Y) //Kollision von Oben
                {
                    PositionCurrent.Y = TempCollision.Zone.Y - (Kollision.Y + Kollision.Height);
                    isFalling = false;
                    jumpCounter = 0;
                    Geschwindigkeit.Y = 0;
                }
                else    //Kollision von Unten
                {
                    PositionCurrent.Y = PositionLast.Y;
                    isFalling = true;
                    Geschwindigkeit.Y = -Geschwindigkeit.Y / 2; //Geschwindigkeit gedämpft umkehren
                }
            }
        }

        public void Accelerate(Richtung richtung)
        {
            if ((Playerstate != Playerstates.Dead) && (Playerstate != Playerstates.Spawn) && (Playerstate != Playerstates.Climb2) && (Playerstate != Playerstates.Lever))
            {
                richtung = (DepressionHandler.isSet(DepressionState.InvertMove)) ? InvertMovement(richtung) : richtung;
                this.Beschleunigung = (DepressionHandler.isSet(DepressionState.Slow)) ? new Vector2(0.25f, this.Beschleunigung.Y) : new Vector2(0.25f, 0.60f);

                switch (richtung)
                {
                    case Richtung.Links:
                        if (!((Geschwindigkeit.Y != 0f) && (Playerstate == Playerstates.Climb1)))
                        {
                            if (Playerstate == Playerstates.Idle) Statuswechsel(Playerstates.Walk);
                            if (Playerstate == Playerstates.Climb1)  //Seitwärtsbewegung beim Klettern -> Klettern abbrechen
                            {
                                isFalling = true;
                                Statuswechsel(Playerstates.Idle);
                            }
                            Blickrichtung = Richtung.Links;
                            Geschwindigkeit.X -= Beschleunigung.X;
                            if (Geschwindigkeit.X > -Initialgeschwindigkeit.X) Geschwindigkeit.X = -Initialgeschwindigkeit.X;
                            if (Geschwindigkeit.X < -(MaxSpeed.X)) Geschwindigkeit.X = -(MaxSpeed.X);
                        }
                        break;
                    case Richtung.Rechts:
                        if (!((Geschwindigkeit.Y != 0f) && (Playerstate == Playerstates.Climb1)))
                        {
                            if (Playerstate == Playerstates.Idle) Statuswechsel(Playerstates.Walk);
                            if (Playerstate == Playerstates.Climb1)  //Seitwärtsbewegung beim Klettern -> Klettern abbrechen
                            {
                                isFalling = true;
                                Statuswechsel(Playerstates.Idle);
                            }
                            Blickrichtung = Richtung.Rechts;
                            Geschwindigkeit.X += Beschleunigung.X;
                            if (Geschwindigkeit.X < Initialgeschwindigkeit.X) Geschwindigkeit.X = Initialgeschwindigkeit.X;
                            if (Geschwindigkeit.X > MaxSpeed.X) Geschwindigkeit.X = MaxSpeed.X;
                        }
                        break;
                    case Richtung.Hoch:
                        isPressingUp = true;   //Spieler betätigt Springen/Klettern diesen Frame (zwecks Sprungabbruch wenn unterbrochen)
                        if (ClimbEnabled)   //Klettern möglich -> nicht springen
                        {
                            if (!isFalling)
                            {
                                if (Playerstate != Playerstates.Climb1) Statuswechsel(Playerstates.Climb1);
                                Geschwindigkeit.Y = -2f; 
                                Geschwindigkeit.X = 0;
                            }
                        }
                        else if (Playerstate != Playerstates.Climb1)    //Klettern nicht möglich -> springen (Statusabfrage unterbindet springen auf Leitern)
                        {
                            if ((!isFalling) && (jumpCooldown == 0))
                            {
                                //if ((Playerstate != Playerstates.Jump) && (Geschwindigkeit.Y <= 0))   //Sprung initialisieren
                                if (Playerstate != Playerstates.Jump)   //Sprung initialisieren
                                {
                                    Statuswechsel(Playerstates.Jump);
                                    Geschwindigkeit.X = 0;  //Stehen bleiben
                                    //Geschwindigkeit.Y = -Initialgeschwindigkeit.Y;  //Startgeschwindigkeit bei Sprung
                                }
                                else if (Animation.AktuelleSpalte >= 2)   //Sprung gedrückt halten + Sprunganimation erreicht Auslösepunkt
                                {
                                    if (Geschwindigkeit.Y == 0) Geschwindigkeit.Y = -Initialgeschwindigkeit.Y;  //Startgeschwindigkeit bei Sprung
                                    jumpCounter++;  //Sprungzeit
                                    Geschwindigkeit.Y -= Beschleunigung.Y;  //Beschleunigen
                                    if (Geschwindigkeit.Y < -MaxSpeed.Y) Geschwindigkeit.Y = -MaxSpeed.Y;   //Maximale Geschwindigkeit
                                }
                                else Geschwindigkeit.X = 0; //Wenn Frame 2 der Sprunganimation noch nicht erreicht X-Bewegung weiterhin einfrieren
                            }
                        }
                        else    //Spieler ist an oberer Kletterkante -> Hochziehen
                        {
                            Statuswechsel(Playerstates.Climb2);
                            if (Blickrichtung == Richtung.Links) PositionCurrent.X -= 10;
                            else if (Blickrichtung == Richtung.Rechts) PositionCurrent.X += 10;
                        }
                        break;
                    case Richtung.Runter:
                        isPressingDown = true;
                        if ((Playerstate == Playerstates.Climb1) && (!isFalling))
                        {
                            Geschwindigkeit.Y = 2f;
                            Geschwindigkeit.X = 0;
                        }
                        break;
                }
            }
        }

        private Richtung InvertMovement(Richtung richtung)
        {
            switch (richtung)
            {
                case Richtung.Links: return Richtung.Rechts;
                case Richtung.Rechts: return Richtung.Links;
                case Richtung.Hoch: return Richtung.Runter;
                case Richtung.Runter: return Richtung.Hoch;                    
                default: return richtung;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(540, PositionCurrent.Y), Color.White, Blickrichtung, Skalierung);
        }

        public void DrawDepressionsmaske(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Contents.Maske, new Vector2(0, 0 - 715 + (int)PositionCurrent.Y), Color.White * DepressionIst);
            if ((Playerstate == Playerstates.Dead) || (Playerstate == Playerstates.Spawn)) spriteBatch.Draw(Contents.Block, new Rectangle(0, 0, 1280, 1024), Color.Black * GameOverMaske);
        }
    }
}
            
      



        


