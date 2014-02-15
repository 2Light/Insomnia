using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    enum Conditiontype
    {
        Neu,
        Depression,
        Position,
        Schalter,
    }

    class Eventcondition
    {
        public Conditiontype Type;
        public Vector2 Position = new Vector2(0, 0);
        public int Depression = 50;  //Prozentualer Depressionszustand des Spielers
        public bool Ausrichtung;    //Wert muss über- oder unterschritten werden (true = überschreiten)
        public Lever Lever;

        private Mathe Mathe;
        private Player Player;

        public Eventcondition(Mathe mathe, Player player) //Konstruktor Editor
        {
            this.Mathe = mathe;
            this.Player = player;
        }

        public Eventcondition(Mathe mathe, Player player, Conditiontype type, Vector2 position) //Kontruktor Ladevorgang
        {
            this.Mathe = mathe;
            this.Player = player;
            this.Type = type;
            this.Position = position;
        }

        public bool CheckCondition()
        {
            switch (Type)
            {
                case Conditiontype.Position:
                    if (Ausrichtung) { if (Player.PositionCurrent.X + 640 > Position.X) return true; }
                    else { if (Player.PositionCurrent.X + 640 < Position.X) return true; }
                    break;
                case Conditiontype.Depression:
                    if (Ausrichtung) { if ((Player.DepressionIst * 100) > Depression) return true; }
                    else { if ((Player.DepressionIst * 100) < Depression) return true; }
                    break;
                case Conditiontype.Schalter:
                    if ((Ausrichtung) && (Lever.Betätigt)) return true;
                    if ((!Ausrichtung) && (!Lever.Betätigt)) return true;
                    break;
            }
            return false;
        }
    }
}
