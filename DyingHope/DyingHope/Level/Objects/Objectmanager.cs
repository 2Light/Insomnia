    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace DyingHope
{
    enum Objektebene
    {
        Spielfeld,
        Hintergrund,
        Vordergrund,
    }

    class Objectmanager : IdebugObjekt
    {
        public List<Object> ObjectsHintergrund = new List<Object>();
        public List<Object> DeleteHintergrund = new List<Object>();
        public List<Object> ObjectsSpielebene = new List<Object>();
        public List<Object> DeleteSpielebene = new List<Object>();
        public List<Object> ObjectsVordergrund = new List<Object>();
        public List<Object> DeleteVordergrund = new List<Object>();
        public Levelmanager Levelmanager;
        private Objectdatabase Objektdatenbank;
        private Animationsmanager Animationsmanager;
        private Player Player;
        public float Gravitation = 0.40f;
        public float Reibung = 0.35f;

        #region debugStuff

        public String debugString { get; set; }

        public StringBuilder stringBuilder { get; set; }
        public DebugFlag debugFlag { get; set; }

        public void addString(String text)
        {
            this.stringBuilder.AppendLine(text);
        }

        public void clearString()
        {
            this.stringBuilder.Clear();
            this.debugString = String.Empty;
        }

        public void DrawExtraDebug(DebugFlag extraFlag,SpriteBatch batch, Camera cam, Contents content)
        {

            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.ViewMatrix);
            foreach (Object Object in ObjectsSpielebene)
            {
                if ((extraFlag & DebugFlag.CollisonShape) == DebugFlag.CollisonShape)
                {
                    batch.Draw(content.Block, new Rectangle((int)614, (int)Player.PositionCurrent.Y+47, 61, 153), Color.Blue*0.5f );
                    if (Object.Objektvariante.Kollision != null)
                    {
                        foreach (Objectcollision Kollision in Object.Objektvariante.Kollision)
                        {
                            batch.Draw(content.Block, new Rectangle((int)(Object.PositionCurrent.X + (Kollision.Zone.X * Object.Skalierung) - Player.PositionCurrent.X), (int)(Object.PositionCurrent.Y + (Kollision.Zone.Y * Object.Skalierung)), (int)((float)Kollision.Zone.Width * Object.Skalierung), (int)((float)Kollision.Zone.Height * Object.Skalierung)), Color.Yellow);
                            batch.DrawString(content.Meiryo8, Kollision.Zone.ToString(), new Vector2(Object.PositionCurrent.X + Kollision.Zone.X - Player.PositionCurrent.X, Object.PositionCurrent.Y + Kollision.Zone.Y + 40), Color.Red);
                        }
                    }
                }
                if ((extraFlag & DebugFlag.ObjectInformation) == DebugFlag.ObjectInformation)
                {
                    batch.DrawString(content.Meiryo8, "Typ: " + Object.Typ.ToString() + " Variante: " + Object.Variante.ToString(), new Vector2(Object.PositionCurrent.X - Player.PositionCurrent.X, Object.PositionCurrent.Y), Color.Red);
                    batch.DrawString(content.Meiryo8, "Skalierung: " + Object.Skalierung.ToString(), new Vector2(Object.PositionCurrent.X - Player.PositionCurrent.X, Object.PositionCurrent.Y + 10), Color.Red);
                    batch.DrawString(content.Meiryo8, Object.PositionCurrent.ToString(), new Vector2(Object.PositionCurrent.X - Player.PositionCurrent.X, Object.PositionCurrent.Y + 20), Color.Red);
                    batch.DrawString(content.Meiryo8, Object.Objektvariante.Texturausschnitt.ToString(), new Vector2(Object.PositionCurrent.X - Player.PositionCurrent.X, Object.PositionCurrent.Y + 30), Color.Red);
                    batch.DrawString(content.Meiryo8, "Layer: " + Object.Layer.ToString(), new Vector2(Object.PositionCurrent.X - Player.PositionCurrent.X, Object.PositionCurrent.Y + 40), Color.Red);
                }
            }
            batch.End();

        }

        public void handelDebug()
        {
           addString("Objekttypen geladen: " + this.Objektdatenbank.Objektdaten.Count().ToString());
           addString("Objekte auf Karte: " + this.ObjectsSpielebene.Count().ToString());
        }
        #endregion

        public Objectmanager(Objectdatabase objektdatenbank, Animationsmanager animationsmanager, Player player, DebugFlag flag)
        {
            this.debugFlag = flag;
            this.stringBuilder = new StringBuilder();

            this.Objektdatenbank = objektdatenbank;
            this.Animationsmanager = animationsmanager;
            this.Player = player;
            Player.Objectmanager = this;
        }

        public void Update()
        {
            foreach (Object objekt in ObjectsSpielebene)
            {
                if (!objekt.Statisch)
                {
                    objekt.PositionLast = new Vector2(objekt.PositionCurrent.X, objekt.PositionCurrent.Y);   //Alte Position speichern für Kollisionsrücksetzung
                    objekt.PositionCurrent += objekt.Geschwindigkeit; //Geschwindigkeit verarbeiten
                    Physik(objekt);
                    Objectcollision TempCollision = Kollisionsabfrage(objekt, new Vector2(objekt.PositionCurrent.X, objekt.PositionLast.Y));
                    if (TempCollision != null)  //Objekt kollidiert mit anderem Objekt auf X-Achse
                    {
                        objekt.PositionCurrent.X = objekt.PositionLast.X;
                        objekt.Geschwindigkeit.X = 0f;
                    }
                    TempCollision = Kollisionsabfrage(objekt, new Vector2(objekt.PositionLast.X, objekt.PositionCurrent.Y));
                    if (TempCollision != null)  //Objekt kollidiert mit anderem Objekt auf Y-Achse
                    {
                        objekt.PositionCurrent.Y = objekt.PositionLast.Y;
                        objekt.Geschwindigkeit.Y = 0f;
                    }
                }
            }
            foreach (Object objekt in DeleteHintergrund) ObjectsHintergrund.Remove(objekt);
            foreach (Object objekt in DeleteSpielebene) ObjectsSpielebene.Remove(objekt);
            foreach (Object objekt in DeleteVordergrund) ObjectsVordergrund.Remove(objekt);
            DeleteHintergrund.Clear();
            DeleteSpielebene.Clear();
            DeleteVordergrund.Clear();
        }

        public void Physik(Object objekt)
        {
            //Reibung/Luftwiderstand--------------------------------------------------------------------------------
            if (objekt.Geschwindigkeit.X > 0)
            {
                objekt.Geschwindigkeit.X -= Reibung;
                if (objekt.Geschwindigkeit.X < 0) objekt.Geschwindigkeit.X = 0;
            }
            else if (objekt.Geschwindigkeit.X < 0)
            {
                objekt.Geschwindigkeit.X += Reibung;
                if (objekt.Geschwindigkeit.X > 0) objekt.Geschwindigkeit.X = 0;
            }
            //Graviation--------------------------------------------------------------------------------------------
            objekt.Geschwindigkeit.Y += Gravitation;   //Graviation erhöht Fallgeschwindigkeit
            if (objekt.Geschwindigkeit.Y > objekt.MaxSpeed.Y) objekt.Geschwindigkeit.Y = objekt.MaxSpeed.Y; //Maximale Fallgeschwindigkeit
            //Boden-------------------------------------------------------------------------------------------------
            if (objekt.PositionCurrent.Y >= Levelmanager.AktuellesLevel.Walkline + 200 - objekt.Objektvariante.Texturausschnitt.Height)   //Landung
            {
                objekt.PositionCurrent.Y = Levelmanager.AktuellesLevel.Walkline + 200 - objekt.Objektvariante.Texturausschnitt.Height;
                objekt.Geschwindigkeit.Y = 0;
            }
        }

        public void ObjektEditieren(Vector3 position, float skalierung, ObjectClass objectclass, int variante, Objektebene objektebene, bool statisch)
        {
            AddObject(new Object(Objektdatenbank, Animationsmanager, objectclass, variante, new Vector2(position.X, position.Y), skalierung, (int)position.Z, objektebene, statisch));
            Bubblesort();
        }

        public Object PositionÜberprüfen(Vector3 position, Objektebene ebene)
        {
            switch (ebene)
            {
                case Objektebene.Hintergrund: for (int i = 0; i < ObjectsHintergrund.Count(); i++) { if ((position.Z == ObjectsHintergrund[i].Layer) && (new Rectangle((int)(ObjectsHintergrund[i].PositionCurrent.X), (int)ObjectsHintergrund[i].PositionCurrent.Y, (int)(ObjectsHintergrund[i].Objektvariante.Texturausschnitt.Width * ObjectsHintergrund[i].Skalierung), (int)(ObjectsHintergrund[i].Objektvariante.Texturausschnitt.Height * ObjectsHintergrund[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsHintergrund[i]; } } break;
                case Objektebene.Spielfeld: for (int i = 0; i < ObjectsSpielebene.Count(); i++) { if ((position.Z == ObjectsSpielebene[i].Layer) && (new Rectangle((int)(ObjectsSpielebene[i].PositionCurrent.X), (int)ObjectsSpielebene[i].PositionCurrent.Y, (int)(ObjectsSpielebene[i].Objektvariante.Texturausschnitt.Width * ObjectsSpielebene[i].Skalierung), (int)(ObjectsSpielebene[i].Objektvariante.Texturausschnitt.Height * ObjectsSpielebene[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsSpielebene[i]; } } break;
                case Objektebene.Vordergrund: for (int i = 0; i < ObjectsVordergrund.Count(); i++) { if ((position.Z == ObjectsVordergrund[i].Layer) && (new Rectangle((int)(ObjectsVordergrund[i].PositionCurrent.X), (int)ObjectsVordergrund[i].PositionCurrent.Y, (int)(ObjectsVordergrund[i].Objektvariante.Texturausschnitt.Width * ObjectsVordergrund[i].Skalierung), (int)(ObjectsVordergrund[i].Objektvariante.Texturausschnitt.Height * ObjectsVordergrund[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsVordergrund[i]; } } break;
            }
            return null;
        }

        public Object PositionÜberprüfen(ObjectType typ, Vector3 position, Objektebene ebene)
        {
            switch (ebene)
            {
                case Objektebene.Hintergrund: for (int i = 0; i < ObjectsHintergrund.Count(); i++) { if ((typ.ObjectClass == ObjectsHintergrund[i].Typ) && (position.Z == ObjectsHintergrund[i].Layer) && (new Rectangle((int)(ObjectsHintergrund[i].PositionCurrent.X), (int)ObjectsHintergrund[i].PositionCurrent.Y, (int)(ObjectsHintergrund[i].Objektvariante.Texturausschnitt.Width * ObjectsHintergrund[i].Skalierung), (int)(ObjectsHintergrund[i].Objektvariante.Texturausschnitt.Height * ObjectsHintergrund[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsHintergrund[i]; } } break;
                case Objektebene.Spielfeld: for (int i = 0; i < ObjectsSpielebene.Count(); i++) { if ((typ.ObjectClass == ObjectsSpielebene[i].Typ) && (position.Z == ObjectsSpielebene[i].Layer) && (new Rectangle((int)(ObjectsSpielebene[i].PositionCurrent.X), (int)ObjectsSpielebene[i].PositionCurrent.Y, (int)(ObjectsSpielebene[i].Objektvariante.Texturausschnitt.Width * ObjectsSpielebene[i].Skalierung), (int)(ObjectsSpielebene[i].Objektvariante.Texturausschnitt.Height * ObjectsSpielebene[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsSpielebene[i]; } } break;
                case Objektebene.Vordergrund: for (int i = 0; i < ObjectsVordergrund.Count(); i++) { if ((typ.ObjectClass == ObjectsVordergrund[i].Typ) && (position.Z == ObjectsVordergrund[i].Layer) && (new Rectangle((int)(ObjectsVordergrund[i].PositionCurrent.X), (int)ObjectsVordergrund[i].PositionCurrent.Y, (int)(ObjectsVordergrund[i].Objektvariante.Texturausschnitt.Width * ObjectsVordergrund[i].Skalierung), (int)(ObjectsVordergrund[i].Objektvariante.Texturausschnitt.Height * ObjectsVordergrund[i].Skalierung)).Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))) { return ObjectsVordergrund[i]; } } break;
            }
            return null;
        }

        public void ChangeVariation(ObjectType typ, Vector3 position, Objektebene ebene, int variante)    //Variation innerhalb eines Typs um "modifikator" ändern    (z.B. für Türen/Schalter)
        {
            Object TempObjekt = PositionÜberprüfen(typ, new Vector3(position.X, position.Y, position.Z), ebene);
            if (TempObjekt != null) 
                switch (ebene)
                {
                    case Objektebene.Hintergrund: ObjectsHintergrund[ObjectsHintergrund.IndexOf(TempObjekt)] = new Object(Objektdatenbank, Animationsmanager, TempObjekt.Typ, variante, TempObjekt.PositionCurrent, TempObjekt.Skalierung, TempObjekt.Layer, TempObjekt.Objektebene, TempObjekt.Statisch); break;
                    case Objektebene.Spielfeld: ObjectsSpielebene[ObjectsSpielebene.IndexOf(TempObjekt)] = new Object(Objektdatenbank, Animationsmanager, TempObjekt.Typ, variante, TempObjekt.PositionCurrent, TempObjekt.Skalierung, TempObjekt.Layer, TempObjekt.Objektebene, TempObjekt.Statisch); break; break;
                    case Objektebene.Vordergrund: ObjectsVordergrund[ObjectsVordergrund.IndexOf(TempObjekt)] = new Object(Objektdatenbank, Animationsmanager, TempObjekt.Typ, variante, TempObjekt.PositionCurrent, TempObjekt.Skalierung, TempObjekt.Layer, TempObjekt.Objektebene, TempObjekt.Statisch); break; break;
                }
        }

        public void AddObject(Object objekt)
        {
            switch (objekt.Objektebene)
            {
                case Objektebene.Hintergrund: ObjectsHintergrund.Add(objekt); break;
                case Objektebene.Spielfeld: ObjectsSpielebene.Add(objekt); break;
                case Objektebene.Vordergrund: ObjectsVordergrund.Add(objekt); break;
            }
        }

        public void AddObject(ObjectType typ, int variante, Vector2 position, float skalierung, int layer, Objektebene objektebene, bool statisch)
        {
            switch (objektebene)
            {
                case Objektebene.Hintergrund: ObjectsHintergrund.Add(new Object(Objektdatenbank, Animationsmanager, typ.ObjectClass, variante, new Vector2(position.X - (typ.Variante[variante].Texturausschnitt.Width / 2), position.Y - (typ.Variante[variante].Texturausschnitt.Height / 2)), skalierung, layer, objektebene, statisch)); break;
                case Objektebene.Spielfeld: ObjectsSpielebene.Add(new Object(Objektdatenbank, Animationsmanager, typ.ObjectClass, variante, new Vector2(position.X - (typ.Variante[variante].Texturausschnitt.Width / 2), position.Y - (typ.Variante[variante].Texturausschnitt.Height / 2)), skalierung, layer, objektebene, statisch)); break;
                case Objektebene.Vordergrund: ObjectsVordergrund.Add(new Object(Objektdatenbank, Animationsmanager, typ.ObjectClass, variante, new Vector2(position.X - (typ.Variante[variante].Texturausschnitt.Width / 2), position.Y - (typ.Variante[variante].Texturausschnitt.Height / 2)), skalierung, layer, objektebene, statisch)); break;
            }
        }

        public void LoadObjects(string filename)
        {
            int Zeiger = 0;
            ObjectsHintergrund.Clear();
            ObjectsSpielebene.Clear();
            ObjectsVordergrund.Clear();
            Object TempObject = new Object();

            XmlReader reader = XmlReader.Create(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.LocalName)
                        {
                            case "NeuesObjekt": TempObject = new Object(); break;
                            case "PositionX": Zeiger = 1; break;
                            case "PositionY": Zeiger = 2; break;
                            case "Ebene": Zeiger = 3; break;
                            case "Typ": Zeiger = 4; break;
                            case "Skalierung": Zeiger = 5; break;
                            case "Objektebene": Zeiger = 6; break;
                            case "Statisch": Zeiger = 7; break;
                            case "Variante": Zeiger = 8; break;
                            default: Zeiger = 0; break;
                        }
                        break;
                    case XmlNodeType.Text:
                        switch (Zeiger)
                        {
                            case 1: TempObject.PositionCurrent = new Vector2(Convert.ToInt32(reader.Value), 0); break;
                            case 2: TempObject.PositionCurrent = new Vector2(TempObject.PositionCurrent.X, Convert.ToInt32(reader.Value)); break;
                            case 3: TempObject.Layer = Convert.ToInt32(reader.Value); break;
                            case 4: TempObject.Typ = (ObjectClass)Enum.Parse(typeof(ObjectClass), reader.Value); break;
                            case 5: TempObject.Skalierung = 0f + (Convert.ToInt32(reader.Value) / 100f); break;
                            case 6: TempObject.Objektebene = (Objektebene)Enum.Parse(typeof(Objektebene), reader.Value); break;
                            case 7: TempObject.Statisch = Convert.ToBoolean(reader.Value); break;
                            case 8: TempObject.Variante = Convert.ToInt32(reader.Value); AddObject(new Object(Objektdatenbank, Animationsmanager, TempObject.Typ, TempObject.Variante, TempObject.PositionCurrent, TempObject.Skalierung, TempObject.Layer, TempObject.Objektebene, TempObject.Statisch)); break;
                            default: break;
                        }
                        break;
                }
            }
            reader.Close();
            AddObject(new Object(Objektdatenbank, Animationsmanager, ObjectClass.Nichts, 0, new Vector2(10000, 10000), 1, 2, Objektebene.Hintergrund, true));    //Dummy damit Spieler/NPC nie letztes Objekt
            AlteMapsBereinigen();   // <-------------------- Entfernen sobald Maps bzw Assets final
            Bubblesort();
        }

        public void SaveObjects(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter writer = XmlWriter.Create(filename, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Objects");

            for (int i = 0; i < ObjectsHintergrund.Count; i++)   //Hintergrundobjekte speichern
            {
                if (ObjectsHintergrund[i].Typ != ObjectClass.Nichts)
                {
                    writer.WriteStartElement("NeuesObjekt");
                    writer.WriteStartElement("PositionX");
                    writer.WriteValue(Math.Round(ObjectsHintergrund[i].PositionCurrent.X));
                    writer.WriteEndElement();
                    writer.WriteStartElement("PositionY");
                    writer.WriteValue(Math.Round(ObjectsHintergrund[i].PositionCurrent.Y));
                    writer.WriteEndElement();
                    writer.WriteStartElement("Ebene");
                    writer.WriteValue(ObjectsHintergrund[i].Layer);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Typ");
                    writer.WriteValue(ObjectsHintergrund[i].Typ.ToString());
                    writer.WriteEndElement();
                    if (ObjectsHintergrund[i].Skalierung != 1)
                    {
                        writer.WriteStartElement("Skalierung");
                        writer.WriteValue(Math.Round(ObjectsHintergrund[i].Skalierung * 100));
                        writer.WriteEndElement();
                    }
                    if (ObjectsHintergrund[i].Objektebene != Objektebene.Spielfeld)
                    {
                        writer.WriteStartElement("Objektebene");
                        writer.WriteValue(ObjectsHintergrund[i].Objektebene.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("Variante");
                    writer.WriteValue(ObjectsHintergrund[i].Variante);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            for (int i = 0; i < ObjectsSpielebene.Count; i++)   //Spielebene speichern
            {
                writer.WriteStartElement("NeuesObjekt");
                writer.WriteStartElement("PositionX");
                writer.WriteValue(Math.Round(ObjectsSpielebene[i].PositionCurrent.X));
                writer.WriteEndElement();
                writer.WriteStartElement("PositionY");
                writer.WriteValue(Math.Round(ObjectsSpielebene[i].PositionCurrent.Y));
                writer.WriteEndElement();
                writer.WriteStartElement("Ebene");
                writer.WriteValue(ObjectsSpielebene[i].Layer);
                writer.WriteEndElement();
                writer.WriteStartElement("Typ");
                writer.WriteValue(ObjectsSpielebene[i].Typ.ToString());
                writer.WriteEndElement();
                if (ObjectsSpielebene[i].Skalierung != 1)
                {
                    writer.WriteStartElement("Skalierung");
                    writer.WriteValue(Math.Round(ObjectsSpielebene[i].Skalierung * 100));
                    writer.WriteEndElement();
                }
                if (ObjectsSpielebene[i].Objektebene != Objektebene.Spielfeld)
                {
                    writer.WriteStartElement("Objektebene");
                    writer.WriteValue(ObjectsSpielebene[i].Objektebene.ToString());
                    writer.WriteEndElement();
                }
                if (!ObjectsSpielebene[i].Statisch)
                {
                    writer.WriteStartElement("Statisch");
                    writer.WriteValue(ObjectsSpielebene[i].Statisch.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteStartElement("Variante");
                writer.WriteValue(ObjectsSpielebene[i].Variante);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            for (int i = 0; i <ObjectsVordergrund.Count; i++)   //Vordergrundobjekte speichern
            {
                writer.WriteStartElement("NeuesObjekt");
                writer.WriteStartElement("PositionX");
                writer.WriteValue(Math.Round(ObjectsVordergrund[i].PositionCurrent.X));
                writer.WriteEndElement();
                writer.WriteStartElement("PositionY");
                writer.WriteValue(Math.Round(ObjectsVordergrund[i].PositionCurrent.Y));
                writer.WriteEndElement();
                writer.WriteStartElement("Ebene");
                writer.WriteValue(ObjectsVordergrund[i].Layer);
                writer.WriteEndElement();
                writer.WriteStartElement("Typ");
                writer.WriteValue(ObjectsVordergrund[i].Typ.ToString());
                writer.WriteEndElement();
                if (ObjectsVordergrund[i].Skalierung != 1)
                {
                    writer.WriteStartElement("Skalierung");
                    writer.WriteValue(Math.Round(ObjectsVordergrund[i].Skalierung * 100));
                    writer.WriteEndElement();
                }
                if (ObjectsVordergrund[i].Objektebene != Objektebene.Spielfeld)
                {
                    writer.WriteStartElement("Objektebene");
                    writer.WriteValue(ObjectsVordergrund[i].Objektebene.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteStartElement("Variante");
                writer.WriteValue(ObjectsVordergrund[i].Variante);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        private void Bubblesort()   //Sortiert die Liste nach der Bearbeitungsreihenfolge
        {
            bool sortiert;
            do
            {
                sortiert = true;
                for (int i = 0; i < (ObjectsVordergrund.Count() - 1); i++)
                {
                    if (ObjectsVordergrund[i].BearbeitungsID > ObjectsVordergrund[i + 1].BearbeitungsID)
                    {
                        Object TauschObjekt1 = ObjectsVordergrund[i];
                        Object TauschObjekt2 = ObjectsVordergrund[i + 1];
                        ObjectsVordergrund[i] = TauschObjekt2;
                        ObjectsVordergrund[i + 1] = TauschObjekt1;
                        sortiert = false;
                    }
                }
            }
            while (!sortiert);  //Hintergrundobjekte sortieren
            do
            {
                sortiert = true;
                for (int i = 0; i < (ObjectsSpielebene.Count() - 1); i++)
                {
                    if (ObjectsSpielebene[i].BearbeitungsID > ObjectsSpielebene[i + 1].BearbeitungsID)
                    {
                        Object TauschObjekt1 = ObjectsSpielebene[i];
                        Object TauschObjekt2 = ObjectsSpielebene[i + 1];
                        ObjectsSpielebene[i] = TauschObjekt2;
                        ObjectsSpielebene[i + 1] = TauschObjekt1;
                        sortiert = false;
                    }
                }
            }
            while (!sortiert);  //Spielobjekte sortieren
            do
            {
                sortiert = true;
                for (int i = 0; i < (ObjectsVordergrund.Count() - 1); i++)
                {
                    if (ObjectsVordergrund[i].BearbeitungsID > ObjectsVordergrund[i + 1].BearbeitungsID)
                    {
                        Object TauschObjekt1 = ObjectsVordergrund[i];
                        Object TauschObjekt2 = ObjectsVordergrund[i + 1];
                        ObjectsVordergrund[i] = TauschObjekt2;
                        ObjectsVordergrund[i + 1] = TauschObjekt1;
                        sortiert = false;
                    }
                }
            }
            while (!sortiert);  //Vordergrundobjekte sortieren
        }

        private void AlteMapsBereinigen()   //Alte, nicht mehr vorhandene Assets aus Levels löschen
        {
            foreach (Object delete in ObjectsVordergrund) if (delete.Objektvariante == null) DeleteVordergrund.Add(delete);
            foreach (Object delete in ObjectsSpielebene) if (delete.Objektvariante == null) DeleteSpielebene.Add(delete);
            foreach (Object delete in ObjectsHintergrund) if (delete.Objektvariante == null) DeleteHintergrund.Add(delete);
            Update();
        }

        //public Objectcollision Kollisionsabfrage(Rectangle zone)    //Abfrage Spieler Alt
        //{
        //    foreach (Object objekt in ObjectsSpielebene)
        //    {
        //        if (objekt.Objektvariante.Kollision != null)
        //            foreach (Objectcollision kollision in objekt.Objektvariante.Kollision)
        //            {
        //                if (zone.Intersects(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)))) return new Objectcollision(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)));
        //            }
        //    }
        //    return null;
        //}

        public Objectcollision Kollisionsabfrage(Rectangle zone, bool xabfrage)    //Abfrage Spieler
        {
            foreach (Object objekt in ObjectsSpielebene)
            {
                if (objekt.Objektvariante.Kollision != null)
                    foreach (Objectcollision kollision in objekt.Objektvariante.Kollision)
                    {
                        if (zone.Intersects(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung))))
                        {
                            if (xabfrage)
                            {
                                if (!objekt.Statisch)
                                {
                                    if (Player.PositionCurrent.X > Player.PositionLast.X)   //Rechtsbewegung
                                    {
                                        Objectcollision TempCollision = Kollisionsabfrage(objekt, new Vector2(objekt.PositionCurrent.X + (Player.PositionCurrent.X - Player.PositionLast.X), objekt.PositionCurrent.Y));
                                        if (TempCollision == null)
                                        {
                                            //objekt.PositionCurrent.X += (Player.PositionCurrent.X - Player.PositionLast.X);
                                            objekt.PositionCurrent.X += 2f;
                                        }
                                        else return new Objectcollision(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)));
                                    }
                                    if (Player.PositionLast.X > Player.PositionCurrent.X)   //Linksbewegung
                                    {
                                        Objectcollision TempCollision = Kollisionsabfrage(objekt, new Vector2(objekt.PositionCurrent.X + (Player.PositionLast.X - Player.PositionCurrent.X), objekt.PositionCurrent.Y));
                                        if (TempCollision == null)
                                        {
                                            //objekt.PositionCurrent.X -= (Player.PositionLast.X - Player.PositionCurrent.X);
                                            objekt.PositionCurrent.X -= 2f;
                                        }
                                        else return new Objectcollision(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)));
                                    }
                                }
                                else return new Objectcollision(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)));
                            }
                            else return new Objectcollision(new Rectangle((int)(objekt.PositionCurrent.X + (kollision.Zone.X * objekt.Skalierung)), (int)(objekt.PositionCurrent.Y + (kollision.Zone.Y * objekt.Skalierung)), (int)((float)kollision.Zone.Width * objekt.Skalierung), (int)((float)kollision.Zone.Height * objekt.Skalierung)));
                        }
                    }
            }
            return null;
        }

        public Objectcollision Kollisionsabfrage(Object Objekt1, Vector2 Position)  //Abfrage Objekt
        {
            foreach (Objectcollision Kollision1 in Objekt1.Objektvariante.Kollision)
            {
                foreach (Object Objekt2 in ObjectsSpielebene)
                {
                    if ((Objekt1 != Objekt2) && (Objekt2.Objektvariante != null))
                    {
                        if (Objekt2.Objektvariante.Kollision != null)
                            foreach (Objectcollision Kollision2 in Objekt2.Objektvariante.Kollision)
                            {
                                if (new Rectangle((int)(Position.X + (Kollision1.Zone.X * Objekt1.Skalierung)), (int)(Position.Y + (Kollision1.Zone.Y * Objekt1.Skalierung)), (int)((float)Kollision1.Zone.Width * Objekt1.Skalierung), (int)((float)Kollision1.Zone.Height * Objekt1.Skalierung)).Intersects(new Rectangle((int)(Objekt2.PositionCurrent.X + (Kollision2.Zone.X * Objekt2.Skalierung)), (int)(Objekt2.PositionCurrent.Y + (Kollision2.Zone.Y * Objekt2.Skalierung)), (int)((float)Kollision2.Zone.Width * Objekt2.Skalierung), (int)((float)Kollision2.Zone.Height * Objekt2.Skalierung))))
                                    return new Objectcollision(new Rectangle((int)(Objekt2.PositionCurrent.X + (Kollision2.Zone.X * Objekt2.Skalierung)), (int)(Objekt2.PositionCurrent.Y + (Kollision2.Zone.Y * Objekt2.Skalierung)), (int)((float)Kollision2.Zone.Width * Objekt2.Skalierung), (int)((float)Kollision2.Zone.Height * Objekt2.Skalierung)));
                            }
                    }
                }
            }
            return null;
        }

        public void Draw(SpriteBatch spriteBatch)    //Hintergrund und Spielfeldebene
        {
            foreach (Object Object in ObjectsHintergrund) Object.Draw(spriteBatch, Player, Color.White);
            foreach (Object Object in ObjectsSpielebene) Object.Draw(spriteBatch, Player, Color.White);
        }

        public void DrawVordergrund(SpriteBatch spriteBatch)    //Ebene vor dem Spieler/Spielfeld
        {
            foreach (Object Object in ObjectsVordergrund)
            {
                Object.Draw(spriteBatch, Player, new Color(Levelmanager.AktuellesLevel.VordergrundAbdunkelung, Levelmanager.AktuellesLevel.VordergrundAbdunkelung, Levelmanager.AktuellesLevel.VordergrundAbdunkelung));
            }
        }
    }
}
