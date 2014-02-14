using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DyingHope
{
    enum Eventzustand
    {
        Wartet, //Wartet auf Start
        Gestartet,
        Beendet,
    }

    class Event
    {
        public string Name;
        public Eventzustand Zustand;
        public bool Repeat;
        public int CooldownStart;
        public int CooldownCurrent;

        public List<Eventtrigger> Triggers = new List<Eventtrigger>();
        public List<Eventcondition> Conditions = new List<Eventcondition>();
        public List<Eventaction> Actions = new List<Eventaction>();

        public Event() { }  //Managerkonstruktor

        public Event(string name)   //Editorkonstruktor
        {
            this.Zustand = Eventzustand.Wartet;
            this.Name = name;
        }
    }
}
