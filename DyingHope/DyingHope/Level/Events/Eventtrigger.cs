using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    enum Triggertype
    {
        Neu,
        Position,
        Depression,
        Schalter,
        Intervall,
        Gegner,
    }

    class Eventtrigger
    {
        private Enemymanager Enemymanager;
        public Triggertype Type;
        public Vector2 Position = new Vector2(0, 0);
        public int Wert = 50;  //Prozentualer Depressionszustand des Spielers / Intervallhöhe Trigger
        private int Intervall;  //Zähler Intervall
        public bool Ausrichtung;    //Depression muss Wert über- oder unterschreiten (true = überschreiten)
        public Lever Lever;
        public Enemytype Enemytype;
        public Enemystate Enemystate;

        private Mathe Mathe;
        private Player Player;

        public Eventtrigger(Enemymanager enemymanager, Mathe mathe, Player player) //Konstruktor Editor
        {
            this.Mathe = mathe;
            this.Player = player;
            this.Enemymanager = enemymanager;
        }

        public Eventtrigger(Enemymanager enemymanager, Mathe mathe, Player player, Triggertype type, Vector2 position) //Kontruktor Ladevorgang
        {
            this.Mathe = mathe;
            this.Player = player;
            this.Type = type;
            this.Position = position;
            this.Enemymanager = enemymanager;
        }

        public bool CheckTrigger()
        {
            switch (Type)
            {
                case Triggertype.Position: if (Mathe.Differenz(Player.PositionCurrent.X + 640, Position.X) < 10) return true; break;
                case Triggertype.Depression:
                    if (Ausrichtung) { if ((Player.DepressionIst * 100) > Wert) return true; }
                    else { if ((Player.DepressionIst * 100) < Wert) return true; }
                    break;
                case Triggertype.Schalter: if (Lever.BetätigtFlanke) return true;
                    break;
                case Triggertype.Intervall:
                    if (Intervall == Wert) { Intervall = 0; return true; }
                    else Intervall++;
                    break;
                case Triggertype.Gegner:
                    Enemy Tempenemy = Enemymanager.PickEnemy(Enemytype, Position);
                    if (Tempenemy != null)
                    {
                        if (Tempenemy.State == Enemystate) return true;
                    }
                    break;
            }
            return false;
        }
    }
}
