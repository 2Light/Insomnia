using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DyingHope
{
    class Key
    {
        public bool Flanke;         //Taste in diesem Frame betätigt
        public bool Gedrückt;       //Taste betätigt
        public int Flankenbefehl;   //ID der Funktion welche die Tastenflanke auslöst
        public int Befehl;          //ID der Funktion welche die Taste auslöst
        public Keys Taste;            //Referenz der Taste

        public Key(int flankenbefehl, int befehl, Keys taste)
        {
            this.Flanke = true;
            this.Gedrückt = false;
            this.Flankenbefehl = flankenbefehl;
            this.Befehl = befehl;
            this.Taste = taste;
        }

        public int Update(bool gedrückt)    //Zyklische Bearbeitung
        {
            if (gedrückt == true)   //Tastenflanke wird gesetzt wenn Button gedrückt und nicht gesetzt
            {
                Gedrückt = true;
                if (Flanke == false)    //Wenn Taste in diesem Frame betätigt wurde
                {
                    Flanke = true;  //Flankenmerker setzen
                    return Flankenbefehl;   //Flankenbefehl zurückgeben
                }
                return Befehl;  //Wenn Taste schon länger betätigt -> eventuellen Dauerbefehl zurückgeben
            }
            else    //Rücksetzen wenn Button losgelassen
            {
                Gedrückt = false;
                Flanke = false;
            }
            return 0;   //Wenn Taste nicht gedrückt "Befehl 0" (Nichts tun)
        }
    }
}
