using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace DyingHope
{
    enum Enemystate
    {
        Idle,
        Chase,
        Attack,
        Dead,
    }
    enum Enemytype
    {
        Wolf,
        Messerkreatur,
        Schatten,
        Greif,
    }

    class Enemy
    {
        private Objectmanager Objectmanager;
        private Mathe Mathe;
        public Enemystate State = Enemystate.Idle;
        public Enemytype Typ;
        public Animation Animation;
        public Vector2 Texturmaße;
        public Texture2D[] Textur = new Texture2D[3];
        public int[] AniGeschwindigkeit = new int[3];
        public int[] Spalten = new int[3];
        public bool[] Wiederholen = new bool[3];
        public string[] Texturname = new string[3];
        public float Skalierung;
        public Vector2 PositionCurrent;
        public Vector2 PositionLast;
        public Richtung Blickrichtung;
        public Vector2 Geschwindigkeit;
        public float Gravitation;   
        public Vector2 MaxSpeed;      //Maximale Geschwindigkeit
        public Rectangle Kollision;

        public Enemy() { }

        public Enemy(Objectmanager objectmanager, ContentManager contentManager, Mathe mathe, Enemydatabase enemydatabase, Enemytype typ, Vector2 position, Richtung blickrichtung, float skalierung)
        {
            Gravitation = 0.40f;
            MaxSpeed = new Vector2(6, 15);
            this.Objectmanager = objectmanager;
            this.Mathe = mathe;
            this.State = Enemystate.Idle;
            this.Blickrichtung = blickrichtung;
            this.Skalierung = skalierung;
            this.PositionCurrent = position;
            this.Typ = typ;
            Enemy Tempenemy = enemydatabase.Read(typ);
            Texturmaße = new Vector2(Tempenemy.Texturmaße.X, Tempenemy.Texturmaße.Y);
            AniGeschwindigkeit = Tempenemy.AniGeschwindigkeit;
            Spalten = Tempenemy.Spalten;
            Texturname = Tempenemy.Texturname;
            Wiederholen = Tempenemy.Wiederholen;
            Textur = Tempenemy.Textur;
            Kollision = new Rectangle((int)(Texturmaße.X / 2) - 5, (int)Texturmaße.Y - 20, 10, 10);
            for (int i = 0; i < Textur.Count(); i++)
            {
                if (Texturname[i] == "Idle")
                {
                    Animation = new Animation(Textur[i], true, Spalten[i], (int)Texturmaße.X, (int)Texturmaße.Y, skalierung, AniGeschwindigkeit[i]); break;
                }
            }
        }

        public void Update(float elapsed, Player player)
        {
            switch (State)
            {
                case Enemystate.Idle:
                    PositionLast = new Vector2(PositionCurrent.X, PositionCurrent.Y);   //Alte Position speichern für Kollisionsrücksetzung
                    PositionCurrent += Geschwindigkeit; //Geschwindigkeit verarbeiten
                    Physik();
                    CollisionCheck(player);

                    float Distanz = Mathe.Distanzberechnung(new Vector2(PositionCurrent.X + (Texturmaße.X / 2), player.PositionCurrent.Y), new Vector2(player.PositionCurrent.X + 640, player.PositionCurrent.Y));
                    if (Distanz < (Texturmaße.X / 2) + 60) //Spieler nah genug für umdrehen
                    {
                        float DistanzX = Mathe.Distanzberechnung(new Vector2(PositionCurrent.X + (Texturmaße.X / 2), PositionCurrent.Y + (Texturmaße.Y / 2)), new Vector2(player.PositionCurrent.X + 640, player.PositionCurrent.Y));
                        if ((PositionCurrent.X + (Texturmaße.X / 2) < player.PositionCurrent.X + 640) && (Blickrichtung == Richtung.Links)) Blickrichtung = Richtung.Rechts;
                        if ((PositionCurrent.X + (Texturmaße.X / 2) > player.PositionCurrent.X + 640) && (Blickrichtung == Richtung.Rechts)) Blickrichtung = Richtung.Links;
                        if (DistanzX < (Texturmaße.X / 2) + 40)  //Spieler nah genug für Angriff
                        {
                            ChangeStatus(Enemystate.Attack);
                            player.DepressionSoll += (float)35 / 100;
                        }
                    }
                    break;
                case Enemystate.Attack:
                    if (Animation.Abgeschlossen) ChangeStatus(Enemystate.Idle);
                    break;
            }
            Animation.Update(elapsed);
        }

        private void Physik()
        {
            Geschwindigkeit.Y += Gravitation;   //Graviation erhöht Fallgeschwindigkeit
            if (Geschwindigkeit.Y > MaxSpeed.Y) Geschwindigkeit.Y = MaxSpeed.Y; //Maximale Fallgeschwindigkeit
            if (PositionCurrent.Y > 1024) State = Enemystate.Dead; //Sturz in Abgrund -> Tod
        }

        private void CollisionCheck(Player player)
        {
            if ((Geschwindigkeit.Y < 1) && (PositionCurrent.Y + (Texturmaße.Y * Skalierung) - 10 > player.Ground + 200))
            {
                PositionCurrent.Y = PositionLast.Y;
                Geschwindigkeit.Y = 0;
            }
            Objectcollision TempCollision = Objectmanager.Kollisionsabfrage(new Rectangle((int)(PositionLast.X + (Kollision.X * Skalierung)), (int)(PositionCurrent.Y + (Kollision.Y * Skalierung)), (int)(Kollision.Width * Skalierung), (int)(Kollision.Height * Skalierung)), false);
            if (TempCollision != null)
            {
                PositionCurrent.Y = TempCollision.Zone.Y - ((Kollision.Y * Skalierung) + (int)(Kollision.Height * Skalierung));
                Geschwindigkeit.Y = 0;
            }
        }

        public void ChangeStatus(Enemystate state)
        {
            if (state != State)
            {
                State = state;
                for (int i = 0; i < Textur.Count(); i++)
                {
                    if (Texturname[i] == state.ToString())
                    {
                        Animation.Texture = Textur[i]; Animation.Spalte = Spalten[i]; Animation.Geschwindigkeit = ((float)1 / AniGeschwindigkeit[i]); Animation.Wiederholen = Wiederholen[i]; break;
                    }
                }
                Animation.Abgeschlossen = false;
                Animation.AktuelleSpalte = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            Animation.Draw(spriteBatch, new Vector2(PositionCurrent.X - player.PositionCurrent.X, PositionCurrent.Y), Color.White, Blickrichtung, Skalierung);
        }
    }
}
