using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Net;
using System.IO;
using System.Xml;

namespace DyingHope
{
    class Eventmanager
    {
        public List<Event> Events = new List<Event>();
        public List<Event> DeleteEvents = new List<Event>();

        private Mathe Mathe;
        private Player Player;
        public Levelmanager Levelmanager;
        private Enemymanager Enemymanager;
        private DepressionHandler DepressionHandler;
        private Objectmanager Objectmanager;
        private Objectdatabase Objectdatabase;
        private BackgroundMusic bgMusik;
        private Levermanager Levermanager;
        public string Levelwechsel = "";

        public Eventmanager(Mathe mathe, Player player, Enemymanager enemymanager, DepressionHandler depressionhandler, Objectmanager objectmanager, Objectdatabase objectdatabase, Levermanager levermanager)
        {
            this.Mathe = mathe;
            this.Player = player;
            this.Enemymanager = enemymanager;
            this.DepressionHandler = depressionhandler;
            this.Objectmanager = objectmanager;
            this.Objectdatabase = objectdatabase;
            this.Levermanager = levermanager;
        }

        public void Update()
        {
            foreach (Event Event in Events) //Alle Events durchgehen...
            {
                if (Event.Zustand == Eventzustand.Wartet)  //Event wartet auf Auslöser
                {
                    bool Activate = false;
                    foreach (Eventtrigger trigger in Event.Triggers) //Wenn nicht -> Auslöser prüfen
                    {
                        if (trigger.CheckTrigger()) Activate = true;    //Wenn gegeben -> Startbedingungsüberprüfung 
                    }
                    if (Event.Triggers.Count() == 0) Activate = true;   //Event Bedarf keiner Auslöser -> starten
                    if (Activate)   //Mindestens 1 Auslöser erfüllt
                    {
                        foreach (Eventcondition condition in Event.Conditions)  //Alle Bedingungen überprüfen
                        {
                            if (!condition.CheckCondition()) Activate = false;  //Bei einer nichterfüllung -> Activierung abbrechen
                        }
                        if (Activate) Event.Zustand = Eventzustand.Gestartet;  //Alle Bedingungen erfüllt -> Event starten
                    }
                }
                if (Event.Zustand == Eventzustand.Gestartet)   //Event läuft..
                {
                    foreach (Eventaction action in Event.Actions)
                    {
                        action.DoActions();   //Aktionen ausführen
                    }
                    Event.Zustand = Eventzustand.Beendet;
                    if (Event.Repeat)if (Event.CooldownStart != 0) Event.CooldownCurrent = Event.CooldownStart;  //Ggf Timer starten
                }
                if (Event.Zustand == Eventzustand.Beendet)  //Event abgeschlossen -> Prüfen/Warten auf Neustart
                {
                    if (Event.Repeat)
                    {
                        if (Event.CooldownCurrent == 0) Event.Zustand = Eventzustand.Wartet;   //Neustartverögerung abgelaufen -> Event Startbereit
                        else Event.CooldownCurrent--;
                    }
                    //DeleteEvents.Add(Event);  //Event löschen -> während der Entwicklung deaktiviert damit abgeschlossene Events im Editor einsehbar bleiben
                }
            }
            if (Levelwechsel != "")
            {
                Levelmanager.LoadLevel(Levelwechsel);
                Levelwechsel = "";
            }
            foreach (Event delete in DeleteEvents) Events.Remove(delete);
        }

        public void NewEvent()
        {
            Events.Add(new Event("Neues Event"));
        }

        public void LoadEvents(string filename)
        {
            int Zeiger = 0;
            int Zeiger2 = 0;
            Events.Clear();
            Event TempEvent = new Event();
            Eventtrigger TempTrigger = new Eventtrigger(Enemymanager, Mathe, Player);
            Eventcondition TempCondition = new Eventcondition(Mathe, Player);
            Eventaction TempAction = new Eventaction(Mathe, Player, Levelmanager, Enemymanager, DepressionHandler, this, Objectmanager, Objectdatabase,bgMusik);

            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "NeuesEvent": TempEvent = new Event(); Events.Add(TempEvent); break;
                            case "Name": Zeiger = 1; break;
                            case "Repeat": Zeiger = 2; break;
                            case "Cooldown": Zeiger = 3; break;
                            case "NeuerAusloeser": TempTrigger = new Eventtrigger(Enemymanager, Mathe, Player); TempEvent.Triggers.Add(TempTrigger); Zeiger2 = 0; break;
                            case "NeueBedingung": TempCondition = new Eventcondition(Mathe, Player); TempEvent.Conditions.Add(TempCondition); Zeiger2 = 1; break;
                            case "NeueAktion": TempAction = new Eventaction(Mathe, Player, Levelmanager, Enemymanager, DepressionHandler, this, Objectmanager, Objectdatabase,bgMusik); TempEvent.Actions.Add(TempAction); Zeiger2 = 2; break;
                            case "Typ": Zeiger = 4; break;
                            case "PositionX": Zeiger = 5; break;
                            case "PositionY": Zeiger = 6; break;
                            case "Depression": Zeiger = 7; break;
                            case "Ausrichtung": Zeiger = 8; break;
                            case "Wert": Zeiger = 9; break;
                            case "Effekttyp": Zeiger = 10; break;
                            case "Effektaktion": Zeiger = 11; break;
                            case "Gegneraktion": Zeiger = 12; break;
                            case "Gegnertyp": Zeiger = 13; break;
                            case "Gegnerstatus": Zeiger = 14; break;
                            case "Richtung": Zeiger = 15; break;
                            case "Levelname": Zeiger = 16; break;
                            case "Objektaktion": Zeiger = 17; break;
                            case "Objekttyp": Zeiger = 18; break;
                            case "Skalierung": Zeiger = 19; break;
                            case "Layer": Zeiger = 20; break;
                            case "Objektebene": Zeiger = 21; break;
                            case "Schalter": Zeiger = 22; break;
                            case "Statisch": Zeiger = 23; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: TempEvent.Name = reader.Value; break;
                            case 2: TempEvent.Repeat = Convert.ToBoolean(reader.Value); break;
                            case 3: TempEvent.CooldownStart = Convert.ToInt32(reader.Value); break;
                            default: break;
                        }
                        switch (Zeiger2)
                        {
                            case 0: 
                                switch (Zeiger)
                                {
                                    case 4: TempTrigger.Type = (Triggertype)Enum.Parse(typeof(Triggertype), reader.Value); break;
                                    case 5: TempTrigger.Position = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                                    case 6: TempTrigger.Position = new Vector2(TempTrigger.Position.X, Convert.ToInt32(reader.Value)); break;
                                    case 7: TempTrigger.Wert = Convert.ToInt32(reader.Value); break;
                                    case 8: TempTrigger.Ausrichtung = Convert.ToBoolean(reader.Value); break;
                                    case 22: TempTrigger.Lever = Levermanager.GetLever(reader.Value); break;
                                    case 13: TempTrigger.Enemytype = (Enemytype)Enum.Parse(typeof(Enemytype), reader.Value); break;
                                    case 14: TempTrigger.Enemystate = (Enemystate)Enum.Parse(typeof(Enemystate), reader.Value); break;
                                    default: break;
                                }
                                break;
                            case 1: 
                                switch (Zeiger)
                                {
                                    case 4: TempCondition.Type = (Conditiontype)Enum.Parse(typeof(Conditiontype), reader.Value); break;
                                    case 5: TempCondition.Position = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                                    case 6: TempCondition.Position = new Vector2(TempCondition.Position.X, Convert.ToInt32(reader.Value)); break;
                                    case 7: TempCondition.Depression = Convert.ToInt32(reader.Value); break;
                                    case 8: TempCondition.Ausrichtung = Convert.ToBoolean(reader.Value); break;
                                    case 22: TempCondition.Lever = Levermanager.GetLever(reader.Value); break;
                                    default: break;
                                }
                                break;
                            case 2: 
                                switch (Zeiger)
                                {
                                    case 4: TempAction.Type = (Actiontype)Enum.Parse(typeof(Actiontype), reader.Value); break;
                                    case 5: TempAction.Position = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                                    case 6: TempAction.Position = new Vector2(TempAction.Position.X, Convert.ToInt32(reader.Value)); break;
                                    case 8: TempAction.Ausrichtung = Convert.ToBoolean(reader.Value); break;
                                    case 9: TempAction.Wert = Convert.ToInt32(reader.Value); break;
                                    case 10: TempAction.DepressionState = (DepressionState)Enum.Parse(typeof(DepressionState), reader.Value); break;
                                    case 11: TempAction.Effektwechsel = (Effektwechsel)Enum.Parse(typeof(Effektwechsel), reader.Value); break;
                                    case 12: TempAction.Gegneraktion = (Gegneraktionstyp)Enum.Parse(typeof(Gegneraktionstyp), reader.Value); break;
                                    case 13: TempAction.Gegnertyp = (Enemytype)Enum.Parse(typeof(Enemytype), reader.Value); break;
                                    case 14: TempAction.Gegnerstatus = (Enemystate)Enum.Parse(typeof(Enemystate), reader.Value); break;
                                    case 15: TempAction.Richtung = (Richtung)Enum.Parse(typeof(Richtung), reader.Value); break;
                                    case 16: TempAction.Name = reader.Value; break;
                                    case 17: TempAction.Objektaktion = (Objektaktionstyp)Enum.Parse(typeof(Objektaktionstyp), reader.Value); break;
                                    case 18: TempAction.Objecttyp = Objectdatabase.Auslesen((ObjectClass)Enum.Parse(typeof(ObjectClass), reader.Value)); break;
                                    case 19: TempAction.Skalierung = 0f + (Convert.ToInt32(reader.Value) / 100f); break;
                                    case 20: TempAction.Layer = Convert.ToInt32(reader.Value); break;
                                    case 21: TempAction.Objektebene = (Objektebene)Enum.Parse(typeof(Objektebene), reader.Value); break;
                                    case 23: TempAction.Statisch = Convert.ToBoolean(reader.Value); break;
                                    default: break;
                                }
                                break;
                            default: break;
                        }
                        break;
                }
            }
            reader.Close();
        }

        public void SaveEvents(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Events");
                for (int i = 0; i < Events.Count; i++)
                {
                    writer.WriteStartElement("NeuesEvent");
                        writer.WriteStartElement("Name");
                        writer.WriteValue(Events[i].Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Repeat");
                        writer.WriteValue(Events[i].Repeat.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("Cooldown");
                        writer.WriteValue(Events[i].CooldownStart.ToString());
                        writer.WriteEndElement();
                        foreach (Eventtrigger trigger in Events[i].Triggers)
                        {
                            writer.WriteStartElement("NeuerAusloeser");
                                writer.WriteStartElement("Typ");
                                writer.WriteValue(trigger.Type.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionX");
                                writer.WriteValue(Math.Round(trigger.Position.X).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionY");
                                writer.WriteValue(Math.Round(trigger.Position.Y).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Depression");
                                writer.WriteValue(trigger.Wert.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Ausrichtung");
                                writer.WriteValue(trigger.Ausrichtung.ToString());
                                writer.WriteEndElement();
                                if (trigger.Lever != null)
                                {
                                    writer.WriteStartElement("Schalter");
                                    writer.WriteValue(trigger.Lever.Name);
                                    writer.WriteEndElement();
                                }
                                writer.WriteStartElement("Gegnertyp");
                                writer.WriteValue(trigger.Enemytype.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Gegnerstatus");
                                writer.WriteValue(trigger.Enemystate.ToString());
                                writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        foreach (Eventcondition condition in Events[i].Conditions)
                        {
                            writer.WriteStartElement("NeueBedingung");
                                writer.WriteStartElement("Typ");
                                writer.WriteValue(condition.Type.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionX");
                                writer.WriteValue(Math.Round(condition.Position.X).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionY");
                                writer.WriteValue(Math.Round(condition.Position.Y).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Depression");
                                writer.WriteValue(condition.Depression.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Ausrichtung");
                                writer.WriteValue(condition.Ausrichtung.ToString());
                                writer.WriteEndElement();
                                if (condition.Lever != null)
                                {
                                    writer.WriteStartElement("Schalter");
                                    writer.WriteValue(condition.Lever.Name);
                                    writer.WriteEndElement();
                                }
                            writer.WriteEndElement();
                        }
                        foreach (Eventaction action in Events[i].Actions)
                        {
                            writer.WriteStartElement("NeueAktion");
                                writer.WriteStartElement("Typ");
                                writer.WriteValue(action.Type.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Wert");
                                writer.WriteValue(action.Wert.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Ausrichtung");
                                writer.WriteValue(action.Ausrichtung.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Effekttyp");
                                writer.WriteValue(action.DepressionState.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Effektaktion");
                                writer.WriteValue(action.Effektwechsel.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Gegneraktion");
                                writer.WriteValue(action.Gegneraktion.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Gegnertyp");
                                writer.WriteValue(action.Gegnertyp.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Gegnerstatus");
                                writer.WriteValue(action.Gegnerstatus.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Richtung");
                                writer.WriteValue(action.Richtung.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionX");
                                writer.WriteValue(Math.Round(action.Position.X).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("PositionY");
                                writer.WriteValue(Math.Round(action.Position.Y).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Levelname");
                                writer.WriteValue(action.Name.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Objektaktion");
                                writer.WriteValue(action.Objektaktion.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Objekttyp");
                                writer.WriteValue(action.Objecttyp.ObjectClass.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Skalierung");
                                writer.WriteValue(Math.Round(action.Skalierung * 100).ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Layer");
                                writer.WriteValue(action.Layer.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Objektebene");
                                writer.WriteValue(action.Objektebene.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("Statisch");
                                writer.WriteValue(action.Statisch.ToString());
                                writer.WriteEndElement();
                        writer.WriteEndElement();
                        }
                    writer.WriteEndElement();
                }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
