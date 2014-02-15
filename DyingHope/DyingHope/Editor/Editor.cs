using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    enum Editortool
    {
        Start,
        Editoreinstellungen,
        Neu,
        Laden,
        Leveleinstellungen,
        Hintergruende,
        Hintergrund,
        Hintergrundtextur,
        Objektkategorie,
        Objektvariante,
        Schalter,
        Schaltereinstellungen,
        Events,
        Event,
        Ausloeser,
        Bedingung,
        Aktion,
        Typ,
        Gegner,
        Items,
    }

    class Editor
    {
        //Klassen--------------------------------------------------------------------------------------------------------------------------
        private Windowmanager Windowmanager;
        private Enemymanager Enemymanager;
        private Enemydatabase Enemydatabase;
        private Backgroundmanager Backgroundmanager;
        private Backgrounddatabase Backgrounddatabase;
        private Objectmanager Objectmanager;
        private Objectdatabase Objectdatabase;
        private Eventmanager Eventmanager;
        private Contents Contents;
        private Cursor Cursor;
        private Player Player;
        private Keymanager Keymanager;
        private Levelmanager Levelmanager;
        private Itemmanager Itemmanager;
        private Levermanager Levermanager;
        //Editoreinstellungen--------------------------------------------------------------------------------------------------------------
        public Editortool Tool;
        public bool VectorWaehlen;  //der nächste Linksklick speichert einen Vector für ein Event
        public bool NurX; //Bei Vectorwahl nur X speichern
        public bool NurY; //Bei Vectorwahl nur Y speichern
        public int Raster = 32;
        public int RasterOffset = 0;    //Verschiebung des Nullpunktes bei Rasterplatzierung
        public bool Rasterplatzierung;
        //Temporäre Variablen--------------------------------------------------------------------------------------------------------------
        private int LäuferY;
        private int LäuferLinksklick;
        public int RückmeldungAnzeigen;
        //Aktuelle Auswahl der Einstellungen und Objekte im UI-----------------------------------------------------------------------------
        public int Autosaveintervall = 0;
        public int AutosaveRest;
        public int AuswahlLayer = 6;
        public int AuswahlScrolling = 20;
        public float AuswahlSkalierung = 1.0f;
        public Background AuswahlHintergrund;
        public ObjectType AuswahlObjektkategorie;
        public Objectvariation AuswahlObjektvariation;
        public Objectvariation VorschauObjektvariation;
        public Event AuswahlEvent;
        public Eventtrigger AuswahlTrigger;
        public Eventcondition AuswahlCondition;
        public Eventaction AuswahlAction;
        public Enemy AuswahlGegner;
        public Enemy VorschauGegner;
        public Richtung AuswahlRichtung = Richtung.Links;
        public Objektebene AuswahlObjektebene = Objektebene.Spielfeld;
        public Item AuswahlItem;
        public bool AuswahlHeilung;
        public Item VorschauItem;
        public Lever AuswahlLever;
        public bool AuswahlStatisch = true;
        //Anzeigeoptionen------------------------------------------------------------------------------------------------------------------
        public bool AnzeigenKollision = false;
        public bool AnzeigenEvents = true;

        public Editor(Windowmanager windowmanager, Enemymanager enemymanager, Backgroundmanager backgroundmanager, Backgrounddatabase backgrounddatabase, Objectmanager objectmanager, Objectdatabase objectdatabase, Eventmanager eventmanager, Contents contents, Cursor cursor, Player player, Keymanager keymanager, Levelmanager levelmanager, Itemmanager itemmanager, Levermanager levermanager)
        {
            Tool = Editortool.Start;
            this.Windowmanager = windowmanager;
            this.Enemymanager = enemymanager;
            this.Enemydatabase = enemymanager.Enemydatabase;
            this.Backgroundmanager = backgroundmanager;
            this.Backgroundmanager.Editor = this;
            this.Backgrounddatabase = backgrounddatabase;
            this.Objectmanager = objectmanager;
            this.Objectdatabase = objectdatabase;
            this.Eventmanager = eventmanager;
            this.Contents = contents;
            this.Cursor = cursor;
            this.Cursor.Editor = this;
            this.Player = player;
            this.Keymanager = keymanager;
            this.Levelmanager = levelmanager;
            this.Itemmanager = itemmanager;
            this.Levermanager = levermanager;
        }

        public void NewLevel(string name)
        {
            Levelmanager.NewLevel(name);
        }

        public void Update()
        {
            if (Cursor.Leftclick)
            {
                if (!Windowmanager.MouseOverGUI)
                {
                    switch (Tool)
                    {
                        case Editortool.Start: break;
                        case Editortool.Leveleinstellungen: if (VectorWaehlen) { Levelmanager.AktuellesLevel.Startposition = new Vector2(Cursor.Mouseposition.X + Player.PositionCurrent.X - 100 - 640, Cursor.Mouseposition.Y - 100); VectorWaehlen = false; } break;
                        case Editortool.Objektvariante: if (AuswahlObjektvariation != null)
                                if (Rasterplatzierung) Objectmanager.ObjektEditieren(new Vector3((int)(Cursor.Mouseposition.X + Player.PositionCurrent.X - (((float)AuswahlObjektvariation.Texturausschnitt.Width / 2) * AuswahlSkalierung) - ((Player.PositionCurrent.X + Cursor.Mouseposition.X) % Raster)), (int)(Cursor.Mouseposition.Y - (((float)AuswahlObjektvariation.Texturausschnitt.Height / 2) * AuswahlSkalierung) - (Cursor.Mouseposition.Y % Raster) + RasterOffset), AuswahlLayer - 1), AuswahlSkalierung, AuswahlObjektkategorie.ObjectClass, AuswahlObjektkategorie.Variante.IndexOf(AuswahlObjektvariation), AuswahlObjektebene, AuswahlStatisch);
                                else Objectmanager.ObjektEditieren(new Vector3((int)(Cursor.Mouseposition.X + Player.PositionCurrent.X - (((float)AuswahlObjektvariation.Texturausschnitt.Width / 2) * AuswahlSkalierung)), (int)(Cursor.Mouseposition.Y - (((float)AuswahlObjektvariation.Texturausschnitt.Height / 2) * AuswahlSkalierung)), AuswahlLayer - 1), AuswahlSkalierung, AuswahlObjektkategorie.ObjectClass, AuswahlObjektkategorie.Variante.IndexOf(AuswahlObjektvariation), AuswahlObjektebene, AuswahlStatisch);
                            break;
                        case Editortool.Gegner: if (AuswahlGegner != null) Enemymanager.AddEnemy(AuswahlGegner.Typ, new Vector2(Cursor.Mouseposition.X + Player.PositionCurrent.X - (((float)AuswahlGegner.Texturmaße.X / 2) * AuswahlSkalierung), Cursor.Mouseposition.Y - (((float)AuswahlGegner.Texturmaße.Y / 2) * AuswahlSkalierung)), AuswahlRichtung, AuswahlSkalierung); break;
                        case Editortool.Ausloeser: if (VectorWaehlen)
                            {
                                if (AuswahlTrigger.Type == Triggertype.Schalter) AuswahlTrigger.Lever = Levermanager.GetLever(Cursor.Mouseposition);
                                else AuswahlTrigger.Position = new Vector2((Cursor.Mouseposition.X + Player.PositionCurrent.X), Cursor.Mouseposition.Y);
                                VectorWaehlen = false;
                            }
                            break;
                        case Editortool.Bedingung: if (VectorWaehlen) 
                            {
                                if (AuswahlCondition.Type == Conditiontype.Schalter) AuswahlCondition.Lever = Levermanager.GetLever(Cursor.Mouseposition);
                                else AuswahlCondition.Position = new Vector2((Cursor.Mouseposition.X + Player.PositionCurrent.X), Cursor.Mouseposition.Y); 
                                VectorWaehlen = false; 
                            } 
                            break;
                        case Editortool.Aktion: if (VectorWaehlen) 
                        {
                            if (AuswahlAction.Type == Actiontype.Klettern)
                            {
                                if (NurX) AuswahlAction.Position = new Vector2(Cursor.Mouseposition.Y, AuswahlAction.Position.Y);
                                else if (NurY) AuswahlAction.Position = new Vector2(AuswahlAction.Position.X, Cursor.Mouseposition.Y); 
                            }
                            else AuswahlAction.Position = new Vector2((Cursor.Mouseposition.X + Player.PositionCurrent.X), Cursor.Mouseposition.Y);
                            VectorWaehlen = false; NurY = false; NurX = false;
                        } 
                        break;
                        case Editortool.Items: Itemmanager.AddItem(AuswahlItem.Name, new Vector2(Cursor.Mouseposition.X + Player.PositionCurrent.X, Cursor.Mouseposition.Y), AuswahlHeilung); break;
                        case Editortool.Schaltereinstellungen: if (VectorWaehlen) { AuswahlLever.Position = new Vector2((Cursor.Mouseposition.X + Player.PositionCurrent.X - AuswahlLever.Mittelpunkt.X), Cursor.Mouseposition.Y - AuswahlLever.Mittelpunkt.Y); VectorWaehlen = false; } break;
                        case Editortool.Hintergrund:
                            if (VectorWaehlen)
                            {
                                if (NurX)
                                {
                                    AuswahlHintergrund.Startposition = (int)(Cursor.Mouseposition.X + (Player.PositionCurrent.X * AuswahlHintergrund.Scrollgeschwindigkeit.X));
                                    if (AuswahlHintergrund.Startposition < 1) AuswahlHintergrund.Startposition = 1;
                                }
                                if (NurY) AuswahlHintergrund.Endposition = (int)(Cursor.Mouseposition.X + (Player.PositionCurrent.X * AuswahlHintergrund.Scrollgeschwindigkeit.X));
                            }
                            VectorWaehlen = false; NurY = false; NurX = false;
                            break;
                    }
                }
                else if ((Windowmanager.Befehl != 3016) && (Windowmanager.Befehl != 3034) && (Windowmanager.Befehl != 3035) && (Windowmanager.Befehl != 1803)) VectorWaehlen = false;   //Klick aufs Interface beendet Vectorwahl, deaktiviert für den Zyklus in dem die Auswahl startet
            }
            else if (Cursor.Rightclick)
            {
                if (!Windowmanager.MouseOverGUI)
                {
                    switch (Tool)
                    {
                        case Editortool.Start: break;
                        case Editortool.Leveleinstellungen: break;
                        case Editortool.Objektkategorie:
                        case Editortool.Objektvariante:
                            switch (AuswahlObjektebene)
                            {
                                case Objektebene.Hintergrund: foreach (Object ObjektHintergrund in Objectmanager.ObjectsHintergrund) if (new Rectangle((int)(ObjektHintergrund.PositionCurrent.X - Player.PositionCurrent.X), (int)ObjektHintergrund.PositionCurrent.Y, (int)(ObjektHintergrund.Objektvariante.Texturausschnitt.Width * ObjektHintergrund.Skalierung), (int)(ObjektHintergrund.Objektvariante.Texturausschnitt.Height * ObjektHintergrund.Skalierung)).Intersects(new Rectangle((int)Cursor.Mouseposition.X, (int)Cursor.Mouseposition.Y, 1, 1))) Objectmanager.DeleteHintergrund.Add(ObjektHintergrund); break;
                                case Objektebene.Spielfeld: foreach (Object ObjektSpielfeld in Objectmanager.ObjectsSpielebene) if (new Rectangle((int)(ObjektSpielfeld.PositionCurrent.X - Player.PositionCurrent.X), (int)ObjektSpielfeld.PositionCurrent.Y, (int)(ObjektSpielfeld.Objektvariante.Texturausschnitt.Width * ObjektSpielfeld.Skalierung), (int)(ObjektSpielfeld.Objektvariante.Texturausschnitt.Height * ObjektSpielfeld.Skalierung)).Intersects(new Rectangle((int)Cursor.Mouseposition.X, (int)Cursor.Mouseposition.Y, 1, 1))) Objectmanager.DeleteSpielebene.Add(ObjektSpielfeld); break;
                                case Objektebene.Vordergrund: foreach (Object ObjektVordergrund in Objectmanager.ObjectsVordergrund) if (new Rectangle((int)(ObjektVordergrund.PositionCurrent.X - Player.PositionCurrent.X), (int)ObjektVordergrund.PositionCurrent.Y, (int)(ObjektVordergrund.Objektvariante.Texturausschnitt.Width * ObjektVordergrund.Skalierung), (int)(ObjektVordergrund.Objektvariante.Texturausschnitt.Height * ObjektVordergrund.Skalierung)).Intersects(new Rectangle((int)Cursor.Mouseposition.X, (int)Cursor.Mouseposition.Y, 1, 1))) Objectmanager.DeleteVordergrund.Add(ObjektVordergrund); break;
                            }
                            break;
                        case Editortool.Gegner: foreach (Enemy Enemy in Enemymanager.Enemys) if (new Rectangle((int)(Enemy.PositionCurrent.X - Player.PositionCurrent.X), (int)Enemy.PositionCurrent.Y, (int)(Enemy.Texturmaße.X * Enemy.Skalierung), (int)(Enemy.Texturmaße.Y * Enemy.Skalierung)).Intersects(new Rectangle((int)Cursor.Mouseposition.X, (int)Cursor.Mouseposition.Y, 1, 1))) Enemymanager.Delete.Add(Enemy); break;
                        case Editortool.Items: foreach (Item item in Itemmanager.Items) if (new Rectangle((int)(item.Feld.X - Player.PositionCurrent.X), (int)item.Feld.Y, item.Feld.Width, item.Feld.Height).Intersects(new Rectangle((int)Cursor.Mouseposition.X, (int)Cursor.Mouseposition.Y, 1, 1))) Itemmanager.RemoveItem(item); break;
                    }
                }
                VectorWaehlen = false;  //Vectorwahl abbrechen
            }
            if (RückmeldungAnzeigen > 0) RückmeldungAnzeigen--;
            Autosave();
        }

        private void Autosave()
        {
            if (Autosaveintervall != 0) //0 = deaktiviert
            {
                if (AutosaveRest >= Autosaveintervall)  //Timer abfragen
                {
                    RückmeldungAnzeigen = 60;   //Speicherung Anzeigen
                    AutosaveRest = 0;   //Timer Rücksetzen
                    Levelmanager.AutoSaveEditor();  //Autosave durchführen
                }
                else AutosaveRest++;
            }
        }

        public void VorschauRuecksetzen()   //Inputmanager setzt Vorschau zurück und prüft anschließend auf neue Vorschau
        {
            VorschauObjektvariation = null;
            VorschauGegner = null;
            VorschauItem = null;
        }

        public void ChangeTool(Editortool tool)
        {
            if (Tool != tool)
            {
                Tool = tool;
                switch (tool)
                {
                    case Editortool.Editoreinstellungen: Windowmanager.AddWindow(Windowtype.Editoreinstellungen); break;
                    case Editortool.Neu: Windowmanager.AddWindow(Windowtype.Neu); break;
                    case Editortool.Laden:
                        Windowmanager.AddWindow(Windowtype.Laden);
                        LäuferY = 40;
                        LäuferLinksklick = 310;
                        foreach (Level level in Levelmanager.Levels)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Laden).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, level.Name, false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Leveleinstellungen:
                        Windowmanager.AddWindow(Windowtype.Leveleinstellungen);
                        break;
                    case Editortool.Hintergruende:
                        AuswahlHintergrund = null;
                        Windowmanager.AddWindow(Windowtype.Hintergruende);
                        Windowmanager.GetWindowOfType(Windowtype.Hintergruende).Feld = Windowmanager.GetWindowPosition(Windowtype.Hintergrund);
                        LäuferY = 40;
                        LäuferLinksklick = 610;
                        foreach (Background Background in Backgroundmanager.Backgrounds)
                        {
                            if (Background.Textur != null) Windowmanager.GetWindowOfType(Windowtype.Hintergruende).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, Background.Name.ToString() + " " + Background.Position.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            else Windowmanager.GetWindowOfType(Windowtype.Hintergruende).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, "Leerer Hintergrund " + Background.Position.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Hintergrund:
                        Windowmanager.AddWindow(Windowtype.Hintergrund);
                        Windowmanager.GetWindowOfType(Windowtype.Hintergrund).Feld = Windowmanager.GetWindowPosition(Windowtype.Hintergruende);
                        break;
                    case Editortool.Hintergrundtextur:
                        Windowmanager.AddWindow(Windowtype.Hintergrundtextur);
                        Windowmanager.GetWindowOfType(Windowtype.Hintergrundtextur).Feld = Windowmanager.GetWindowPosition(Windowtype.Hintergrund);
                        LäuferY = 40;
                        LäuferLinksklick = 410;
                        foreach (Background background in Backgrounddatabase.Backgrounds)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Hintergrundtextur).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, background.Name, false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Objektkategorie:
                        AuswahlObjektkategorie = null;
                        AuswahlObjektvariation = null;
                        Windowmanager.RemoveWindow(Windowtype.Event);
                        Windowmanager.AddWindow(Windowtype.Objektkategorie);
                        Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).Feld = Windowmanager.GetWindowPosition(Windowtype.Objektvariante);
                        LäuferY = 40;
                        LäuferLinksklick = 810;
                        foreach (ObjectType Objekttyp in Objectdatabase.Objektdaten)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, Objekttyp.ObjectClass.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Objektvariante:
                        Windowmanager.AddWindow(Windowtype.Objektvariante);
                        Windowmanager.GetWindowOfType(Windowtype.Objektvariante).Feld = Windowmanager.GetWindowPosition(Windowtype.Objektkategorie);
                        LäuferY = 40;
                        LäuferLinksklick = 910;
                        foreach (Objectvariation Variation in AuswahlObjektkategorie.Variante)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Objektvariante).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, Variation.Name, false, Color.Blue, LäuferLinksklick, 0, LäuferLinksklick + 40));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Events:
                        AnzeigenEvents = true;
                        AuswahlEvent = null;
                        Windowmanager.RemoveWindow(Windowtype.Ausloeser);
                        Windowmanager.RemoveWindow(Windowtype.Bedingung);
                        Windowmanager.RemoveWindow(Windowtype.Aktion);
                        Windowmanager.AddWindow(Windowtype.Events);
                        Windowmanager.GetWindowOfType(Windowtype.Events).Feld = Windowmanager.GetWindowPosition(Windowtype.Event);
                        LäuferY = 40;
                        LäuferLinksklick = 1010;
                        foreach (Event Event in Eventmanager.Events)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Events).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, Event.Name, false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Event:
                        Windowmanager.RemoveWindow(Windowtype.Event);
                        Windowmanager.RemoveWindow(Windowtype.Ausloeser);
                        Windowmanager.RemoveWindow(Windowtype.Bedingung);
                        Windowmanager.RemoveWindow(Windowtype.Aktion);
                        Windowmanager.AddWindow(Windowtype.Event);
                        Windowmanager.GetWindowOfType(Windowtype.Event).Feld = Windowmanager.GetWindowPosition(Windowtype.Events);
                        LäuferY = 40;
                        LäuferLinksklick = 1100;
                        foreach (Eventtrigger trigger in AuswahlEvent.Triggers)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Event).AddButton(new Button(new Rectangle(236, LäuferY, 200, 15), Contents.Meiryo8, trigger.Type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        LäuferY = 40;
                        LäuferLinksklick = 1125;
                        foreach (Eventcondition condition in AuswahlEvent.Conditions)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Event).AddButton(new Button(new Rectangle(462, LäuferY, 200, 15), Contents.Meiryo8, condition.Type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        LäuferY = 40;
                        LäuferLinksklick = 1150;
                        foreach (Eventaction action in AuswahlEvent.Actions)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Event).AddButton(new Button(new Rectangle(688, LäuferY, 200, 15), Contents.Meiryo8, action.Type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Ausloeser:
                        AuswahlCondition = null;
                        AuswahlAction = null;
                        Windowmanager.RemoveWindow(Windowtype.Ausloeser); 
                        Windowmanager.AddWindow(Windowtype.Ausloeser);
                        LäuferY = 70;
                        switch (AuswahlTrigger.Type)
                        {
                            case Triggertype.Position: 
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Position X-Achse", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 23, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Triggertype.Depression:
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Depression                                %", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 24, 3017, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 2), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ausrichtung", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 25, 3018, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Triggertype.Schalter:
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Schalter", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 55, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Triggertype.Intervall:
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Frameintervall", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 24, 3017, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Triggertype.Gegner:
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Position", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 67, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Gegnertyp", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 66, 3050, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Status", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Ausloeser, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 68, 3051, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                        }
                        break;
                    case Editortool.Bedingung:
                        AuswahlTrigger = null;
                        AuswahlAction = null;
                        Windowmanager.RemoveWindow(Windowtype.Bedingung);
                        Windowmanager.AddWindow(Windowtype.Bedingung);
                        LäuferY = 70;
                        switch (AuswahlCondition.Type)
                        {
                            case Conditiontype.Position: 
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ausrichtung", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 41, 3021, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Position X-Achse", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 26, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Conditiontype.Depression:
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Depression                                %", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 27, 3020, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 2), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ausrichtung", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 28, 3021, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Conditiontype.Schalter:
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Schalter", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 58, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Bedingung).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Zustand", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Bedingung, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 59, 3021, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                        }
                        break;
                    case Editortool.Aktion:
                        AuswahlTrigger = null;
                        AuswahlCondition = null;
                        Windowmanager.RemoveWindow(Windowtype.Aktion); 
                        Windowmanager.AddWindow(Windowtype.Aktion);
                        LäuferY = 70;
                        switch (AuswahlAction.Type)
                        {
                            case Actiontype.Animation: break;
                            case Actiontype.Depression:
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ausrichtung", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 32, 3023, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Depression                                %", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 33, 3024, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 2), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Actiontype.Effekt: 
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Effekttyp", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 34, 3025, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Aktion", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 35, 3026, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Actiontype.Gegneraktion:
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Gegneraktion", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 38, 3019, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Gegnertyp", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 36, 3027, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;    
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Position", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 37, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                switch (AuswahlAction.Gegneraktion)
                                {
                                    case Gegneraktionstyp.Erstellen:
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Blickrichtung", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 39, 3028, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                        break;
                                    case Gegneraktionstyp.Entfernen: break;
                                    case Gegneraktionstyp.Status:
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Gegnerstatus", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 40, 3029, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                        break;
                                }
                                break;
                            case Actiontype.Hintergrund: break;
                            case Actiontype.Objektaktion:
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Objektaktion", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 60, 3044, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Position", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 37, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Layer", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 62, 3046, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Objekttyp", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 61, 3047, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                switch (AuswahlAction.Objektaktion)
                                {
                                    case Objektaktionstyp.Erstellen: 
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Variante", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 64, 3045, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Skalierung", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 63, 0/*3048*/, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3), new Vector2(10, LäuferY)); LäuferY += 15;
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Statisch", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 70, 3053, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3), new Vector2(10, LäuferY)); LäuferY += 15;
                                        break;
                                    case Objektaktionstyp.Entfernen: break;
                                    case Objektaktionstyp.Austauschen:
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Objektvariation wechseln zu:", Contents.Meiryo8, Color.White, false)); LäuferY += 15;
                                        Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Variante", Contents.Meiryo8, Color.White, false));
                                        Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 64, 3045, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                        break;
                                }
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Objektebene", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(95, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 65, 3049, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Actiontype.Levelwechsel: 
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Kartenname", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 42, 3030, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;
                            case Actiontype.Klettern:
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Oberkante", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 48, 3034, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                Windowmanager.GetWindowOfType(Windowtype.Aktion).Elemente.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Unterkante", Contents.Meiryo8, Color.White, false));
                                Windowmanager.AddTextCell(Windowtype.Aktion, new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 49, 3035, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1), new Vector2(10, LäuferY)); LäuferY += 15;
                                break;                        
                        }
                        break;
                    case Editortool.Typ:
                        Windowmanager.AddWindow(Windowtype.Typ);
                        LäuferY = 40;
                        LäuferLinksklick = 1510;
                        if (AuswahlTrigger != null)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Typ).Feld = Windowmanager.GetWindowPosition(Windowtype.Ausloeser);
                            Windowmanager.RemoveWindow(Windowtype.Ausloeser);
                            foreach (Triggertype type in Enum.GetValues(typeof(Triggertype)))
                            {
                                Windowmanager.GetWindowOfType(Windowtype.Typ).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                                LäuferY += 15; LäuferLinksklick++;
                            }
                        }
                        else if (AuswahlCondition != null)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Typ).Feld = Windowmanager.GetWindowPosition(Windowtype.Bedingung);
                            Windowmanager.RemoveWindow(Windowtype.Bedingung);
                            foreach (Conditiontype type in Enum.GetValues(typeof(Conditiontype)))
                            {
                                Windowmanager.GetWindowOfType(Windowtype.Typ).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                                LäuferY += 15; LäuferLinksklick++;
                            }
                        }
                        else if (AuswahlAction != null)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Typ).Feld = Windowmanager.GetWindowPosition(Windowtype.Aktion);
                            Windowmanager.RemoveWindow(Windowtype.Aktion);
                            foreach (Actiontype type in Enum.GetValues(typeof(Actiontype)))
                            {
                                Windowmanager.GetWindowOfType(Windowtype.Typ).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, type.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                                LäuferY += 15; LäuferLinksklick++;
                            }
                        }
                        break;
                    case Editortool.Gegner:
                        AuswahlGegner = null;
                        Windowmanager.AddWindow(Windowtype.Gegner);
                        LäuferY = 40;
                        LäuferLinksklick = 1610;
                        foreach (Enemy enemy in Enemymanager.Enemydatabase.Enemys)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Gegner).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, enemy.Typ.ToString(), false, Color.Blue, LäuferLinksklick, 0, LäuferLinksklick + 40));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Items:
                        AuswahlItem = null;
                        Windowmanager.AddWindow(Windowtype.Items);
                        LäuferY = 40;
                        LäuferLinksklick = 1710;
                        foreach (Item item in Itemmanager.Datenbank)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Items).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, item.Name, false, Color.Blue, LäuferLinksklick, 0, LäuferLinksklick + 40));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Schalter:
                        AuswahlLever = null;
                        Windowmanager.RemoveWindow(Windowtype.Lever);
                        Windowmanager.AddWindow(Windowtype.Levers);
                        LäuferY = 40;
                        LäuferLinksklick = 1810;
                        foreach (Lever lever in Levermanager.Levers)
                        {
                            Windowmanager.GetWindowOfType(Windowtype.Levers).AddButton(new Button(new Rectangle(10, LäuferY, 120, 15), Contents.Meiryo8, lever.Name, false, Color.Blue, LäuferLinksklick, 0, 0));
                            LäuferY += 15; LäuferLinksklick++;
                        }
                        break;
                    case Editortool.Schaltereinstellungen:
                        Windowmanager.RemoveWindow(Windowtype.Levers); 
                        Windowmanager.AddWindow(Windowtype.Lever);
                        LäuferY = 70;

                        break;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if ((AnzeigenKollision) || (Tool == Editortool.Leveleinstellungen)) DrawKollision(spritebatch);
            if (AnzeigenEvents) DrawEventmarker(spritebatch);
            DrawZeiger(spritebatch);
        }

        private void DrawEventmarker(SpriteBatch spritebatch)
        {
            foreach (Event Event in Eventmanager.Events)
            {
                foreach (Eventtrigger Trigger in Event.Triggers)
                {
                    switch (Trigger.Type)
                    {
                        case Triggertype.Position:
                            spritebatch.Draw(Contents.Block, new Rectangle((int)(Trigger.Position.X - Player.PositionCurrent.X - 2), 0, 4, 1024), Color.Green);
                            TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Trigger.Position.X - Player.PositionCurrent.X, 512), Color.Red);
                            break;
                        case Triggertype.Gegner:
                            spritebatch.Draw(Contents.GUIParts, new Rectangle((int)(Trigger.Position.X - Player.PositionCurrent.X - 25), (int)Trigger.Position.Y - 25, 50, 50), new Rectangle(88, 28, 30, 30), Color.Blue);
                            TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Trigger.Position.X - Player.PositionCurrent.X, Trigger.Position.Y), Color.Red);
                            break;
                        case Triggertype.Schalter:
                            if (Trigger.Lever != null) TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Trigger.Lever.Position.X + Trigger.Lever.Mittelpunkt.X - Player.PositionCurrent.X, Trigger.Lever.Position.Y + Trigger.Lever.Mittelpunkt.Y), Color.Red);
                            break;
                    }
                }
                foreach (Eventcondition Condition in Event.Conditions)
                {
                    switch (Condition.Type)
                    {
                        case Conditiontype.Position:
                            spritebatch.Draw(Contents.Block, new Rectangle((int)(Condition.Position.X - Player.PositionCurrent.X - 2), 0, 4, 1024), Color.Blue);
                            if (Condition.Ausrichtung) spritebatch.Draw(Contents.GUIParts, new Vector2(Condition.Position.X - Player.PositionCurrent.X, 492), new Rectangle(0, 70, 30, 20), Color.Blue);
                            else spritebatch.Draw(Contents.GUIParts, new Vector2(Condition.Position.X - Player.PositionCurrent.X - 30, 492), new Rectangle(0, 90, 30, 20), Color.Blue);
                            TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Condition.Position.X - Player.PositionCurrent.X, 512), Color.Red);
                            break;
                        case Conditiontype.Schalter:
                            if (Condition.Lever != null) TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Condition.Lever.Position.X + Condition.Lever.Mittelpunkt.X - Player.PositionCurrent.X, Condition.Lever.Position.Y + Condition.Lever.Mittelpunkt.Y), Color.Red);
                            break;
                    }
                }
                foreach (Eventaction Action in Event.Actions)
                {
                    switch (Action.Type)
                    {
                        case Actiontype.Gegneraktion:
                            switch (Action.Gegneraktion)
                            {
                                case Gegneraktionstyp.Erstellen: Enemy TempEnemy = Enemydatabase.Read(Action.Gegnertyp);
                                    if (Action.Richtung == Richtung.Links) spritebatch.Draw(TempEnemy.Textur[0], new Rectangle((int)(Action.Position.X - Player.PositionCurrent.X - ((TempEnemy.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Action.Position.Y - ((TempEnemy.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(TempEnemy.Texturmaße.X * AuswahlSkalierung), (int)(TempEnemy.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, 0, (int)TempEnemy.Texturmaße.X, (int)TempEnemy.Texturmaße.Y), Color.Red * 0.3f);
                                    else if (Action.Richtung == Richtung.Rechts) spritebatch.Draw(TempEnemy.Textur[0], new Rectangle((int)(Action.Position.X - Player.PositionCurrent.X - ((TempEnemy.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Action.Position.Y - ((TempEnemy.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(TempEnemy.Texturmaße.X * AuswahlSkalierung), (int)(TempEnemy.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, (int)TempEnemy.Texturmaße.Y, (int)TempEnemy.Texturmaße.X, (int)TempEnemy.Texturmaße.Y), Color.Red * 0.3f);
                                    break;
                                case Gegneraktionstyp.Entfernen:
                                case Gegneraktionstyp.Status:
                                    spritebatch.Draw(Contents.GUIParts, new Rectangle((int)(Action.Position.X - Player.PositionCurrent.X - 25), (int)Action.Position.Y - 25, 50, 50), new Rectangle(88, 28, 30, 30), Color.Blue);
                                    break;
                            }
                            TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Action.Position.X - Player.PositionCurrent.X, Action.Position.Y), Color.Red);
                            break;
                        case Actiontype.Klettern:
                            spritebatch.Draw(Contents.Block, new Rectangle((int)(Event.Triggers[0].Position.X - Player.PositionCurrent.X - 35), (int)(Action.Position.X - 2), 70, 4), Color.Red);
                            spritebatch.Draw(Contents.Block, new Rectangle((int)(Event.Triggers[0].Position.X - Player.PositionCurrent.X - 35), (int)(Action.Position.Y - 2), 70, 4), Color.Red);
                            break;
                        case Actiontype.Objektaktion:
                            switch (Action.Objektaktion)
                            {
                                case Objektaktionstyp.Erstellen: spritebatch.Draw(Action.Objecttyp.Textur, new Vector2(Action.Position.X - Player.PositionCurrent.X - ((Action.Objecttyp.Variante[Action.Wert].Texturausschnitt.Width * Action.Skalierung) / 2), Action.Position.Y - ((Action.Objecttyp.Variante[Action.Wert].Texturausschnitt.Height * Action.Skalierung) / 2)), Action.Objecttyp.Variante[Action.Wert].Texturausschnitt, Color.Red * 0.3f); break;
                                case Objektaktionstyp.Entfernen:
                                case Objektaktionstyp.Austauschen: spritebatch.Draw(Contents.GUIParts, new Rectangle((int)(Action.Position.X - Player.PositionCurrent.X - 25), (int)Action.Position.Y - 25, 50, 50), new Rectangle(88, 28, 30, 30), Color.Blue); break;
                            }
                            TextZentriert(spritebatch, Contents.Meiryo8, Event.Name, new Vector2(Action.Position.X - Player.PositionCurrent.X, Action.Position.Y), Color.Red);
                            break;
                    }
                }
            }
        }

        private void DrawZeiger(SpriteBatch spritebatch)
        {
            if (!Windowmanager.MouseOverGUI)
            {
                switch (Tool)
                {
                    case Editortool.Leveleinstellungen:
                        if (VectorWaehlen) spritebatch.Draw(Contents.Block, new Rectangle((int)Cursor.Mouseposition.X - 100 + Player.Kollision.X, (int)Cursor.Mouseposition.Y- 100 + Player.Kollision.Y, 61, 153), Color.Blue * 0.5f); break;
                    case Editortool.Objektvariante:
                        if (AuswahlObjektvariation != null)
                            if (Rasterplatzierung) spritebatch.Draw(AuswahlObjektvariation.Textur, new Rectangle((int)(Cursor.Mouseposition.X - ((AuswahlObjektvariation.Texturausschnitt.Width * AuswahlSkalierung) / 2) - ((Player.PositionCurrent.X + Cursor.Mouseposition.X) % Raster)), (int)(Cursor.Mouseposition.Y - ((AuswahlObjektvariation.Texturausschnitt.Height * AuswahlSkalierung) / 2) - (Cursor.Mouseposition.Y % Raster) + RasterOffset), (int)(AuswahlObjektvariation.Texturausschnitt.Width * AuswahlSkalierung), (int)(AuswahlObjektvariation.Texturausschnitt.Height * AuswahlSkalierung)), AuswahlObjektvariation.Texturausschnitt, Color.White * 0.7f);
                            else spritebatch.Draw(AuswahlObjektvariation.Textur, new Rectangle((int)(Cursor.Mouseposition.X - ((AuswahlObjektvariation.Texturausschnitt.Width * AuswahlSkalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((AuswahlObjektvariation.Texturausschnitt.Height * AuswahlSkalierung) / 2)), (int)(AuswahlObjektvariation.Texturausschnitt.Width * AuswahlSkalierung), (int)(AuswahlObjektvariation.Texturausschnitt.Height * AuswahlSkalierung)), AuswahlObjektvariation.Texturausschnitt, Color.White * 0.7f);
                        break;
                    case Editortool.Gegner:
                        if (AuswahlGegner != null)
                            if (AuswahlRichtung == Richtung.Links) spritebatch.Draw(AuswahlGegner.Textur[0], new Rectangle((int)(Cursor.Mouseposition.X - ((AuswahlGegner.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((AuswahlGegner.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(AuswahlGegner.Texturmaße.X * AuswahlSkalierung), (int)(AuswahlGegner.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, 0, (int)AuswahlGegner.Texturmaße.X, (int)AuswahlGegner.Texturmaße.Y), Color.White * 0.7f);
                            else if (AuswahlRichtung == Richtung.Rechts) spritebatch.Draw(AuswahlGegner.Textur[0], new Rectangle((int)(Cursor.Mouseposition.X - ((AuswahlGegner.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((AuswahlGegner.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(AuswahlGegner.Texturmaße.X * AuswahlSkalierung), (int)(AuswahlGegner.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, (int)AuswahlGegner.Texturmaße.Y, (int)AuswahlGegner.Texturmaße.X, (int)AuswahlGegner.Texturmaße.Y), Color.White * 0.7f);
                        break;
                    case Editortool.Aktion:
                        if (VectorWaehlen)
                            switch (AuswahlAction.Type)
                            {
                                case Actiontype.Gegneraktion:
                                    if (AuswahlAction.Gegneraktion == Gegneraktionstyp.Erstellen)
                                    {
                                        Enemy TempEnemy = Enemydatabase.Read(AuswahlAction.Gegnertyp);
                                        if (AuswahlAction.Richtung == Richtung.Links) spritebatch.Draw(TempEnemy.Textur[0], new Rectangle((int)(Cursor.Mouseposition.X - ((TempEnemy.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((TempEnemy.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(TempEnemy.Texturmaße.X * AuswahlSkalierung), (int)(TempEnemy.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, 0, (int)TempEnemy.Texturmaße.X, (int)TempEnemy.Texturmaße.Y), Color.White * 0.7f);
                                        else if (AuswahlAction.Richtung == Richtung.Rechts) spritebatch.Draw(TempEnemy.Textur[0], new Rectangle((int)(Cursor.Mouseposition.X - ((TempEnemy.Texturmaße.X * AuswahlSkalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((TempEnemy.Texturmaße.Y * AuswahlSkalierung) / 2)), (int)(TempEnemy.Texturmaße.X * AuswahlSkalierung), (int)(TempEnemy.Texturmaße.Y * AuswahlSkalierung)), new Rectangle(0, (int)TempEnemy.Texturmaße.Y, (int)TempEnemy.Texturmaße.X, (int)TempEnemy.Texturmaße.Y), Color.White * 0.7f);
                                    }
                                    break;
                                case Actiontype.Klettern:
                                    spritebatch.Draw(Contents.Block, new Rectangle((int)Cursor.Mouseposition.X - 35, (int)Cursor.Mouseposition.Y - 2, 70, 4), Color.Red);
                                    break;
                                case Actiontype.Objektaktion:
                                    if (AuswahlAction.Objektaktion == Objektaktionstyp.Erstellen)
                                    {
                                        spritebatch.Draw(AuswahlAction.Objecttyp.Textur, new Rectangle((int)(Cursor.Mouseposition.X - ((AuswahlAction.Objecttyp.Variante[AuswahlAction.Wert].Texturausschnitt.Width * AuswahlAction.Skalierung) / 2)), (int)(Cursor.Mouseposition.Y - ((AuswahlAction.Objecttyp.Variante[AuswahlAction.Wert].Texturausschnitt.Height * AuswahlAction.Skalierung) / 2)), (int)(AuswahlAction.Objecttyp.Variante[AuswahlAction.Wert].Texturausschnitt.Width * AuswahlSkalierung), (int)(AuswahlAction.Objecttyp.Variante[AuswahlAction.Wert].Texturausschnitt.Height * AuswahlSkalierung)), AuswahlAction.Objecttyp.Variante[AuswahlAction.Wert].Texturausschnitt, Color.White * 0.7f);
                                    }
                                    break;
                            }
                        break;
                    case Editortool.Items:
                        if (AuswahlItem != null)
                            spritebatch.Draw(AuswahlItem.Textur, new Vector2(Cursor.Mouseposition.X - (AuswahlItem.Textur.Width / 2), Cursor.Mouseposition.Y - (AuswahlItem.Textur.Height / 2)), Color.White * 0.7f); break;
                    case Editortool.Schaltereinstellungen: if (VectorWaehlen) spritebatch.Draw(Contents.Schalter, new Vector2(Cursor.Mouseposition.X - AuswahlLever.Mittelpunkt.X, Cursor.Mouseposition.Y - AuswahlLever.Mittelpunkt.Y), new Rectangle(0, 0, 280, 201), Color.White); break;
                }
            }
        }

        private void DrawKollision(SpriteBatch spritebatch)
        {
            foreach (Object Object in Objectmanager.ObjectsSpielebene)
            {
                if (Object.Objektvariante.Kollision != null)
                {
                    foreach (Objectcollision Kollision in Object.Objektvariante.Kollision)
                    {
                        spritebatch.Draw(Contents.Block, new Rectangle((int)(Object.PositionCurrent.X + (Kollision.Zone.X * Object.Skalierung) - Player.PositionCurrent.X), (int)(Object.PositionCurrent.Y + (Kollision.Zone.Y * Object.Skalierung)), (int)((float)Kollision.Zone.Width * Object.Skalierung), (int)((float)Kollision.Zone.Height * Object.Skalierung)), Color.Yellow);
                    }
                }
            }
            spritebatch.Draw(Contents.Block, new Rectangle(0, (int)Player.Ground - 2 + 200, 1280, 4), Color.Red);    //Walkline
        }

        public void DrawVorschau(SpriteBatch spritebatch)
        {
            if (RückmeldungAnzeigen != 0) spritebatch.DrawString(Contents.Meiryo8, "Level gespeichert!", new Vector2(Cursor.Mouseposition.X + 25, Cursor.Mouseposition.Y), Color.LawnGreen);
            switch (Tool)
            {
                case Editortool.Objektvariante:
                    if (VorschauObjektvariation != null)
                    {
                        spritebatch.Draw(Contents.Block, new Rectangle((int)Cursor.Mouseposition.X + 100, (int)Cursor.Mouseposition.Y, VorschauObjektvariation.Texturausschnitt.Width, VorschauObjektvariation.Texturausschnitt.Height), Color.White);
                        spritebatch.Draw(VorschauObjektvariation.Textur, new Rectangle((int)Cursor.Mouseposition.X + 100, (int)Cursor.Mouseposition.Y, VorschauObjektvariation.Texturausschnitt.Width, VorschauObjektvariation.Texturausschnitt.Height), VorschauObjektvariation.Texturausschnitt, Color.White);
                        if (Rasterplatzierung) spritebatch.DrawString(Contents.Meiryo8, VorschauObjektvariation.Texturausschnitt.Width.ToString() + "*" + VorschauObjektvariation.Texturausschnitt.Height.ToString(), new Vector2(Cursor.Mouseposition.X + 105, Cursor.Mouseposition.Y + 5), Color.Red);
                    }
                    break;
                case Editortool.Gegner:
                    if (VorschauGegner != null)
                    {
                        spritebatch.Draw(Contents.Block, new Rectangle((int)Cursor.Mouseposition.X + 100, (int)Cursor.Mouseposition.Y, (int)VorschauGegner.Texturmaße.X, (int)VorschauGegner.Texturmaße.Y), Color.White);
                        spritebatch.Draw(VorschauGegner.Textur[0], new Rectangle((int)Cursor.Mouseposition.X + 100, (int)Cursor.Mouseposition.Y, (int)VorschauGegner.Texturmaße.X, (int)VorschauGegner.Texturmaße.Y), new Rectangle(0, 0, (int)VorschauGegner.Texturmaße.X, (int)VorschauGegner.Texturmaße.Y), Color.White);
                    }
                    break;
                case Editortool.Ausloeser:
                case Editortool.Bedingung:
                case Editortool.Hintergrund:
                case Editortool.Aktion:
                    if (VectorWaehlen)
                    {
                        spritebatch.Draw(Contents.Bar, new Rectangle((int)Cursor.Mouseposition.X + 40, (int)Cursor.Mouseposition.Y - 1, 147, 21), Color.White);
                        spritebatch.DrawString(Contents.Meiryo8, "Position Linksklicken!", new Vector2(Cursor.Mouseposition.X + 50, Cursor.Mouseposition.Y + 2), Color.White);
                    }
                    break;
                case Editortool.Items:
                    if (VorschauItem != null)
                    {
                        spritebatch.Draw(Contents.Block, new Rectangle((int)Cursor.Mouseposition.X + 100, (int)Cursor.Mouseposition.Y, VorschauItem.Feld.Width, VorschauItem.Feld.Height), Color.White);
                        spritebatch.Draw(VorschauItem.Textur, new Vector2(Cursor.Mouseposition.X + 100, Cursor.Mouseposition.Y), Color.White * 0.7f);
                    }
                    break;
            }
        }

        private void TextZentriert(SpriteBatch spritebatch, SpriteFont spritefont, string text, Vector2 position, Color color)
        {
            spritebatch.DrawString(spritefont, text, new Vector2((int)position.X - (int)(spritefont.MeasureString(text).X / 2), (int)position.Y - (int)(spritefont.MeasureString(text).Y / 2)), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
    }
}
