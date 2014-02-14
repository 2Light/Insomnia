using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    enum Actiontype
    {
        Neu,
        Depression,
        Effekt,
        Gegneraktion,
        Objektaktion,
        Animation,
        Hintergrund,
        Checkpoint,
        Levelwechsel,
        Grundkollision,
        Klettern,
        Musik,
        NoiseUp,
        NoiseLow,
        
    }

    enum Effektwechsel
    {
        Einschalten,
        Ausschalten,
        Umschalten,
    }

    enum Gegneraktionstyp
    {
        Erstellen,
        Entfernen,
        Status,
    }

    enum Objektaktionstyp
    {
        Erstellen,
        Entfernen,
        Austauschen,
    }

    class Eventaction
    {
        public Actiontype Type;

        private Mathe Mathe;
        private Player Player;
        private Levelmanager Levelmanager;
        private Enemymanager Enemymanager;
        private DepressionHandler DepressionHandler;
        private Eventmanager Eventmanager;
        private Objectmanager Objectmanager;
        private Objectdatabase Objectdatabase;

        public int Wert;    //Depressionsänderung, Objektvariante...
        public bool Ausrichtung;    //Depression steigern/senken (true = steigern),...
        public DepressionState DepressionState = DepressionState.GrayScal;
        public Effektwechsel Effektwechsel = Effektwechsel.Einschalten;
        public Gegneraktionstyp Gegneraktion = Gegneraktionstyp.Erstellen;
        public Enemytype Gegnertyp;
        public Enemystate Gegnerstatus;
        public Objektaktionstyp Objektaktion = Objektaktionstyp.Erstellen;
        public ObjectType Objecttyp;
        public float Skalierung = 1f;
        public int Layer = 5;
        public Objektebene Objektebene = Objektebene.Spielfeld;
        BackgroundMusic bgMusik;
        public Richtung Richtung;
        public Vector2 Position;
        public string Name;
        public bool Statisch = true;
        //public Enemy Gegner;

        public Eventaction(Mathe mathe, Player player, Levelmanager levelmanager, Enemymanager enemymanager, DepressionHandler depressionhandler, Eventmanager eventmanager, Objectmanager objectmanager, Objectdatabase objectdatabase,BackgroundMusic musik) //Konstruktor Editor
        {
            this.Mathe = mathe;
            this.Player = player;
            this.Levelmanager = levelmanager;
            this.Name = levelmanager.Levels[0].Name;
            this.Enemymanager = enemymanager;
            this.DepressionHandler = depressionhandler;
            this.Eventmanager = eventmanager;
            this.Objectmanager = objectmanager;
            this.Objectdatabase = objectdatabase;
            this.Objecttyp = Objectdatabase.Auslesen(ObjectClass.Stadtdekoration);
            this.bgMusik = musik;
        }

        //public Eventaction(Mathe mathe, Player player, Actiontype type, DepressionHandler depressionhandler, Vector2 position) //Kontruktor Ladevorgang
        //{
        //    this.Mathe = mathe;
        //    this.Player = player;
        //    this.DepressionHandler = depressionhandler;
        //    this.Type = type;
            
        //}

        public void DoActions()
        {
            switch (Type)
            {
                case Actiontype.Animation:

                    break;
                case Actiontype.Depression:
                    if (Ausrichtung) Player.DepressionSoll += (float)Wert / 100;
                    else Player.DepressionSoll -= (float)Wert / 100;
                    break;
                case Actiontype.Effekt:
                    switch (Effektwechsel)
                    {
                        case Effektwechsel.Einschalten: if (!DepressionHandler.isSet(DepressionState)) DepressionHandler.startDepression(DepressionState); break;
                        case Effektwechsel.Ausschalten: if (DepressionHandler.isSet(DepressionState)) DepressionHandler.startDepression(DepressionState); break;
                        case Effektwechsel.Umschalten: DepressionHandler.setFlag(DepressionState); break;
                    }
                    break;
                case Actiontype.Gegneraktion:
                    Enemy TempEnemy = Enemymanager.Enemydatabase.Read(Gegnertyp);
                    switch (Gegneraktion)
                    {
                        case Gegneraktionstyp.Erstellen: Enemymanager.AddEnemy(Gegnertyp, new Vector2(Position.X - (TempEnemy.Texturmaße.X / 2), Position.Y - (TempEnemy.Texturmaße.Y / 2)), Richtung, 1); break;
                        case Gegneraktionstyp.Entfernen: Enemymanager.RemoveEnemy(Enemymanager.PickEnemy(Gegnertyp, new Vector2(Position.X, Position.Y))); break;
                        case Gegneraktionstyp.Status: Enemymanager.PickEnemy(Gegnertyp, Position).ChangeStatus(Gegnerstatus); break;
                    }
                    break;
                case Actiontype.Hintergrund:

                    break;
                case Actiontype.Objektaktion:
                    switch (Objektaktion)
                    {
                        case Objektaktionstyp.Erstellen: Objectmanager.AddObject(Objecttyp, Wert, Position, Skalierung, Layer, Objektebene, Statisch); break;
                        case Objektaktionstyp.Entfernen: Object TempObject = Objectmanager.PositionÜberprüfen(Objecttyp, new Vector3(Position.X, Position.Y, Layer), Objektebene);
                            if (TempObject != null)
                            {
                                switch (Objektebene)
                                {
                                    case Objektebene.Hintergrund: Objectmanager.DeleteHintergrund.Add(TempObject); break;
                                    case Objektebene.Spielfeld: Objectmanager.DeleteSpielebene.Add(TempObject); break;
                                    case Objektebene.Vordergrund: Objectmanager.DeleteVordergrund.Add(TempObject); break;
                                }
                            }
                            break;
                        case Objektaktionstyp.Austauschen: Objectmanager.ChangeVariation(Objecttyp, new Vector3(Position.X, Position.Y, Layer), Objektebene, Wert); break;
                    }
                    break;
                case Actiontype.Checkpoint: Levelmanager.Checkpoint = new Vector2(Player.PositionCurrent.X, Player.PositionCurrent.Y - 1); break;
                case Actiontype.Levelwechsel: Eventmanager.Levelwechsel = Name; break;
                case Actiontype.Grundkollision: Player.Groundcollision = false; break;
                case Actiontype.Klettern: if ((Player.PositionCurrent.Y + 200 - Player.Kollision.Height > Position.X) && (Player.PositionCurrent.Y + 200 < Position.Y)) Player.ClimbEnabled = true; break;
                case Actiontype.Musik: this.bgMusik.currentSong.breakLoop(); break;
                case Actiontype.NoiseUp: Game1.onGo = 1; break;
                case Actiontype.NoiseLow: Game1.onGo = 2; break;

            }
        }
    }
}
