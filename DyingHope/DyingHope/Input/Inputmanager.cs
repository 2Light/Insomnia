using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DyingHope
{
    class Inputmanager
    {
        public bool Debug;
        public bool Collision;
        private Player Player;
        private Contents Contents;
        private Camera Camera;
        private Keymanager Keymanager;
        private Cursor Cursor;
        private Enemymanager Enemymanager;
        private Windowmanager Windowmanager;
        private Backgroundmanager Backgroundmanager;
        private Backgrounddatabase Backgrounddatabase;
        private Objectmanager Objectmanager;
        private Objectdatabase Objectdatabase;
        private Eventmanager Eventmanager;
        private Levelmanager Levelmanager;
        private Levermanager Levermanager;
        private ContentManager Contentmanager;
        public Menuestate Menuestate;
        private Editor Editor;
        private Mathe Mathe;
        private Itemmanager Itemmanager;
        private DebugView DebugView;    
        private DepressionHandler Depression;
        private BackgroundMusic bg;

        public Inputmanager(Contents contents, Camera camera, Player player, Keymanager keymanager, Cursor mouse, Enemymanager enemymanager, Windowmanager windowmanager, Backgroundmanager backgroundmanager, Backgrounddatabase backgrounddatabase, Objectmanager objectmanager, Objectdatabase objectdatabase, Eventmanager eventmanager, Levelmanager levelmanager, Levermanager levermanager, ContentManager contentmanager, Editor editor, DebugView DebugView, DepressionHandler Depression, Mathe mathe, Itemmanager itemmanager, BackgroundMusic bmus)
        {
            this.Contents = contents;
            this.Player = player;
            this.Keymanager = keymanager;
            this.Cursor = mouse;
            this.Enemymanager = enemymanager;
            this.Windowmanager = windowmanager;
            this.Backgroundmanager = backgroundmanager;
            this.Backgrounddatabase = backgrounddatabase;
            this.Objectmanager = objectmanager;
            this.Objectdatabase = objectdatabase;
            this.Eventmanager = eventmanager;
            this.Levelmanager = levelmanager;
            this.Levermanager = levermanager;
            this.Contentmanager = contentmanager;
            this.Editor = editor;
            this.Mathe = mathe;
            this.Keymanager.Windowmanager = windowmanager;
            this.Camera = camera;
            this.Itemmanager = itemmanager;
            this.DebugView = DebugView;
            this.Depression = Depression;
            this.bg = bmus;
        }

        public Menuestate Update(Gamestate gamestate, Menuestate menuestate)
        {
            //this.Gamestate = gamestate;
            this.Menuestate = menuestate;
            Editor.VorschauRuecksetzen();
            Keymanager.Update();
            Cursor.Update(Player.PositionCurrent, Windowmanager.MouseOverGUI, gamestate);
            Windowmanager.Update();
            Aktionen(gamestate);
            foreach (Windowelement Eingabezelle in Windowmanager.Eingabezellen) EingabezelleWertLesen(Eingabezelle);
            return Menuestate;
        }

        private void Aktionen(Gamestate gamestate)
        {
            //Keyboardaktionen--------------------------------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < Keymanager.Befehle.Count(); i++)
            {
                switch (Keymanager.Befehle[i])
                {
                    //Spiel
                    case 1: Player.Accelerate(Richtung.Hoch); break;
                    case 2: Player.Accelerate(Richtung.Runter); break;
                    case 3: Player.Accelerate(Richtung.Links); break;
                    case 4: Player.Accelerate(Richtung.Rechts); break;
                    case 5: Levermanager.Use(); break;

                    case 6: Player.DepressionEnabled = !Player.DepressionEnabled; break;
                    
                    //Kamera
                    //case 5: Camera.Update(Richtung.Rechts); break;
                    //case 6: Camera.Update(Richtung.Links); break;
                    //case 7: Camera.Update(Richtung.Hoch); break;
                    //case 8: Camera.Update(Richtung.Runter); break;
                    
                    //Debug & Menue

                    // Debug Toogle
                    case 20: DebugView.setFlag(DebugFlag.PlayerStats, false); break;
                    case 21: DebugView.setFlag(DebugFlag.CollisonShape, true); break;
                    case 22: DebugView.setFlag(DebugFlag.ObjectInformation, true); break;
                    case 23: DebugView.setFlag(DebugFlag.ObjectmanagerStats, false); break;
                    case 24: DebugView.setFlag(DebugFlag.EditorStats, false); break;
                    case 25: DebugView.setFlag(DebugFlag.Inputstats, false); break;
                    case 26: DebugView.setFlag(DebugFlag.KammeraStats, false); break;
                    case 27: DebugView.setFlag(DebugFlag.Pertikle, true); break;
                    case 28: DebugView.setFlag(DebugFlag.BackgrundStats, false); break;
                    case 29: DebugView.setFlag(DebugFlag.WindowStats, false); break;
                    
                    //Depression Flags
                    case 40: Depression.setFlag(DepressionState.InvertMove); break;
                    case 41: Depression.setFlag(DepressionState.Slow); break;
                    case 42: Depression.setFlag(DepressionState.InvertScreen); break;
                    case 43: Depression.setFlag(DepressionState.ModifyWorld); break;
                    case 44: Depression.setFlag(DepressionState.ReduceFOV); break;
                    case 45: Depression.setFlag(DepressionState.Srink); break;
                    case 46: Depression.setFlag(DepressionState.GrayScal); break;

                    //case 11: if (Menuestate == Menuestate.Pause) Menuestate = Menuestate.Continue; else if (gamestate != Gamestate.Menue) { Menuestate = Menuestate.Pause; Windowmanager.AddWindow(Windowtype.MainMenue); Keymanager.TastenMenü(); bg.Stop(); } break; //Gamestate = Gamestate.Exit; break;
                    case 11:
                        switch (gamestate)
                        {
                            case Gamestate.Editor: if (Editor.VectorWaehlen) Editor.VectorWaehlen = false; else { Menuestate = Menuestate.Pause; Windowmanager.AddWindow(Windowtype.MainMenue); Keymanager.TastenMenü(); bg.Stop(); } break;
                            case Gamestate.Game: Menuestate = Menuestate.Pause; Windowmanager.AddWindow(Windowtype.MainMenue); Keymanager.TastenMenü(); bg.Stop(); break;
                            case Gamestate.Menue: if (Menuestate == Menuestate.Pause) Menuestate = Menuestate.Continue; break;
                        }
                        break;
                    case 997: if (Menuestate != Menuestate.Started) Menuestate = Menuestate.StartEditor; break; //Gamestate = Gamestate.Editor; Editor.Starten(); break;
                    case 998: Debug = !Debug; break;
                    case 999: Collision = !Collision; break;

                    //Editor
                    case 2000: Player.PositionCurrent.X -= Editor.AuswahlScrolling; break;
                    case 2001: Player.PositionCurrent.X += Editor.AuswahlScrolling; break;
                    case 2002: Menuestate = Menuestate.LeaveEditor; break; //Gamestate = Gamestate.Game; Editor.Beenden(); break;

                    //Eingabezellen
                    case 3001: Editor.AuswahlLayer = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3002: Editor.AuswahlScrolling = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3003: Editor.AuswahlSkalierung = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3004: Editor.Raster = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3005: Editor.AuswahlHintergrund.versatzY = Convert.ToInt32(Keymanager.Texteingabe); Backgroundmanager.Update(); break;
                    case 3006: Editor.AuswahlHintergrund.Scrollgeschwindigkeit.X = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3007: Editor.AuswahlHintergrund.Scrollgeschwindigkeit.Y = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3008: Editor.AuswahlHintergrund.Transparenz = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3009: Editor.ChangeTool(Editortool.Start); Editor.NewLevel(Keymanager.Texteingabe); break;
                    case 3010: Editor.AuswahlEvent.Name = Keymanager.Texteingabe; break;
                    case 3016: Editor.AuswahlTrigger.Position.X = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3017: Editor.AuswahlTrigger.Wert = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3019: Editor.AuswahlCondition.Position.X = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3020: Editor.AuswahlCondition.Depression = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3022: Editor.AuswahlEvent.CooldownStart = Convert.ToInt32(Keymanager.Texteingabe) * 60; break;
                    case 3024: Editor.AuswahlAction.Wert = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3033: Levelmanager.AktuellesLevel.Walkline = Convert.ToInt32(Keymanager.Texteingabe);
                        Player.Ground = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3036: Levelmanager.AktuellesLevel.DepressionRate = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100;
                        Player.DepressionRate = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3037: Editor.RasterOffset = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3038: Levelmanager.AktuellesLevel.VordergrundAbdunkelung = Convert.ToInt32((Convert.ToDouble(Keymanager.Texteingabe) / 100) * 255); break;
                    case 3039: Editor.AuswahlLever.Name = Keymanager.Texteingabe; break;
                    case 3046: Editor.AuswahlAction.Layer = Convert.ToInt32(Keymanager.Texteingabe); break;
                    case 3048: Editor.AuswahlAction.Skalierung = (float)Convert.ToDouble(Keymanager.Texteingabe) / 100; break;
                    case 3054: Editor.Autosaveintervall = Convert.ToInt32(Keymanager.Texteingabe) * 60; break;
                    //case 3055: Editor.AuswahlHintergrund.Startposition = Convert.ToInt32(Keymanager.Texteingabe); break;
                    //case 3056: Editor.AuswahlHintergrund.Endposition = Convert.ToInt32(Keymanager.Texteingabe); break;
                }
            }

            //UI-Aktionen-------------------------------------------------------------------------------------------------------------------------------------------
            switch (Windowmanager.Befehl)
            {
                //Hauptmenü
                case 1: Menuestate = Menuestate.New; break;
                //case 2: Menuestate = Menuestate.Load; break;
                case 3: Menuestate = Menuestate.Exit; Levelmanager.AutoSaveEditor(); break;
                //case 4: Menuestate = Menuestate.Settings; break;

                //Editor: Toolfenster
                case 10: Windowmanager.AddWindow(Windowtype.Editor); break;
                case 11: Windowmanager.RemoveWindow(Windowtype.Editor); break;
                case 12: Windowmanager.AddWindow(Windowtype.Infofenster); break;
                case 13: Editor.ChangeTool(Editortool.Editoreinstellungen); break;
                case 14: Editor.ChangeTool(Editortool.Laden); break;
                case 15: Levelmanager.SaveLevelEditor(); Editor.RückmeldungAnzeigen = 100; break;
                case 16: Editor.ChangeTool(Editortool.Leveleinstellungen); break;
                case 17: Editor.ChangeTool(Editortool.Hintergruende); break;
                case 18: Editor.ChangeTool(Editortool.Objektkategorie); break;
                case 19: Editor.ChangeTool(Editortool.Events); break;
                case 20: Editor.ChangeTool(Editortool.Gegner); break;
                case 21: Editor.ChangeTool(Editortool.Items); break;
                case 22: Editor.ChangeTool(Editortool.Schalter); break;
                case 29: Menuestate = Menuestate.LeaveEditor; break; //Gamestate = Gamestate.Game; Editor.Beenden(); break;
                case 30: Menuestate = Menuestate.Exit; Levelmanager.AutoSaveEditor(); break; //Gamestate = Gamestate.Exit; break;
                case 31: Editor.ChangeTool(Editortool.Neu); break;

                //Editor: Infofenster
                case 100: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Infofenster); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Infofenster).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Infofenster).Feld.Y); break;    //Fenster verschieben
                case 101: Windowmanager.RemoveWindow(Windowtype.Infofenster); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                case 102: Editor.Rasterplatzierung = false; break;
                case 103: Editor.Rasterplatzierung = true; break;
                case 104: Editor.AuswahlRichtung = Richtung.Links; break;
                case 105: Editor.AuswahlRichtung = Richtung.Rechts; break;

                //Editor: Editoreinstellungsfenster
                case 200: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Editoreinstellungen); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Editoreinstellungen).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Editoreinstellungen).Feld.Y); break;    //Fenster verschieben
                case 201: Windowmanager.RemoveWindow(Windowtype.Editoreinstellungen); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen

                //Editor: Editoreinstellungsfenster
                case 298: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Neu); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Neu).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Neu).Feld.Y); break;    //Fenster verschieben
                case 299: Windowmanager.RemoveWindow(Windowtype.Neu); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen

                //Editor: Map-Laden Fenster
                case 300: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Laden); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Laden).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Laden).Feld.Y); break;    //Fenster verschieben
                case 301: Windowmanager.RemoveWindow(Windowtype.Laden); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                //310-349 als Bereich vergeben

                //Editor: Hintergrundtextur Fenster
                case 400: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Hintergrundtextur); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Hintergrundtextur).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Hintergrundtextur).Feld.Y); break;    //Fenster verschieben
                case 401: Windowmanager.RemoveWindow(Windowtype.Hintergrundtextur); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                //410-449 als Bereich vergeben

                //Editor: Leveleinstellungsfenster
                case 500: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen).Feld.Y); break;    //Fenster verschieben
                case 501: Windowmanager.RemoveWindow(Windowtype.Leveleinstellungen); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                case 502:
                    if (Levelmanager.AktuellesLevel.Name != "Stadt")
                    {
                        System.IO.Directory.Delete(@"Levels\" + Levelmanager.AktuellesLevel.Name, true);
                        Levelmanager.Levels.Remove(Levelmanager.AktuellesLevel);
                        Levelmanager.LoadLevel("Stadt");
                    }
                    break;

                //Editor: Hintergründeauswahlfenster
                case 600: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Hintergruende); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Hintergruende).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Hintergruende).Feld.Y); break;    //Fenster verschieben
                case 601: Windowmanager.RemoveWindow(Windowtype.Hintergruende); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                    //610-649 als Bereich vergeben
                case 650: Backgroundmanager.Backgrounds.Add(new Background("Neu", new Vector2(0, 0), 1.0f, 0, 0, 0, Contentmanager));
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Hintergruende);    //Hintergrundsliste neu Laden für Buttonerstellung
                    break;

                //Editor: Hintergrundoptionsfenster
                case 700: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Hintergrund); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Hintergrund).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Hintergrund).Feld.Y); break;    //Fenster verschieben
                case 701: Windowmanager.RemoveWindow(Windowtype.Hintergrund); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                case 702: Editor.ChangeTool(Editortool.Hintergruende); break; //Zurück
                    //710-749 als Bereich vergeben
                case 750: Editor.ChangeTool(Editortool.Hintergrundtextur); break;
                case 751:
                    int TempInt = Backgroundmanager.Backgrounds.IndexOf(Editor.AuswahlHintergrund);
                    if (TempInt < Backgroundmanager.Backgrounds.Count() - 1)
                    {
                        Background TempBackground = Backgroundmanager.Backgrounds[Backgroundmanager.Backgrounds.IndexOf(Editor.AuswahlHintergrund) + 1];
                        Backgroundmanager.Backgrounds[TempInt + 1] = Backgroundmanager.Backgrounds[TempInt];
                        Backgroundmanager.Backgrounds[TempInt] = TempBackground;
                    }
                    break;
                case 752:
                    TempInt = Backgroundmanager.Backgrounds.IndexOf(Editor.AuswahlHintergrund);
                    if (Backgroundmanager.Backgrounds.IndexOf(Editor.AuswahlHintergrund) > 0)
                    {
                        Background TempBackground = Backgroundmanager.Backgrounds[TempInt - 1];
                        Backgroundmanager.Backgrounds[TempInt - 1] = Backgroundmanager.Backgrounds[TempInt];
                        Backgroundmanager.Backgrounds[TempInt] = TempBackground;
                    }
                    break;
                case 753:
                    Backgroundmanager.Backgrounds.Remove(Editor.AuswahlHintergrund);
                    Editor.ChangeTool(Editortool.Hintergruende);
                    break;

                //Editor: Objektkategoriefenster
                case 800: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Objektkategorie); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).Feld.Y); break;    //Fenster verschieben
                case 801: Windowmanager.RemoveWindow(Windowtype.Objektkategorie); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                    //810-849 als Bereich vergeben

                //Editor: Objektvariationsfenster
                case 900: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Objektvariante); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Objektvariante).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Objektvariante).Feld.Y); break;    //Fenster verschieben
                case 901: Windowmanager.RemoveWindow(Windowtype.Objektvariante); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                case 902: if (Editor.Tool == Editortool.Objektvariante) Editor.ChangeTool(Editortool.Objektkategorie);
                    else if (Editor.Tool == Editortool.Aktion) { Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion); } break; //Zurück
                    //910-949 als Bereich vergeben -> Auswahl
                    //950-990 als Bereich vergeben -> Vorschau

                //Editor: Eventsfenster
                case 1000: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Events); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Events).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Events).Feld.Y); break;    //Fenster verschieben
                case 1001: Windowmanager.RemoveWindow(Windowtype.Events); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                    //1110-1149 als Bereich vergeben
                case 1050: Eventmanager.NewEvent(); //Neues Event
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Events); break;    //Eventliste neu Laden für Buttonerstellung

                //Editor: Eventfenster
                case 1060: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Event); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Event).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Event).Feld.Y); break;    //Fenster verschieben
                case 1061: Windowmanager.RemoveWindow(Windowtype.Event);  //Fenster schließen 
                    Windowmanager.RemoveWindow(Windowtype.Ausloeser);
                    Windowmanager.RemoveWindow(Windowtype.Bedingung);
                    Windowmanager.RemoveWindow(Windowtype.Aktion);
                    Editor.ChangeTool(Editortool.Start); 
                    break;
                case 1062: Editor.ChangeTool(Editortool.Events); break; //Zurück
                case 1063: Editor.AuswahlEvent.Triggers.Add(new Eventtrigger(Enemymanager, Mathe, Player)); //Neuer Auslöser
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Event); break;    //Eventfenster neu Laden für Buttonerstellung
                case 1064: Editor.AuswahlEvent.Conditions.Add(new Eventcondition(Mathe, Player));  //Neue Bedingung
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Event); break;    //Eventfenster neu Laden für Buttonerstellung
                case 1065: Editor.AuswahlEvent.Actions.Add(new Eventaction(Mathe, Player, Levelmanager, Enemymanager, Depression, Eventmanager, Objectmanager, Objectdatabase,bg));  //Neue Aktion
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Event); break;   //Eventfenster neu Laden für Buttonerstellung
                case 1066: Eventmanager.Events.Remove(Editor.AuswahlEvent); Editor.ChangeTool(Editortool.Events); break; //Event löschen
                    //1100-1199 als Bereich vergeben

                //Editor: Eventausloeser
                case 1200: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Ausloeser); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Ausloeser).Feld.Y); break;    //Fenster verschieben
                case 1201: Editor.ChangeTool(Editortool.Event); break;  //Zurück
                case 1209: Editor.AuswahlEvent.Triggers.Remove(Editor.AuswahlTrigger); Editor.ChangeTool(Editortool.Event); break;  //Auslöser löschen
                    //1210-1249 als Bereich vergeben

                //Editor: Eventbedingung
                case 1300: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Bedingung); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Bedingung).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Bedingung).Feld.Y); break;    //Fenster verschieben
                case 1301: Editor.ChangeTool(Editortool.Event); break;  //Zurück
                case 1309: Editor.AuswahlEvent.Conditions.Remove(Editor.AuswahlCondition); Editor.ChangeTool(Editortool.Event); break;  //Bedingung löschen
                    //1310-1349 als Bereich vergeben

                //Editor: Eventaktion
                case 1400: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Aktion); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Aktion).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Aktion).Feld.Y); break;    //Fenster verschieben
                case 1401: Editor.ChangeTool(Editortool.Event); break;  //Zurück
                case 1409: Editor.AuswahlEvent.Actions.Remove(Editor.AuswahlAction); Editor.ChangeTool(Editortool.Event); break;  //Aktion löschen
                    //1410-1449 als Bereich vergeben

                //Editor: Eventelementtyp
                case 1500: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Typ); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Typ).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Typ).Feld.Y); break;    //Fenster verschieben
                //case 1501: Windowmanager.RemoveWindow(Windowtype.Typ); break;  //Fenster schließen
                case 1501: if (Editor.AuswahlTrigger != null) Editor.ChangeTool(Editortool.Ausloeser); 
                    else if (Editor.AuswahlCondition != null) Editor.ChangeTool(Editortool.Bedingung);
                    else if (Editor.AuswahlAction != null) Editor.ChangeTool(Editortool.Aktion); 
                    break; //Zurück
                    //1510-1549 als Bereich vergeben

                //Editor: Gegnerfenster
                case 1600: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Gegner); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Gegner).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Gegner).Feld.Y); break;    //Fenster verschieben
                case 1601: Windowmanager.RemoveWindow(Windowtype.Gegner); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                    //1610-1649 als Bereich vergeben -> Auswahl
                    //1650-1690 als Bereich vergeben -> Vorschau

                //Editor: Itemfenster
                case 1700: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Items); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Items).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Items).Feld.Y); break;    //Fenster verschieben
                case 1701: Windowmanager.RemoveWindow(Windowtype.Items); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                    //1710-1749 als Bereich vergeben -> Auswahl
                    //1750-1790 als Bereich vergeben -> Vorschau

                //Editor: Schalterfenster
                case 1800: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Lever); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Lever).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Lever).Feld.Y); break;    //Fenster verschieben
                case 1801: Windowmanager.RemoveWindow(Windowtype.Lever); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen
                case 1802: Editor.ChangeTool(Editortool.Schalter); break; //Zurück
                case 1803: Editor.VectorWaehlen = true; break;
                case 1809: Levermanager.Levers.Remove(Editor.AuswahlLever); Editor.ChangeTool(Editortool.Schalter); break;  //Schalter löschen
                //1810-1849 als Bereich vergeben -> Auswahl
                case 1850: Levermanager.AddLever(); 
                    Editor.ChangeTool(Editortool.Start); Editor.ChangeTool(Editortool.Schalter); break;    //Schalterliste neu Laden für Buttonerstellung

                //Editor: Schaltereinstellungsfenster
                case 1900: Cursor.Fenster = Windowmanager.GetWindowOfType(Windowtype.Levers); Cursor.Bezugspunkt = new Vector2(Cursor.Mouseposition.X - Windowmanager.GetWindowOfType(Windowtype.Levers).Feld.X, Cursor.Mouseposition.Y - Windowmanager.GetWindowOfType(Windowtype.Levers).Feld.Y); break;    //Fenster verschieben
                case 1901: Windowmanager.RemoveWindow(Windowtype.Levers); Editor.ChangeTool(Editortool.Start); break;  //Fenster schließen

                //Eingabezellen
                case 3000: Editor.AnzeigenKollision = !Editor.AnzeigenKollision; break;
                case 3001: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Infofenster), 5); break;
                case 3002: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Editoreinstellungen), 4); break;
                case 3003: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Infofenster), 11); break;
                case 3004: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Infofenster), 15); break;
                case 3005: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 7); break;
                case 3006: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 9); break;
                case 3007: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 11); break;
                case 3008: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 13); break;
                case 3009: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Neu), 3); break;
                case 3010: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Event), 6); break;
                case 3011: Editor.ChangeTool(Editortool.Typ); break;
                case 3012: Editor.Rasterplatzierung = !Editor.Rasterplatzierung; break;
                case 3013: if (Editor.AuswahlRichtung == Richtung.Links) Editor.AuswahlRichtung = Richtung.Rechts; else Editor.AuswahlRichtung = Richtung.Links; break;
                case 3014: switch (Editor.AuswahlObjektebene)
                    {
                        case Objektebene.Hintergrund: Editor.AuswahlObjektebene = Objektebene.Spielfeld; break;
                        case Objektebene.Spielfeld: Editor.AuswahlObjektebene = Objektebene.Vordergrund; break;
                        case Objektebene.Vordergrund: Editor.AuswahlObjektebene = Objektebene.Hintergrund; break;
                    }
                    break;
                case 3015: Editor.AuswahlEvent.Repeat = !Editor.AuswahlEvent.Repeat; break;
                case 3016: Editor.VectorWaehlen = true; break;
                case 3017: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Ausloeser), 5); break;
                case 3018: Editor.AuswahlTrigger.Ausrichtung = !Editor.AuswahlTrigger.Ausrichtung; break;
                case 3019: switch (Editor.AuswahlAction.Gegneraktion)
                    {
                        case Gegneraktionstyp.Erstellen: Editor.AuswahlAction.Gegneraktion = Gegneraktionstyp.Entfernen; break;
                        case Gegneraktionstyp.Entfernen: Editor.AuswahlAction.Gegneraktion = Gegneraktionstyp.Status; break;
                        case Gegneraktionstyp.Status: Editor.AuswahlAction.Gegneraktion = Gegneraktionstyp.Erstellen; break;
                    }
                    Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion);
                    break;
                case 3020: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Bedingung), 5); break;
                case 3021: Editor.AuswahlCondition.Ausrichtung = !Editor.AuswahlCondition.Ausrichtung; break;
                case 3022: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Event), 13); break;
                case 3023: Editor.AuswahlAction.Ausrichtung = !Editor.AuswahlAction.Ausrichtung; break;
                case 3024: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Aktion), 7); break;
                case 3025: switch (Editor.AuswahlAction.DepressionState)
                    {
                        case DepressionState.GrayScal: Editor.AuswahlAction.DepressionState = DepressionState.InvertMove; break;
                        case DepressionState.InvertMove: Editor.AuswahlAction.DepressionState = DepressionState.InvertScreen; break;
                        case DepressionState.InvertScreen: Editor.AuswahlAction.DepressionState = DepressionState.ModifyWorld; break;
                        case DepressionState.ModifyWorld: Editor.AuswahlAction.DepressionState = DepressionState.ReduceFOV; break;
                        case DepressionState.ReduceFOV: Editor.AuswahlAction.DepressionState = DepressionState.Slow; break;
                        case DepressionState.Slow: Editor.AuswahlAction.DepressionState = DepressionState.Srink; break;
                        case DepressionState.Srink: Editor.AuswahlAction.DepressionState = DepressionState.GrayScal; break;
                    }
                    break;
                case 3026: switch (Editor.AuswahlAction.Effektwechsel)
                    {
                        case Effektwechsel.Einschalten: Editor.AuswahlAction.Effektwechsel = Effektwechsel.Ausschalten; break;
                        case Effektwechsel.Ausschalten: Editor.AuswahlAction.Effektwechsel = Effektwechsel.Umschalten; break;
                        case Effektwechsel.Umschalten: Editor.AuswahlAction.Effektwechsel = Effektwechsel.Einschalten; break;
                    }
                    break;
                case 3027: switch (Editor.AuswahlAction.Gegnertyp)
                    {
                        case Enemytype.Greif: Editor.AuswahlAction.Gegnertyp = Enemytype.Messerkreatur; break;
                        case Enemytype.Messerkreatur: Editor.AuswahlAction.Gegnertyp = Enemytype.Schatten; break;
                        case Enemytype.Schatten: Editor.AuswahlAction.Gegnertyp = Enemytype.Wolf; break;
                        case Enemytype.Wolf: Editor.AuswahlAction.Gegnertyp = Enemytype.Greif; break;
                    }
                    break;
                case 3028: if (Editor.AuswahlAction.Richtung == Richtung.Links) Editor.AuswahlAction.Richtung = Richtung.Rechts; else Editor.AuswahlAction.Richtung = Richtung.Links; break;
                case 3029: switch (Editor.AuswahlAction.Gegnerstatus)
                    {
                        case Enemystate.Idle: Editor.AuswahlAction.Gegnerstatus = Enemystate.Attack; break;
                        //case Enemystate.Chase: Editor.AuswahlAction.Gegnertyp = Enemytype.Schatten; break;
                        case Enemystate.Attack: Editor.AuswahlAction.Gegnerstatus = Enemystate.Idle; break;
                        //case Enemystate.Dead: Editor.AuswahlAction.Gegnertyp = Enemytype.Greif; break;
                    }
                    break;
                case 3030:
                    foreach (Level level in Levelmanager.Levels)
                    {
                        if (Editor.AuswahlAction.Name == level.Name)
                        {
                            if (Levelmanager.Levels.IndexOf(level) + 1 == Levelmanager.Levels.Count()) Editor.AuswahlAction.Name = Levelmanager.Levels[0].Name;
                            else Editor.AuswahlAction.Name = Levelmanager.Levels[Levelmanager.Levels.IndexOf(level) + 1].Name;
                            break;
                        }
                    }
                    break;
                case 3031: Editor.AnzeigenEvents = !Editor.AnzeigenEvents; break;
                case 3032: Editor.AuswahlHeilung = !Editor.AuswahlHeilung; break;
                case 3033: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen), 7); break;
                case 3034: Editor.VectorWaehlen = true; Editor.NurX = true; break;
                case 3035: Editor.VectorWaehlen = true; Editor.NurY = true; break;
                case 3036: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen), 9); break;
                case 3037: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Infofenster), 13); break;
                case 3038: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Leveleinstellungen), 11); break;
                case 3039: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Lever), 3); break;
                case 3040: Editor.AuswahlLever.Einmalig = !Editor.AuswahlLever.Einmalig; break;
                case 3041: break;
                case 3042: Editor.AuswahlLever.Betätigt = !Editor.AuswahlLever.Betätigt; break;
                case 3043: Editor.AuswahlLever.Rücksetzen = !Editor.AuswahlLever.Rücksetzen; break;
                case 3044: switch (Editor.AuswahlAction.Objektaktion)
                    {
                        case Objektaktionstyp.Erstellen: Editor.AuswahlAction.Objektaktion = Objektaktionstyp.Entfernen; break;
                        case Objektaktionstyp.Entfernen: Editor.AuswahlAction.Objektaktion = Objektaktionstyp.Austauschen; break;
                        case Objektaktionstyp.Austauschen: Editor.AuswahlAction.Objektaktion = Objektaktionstyp.Erstellen; break;
                    }
                    Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion);
                    break;
                case 3045: Windowmanager.RemoveWindow(Windowtype.Aktion);
                    Editor.AuswahlObjektkategorie = Editor.AuswahlAction.Objecttyp;
                    Windowmanager.AddWindow(Windowtype.Objektvariante); 
                    Windowmanager.GetWindowOfType(Windowtype.Objektvariante).Feld = Windowmanager.GetWindowPosition(Windowtype.Aktion);
                    int LäuferY = 40;
                    int LäuferLinksklick = 910;
                    foreach (Objectvariation Variation in Editor.AuswahlAction.Objecttyp.Variante)
                    {
                        Windowmanager.GetWindowOfType(Windowtype.Objektvariante).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, Variation.Name, false, Color.Blue, LäuferLinksklick, 0, LäuferLinksklick + 40));
                        LäuferY += 15; LäuferLinksklick++;
                    }
                    break;
                case 3046: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Aktion), 9); break;
                case 3047: Windowmanager.RemoveWindow(Windowtype.Aktion);
                    Editor.AuswahlObjektkategorie = Editor.AuswahlAction.Objecttyp;
                    Windowmanager.AddWindow(Windowtype.Objektkategorie);
                    Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).Feld = Windowmanager.GetWindowPosition(Windowtype.Aktion);
                    LäuferY = 40;
                    LäuferLinksklick = 810;
                    foreach (ObjectType Objekttyp in Objectdatabase.Objektdaten)
                    {
                        Windowmanager.GetWindowOfType(Windowtype.Objektkategorie).AddButton(new Button(new Rectangle(10, LäuferY, 200, 15), Contents.Meiryo8, Objekttyp.ObjectClass.ToString(), false, Color.Blue, LäuferLinksklick, 0, 0));
                        LäuferY += 15; LäuferLinksklick++;
                    }
                    break;
                case 3048: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Aktion), 15); break;
                case 3049:
                    switch (Editor.AuswahlAction.Objektebene)
                    {
                        case Objektebene.Hintergrund: Editor.AuswahlAction.Objektebene = Objektebene.Spielfeld; break;
                        case Objektebene.Spielfeld: Editor.AuswahlAction.Objektebene = Objektebene.Vordergrund; break;
                        case Objektebene.Vordergrund: Editor.AuswahlAction.Objektebene = Objektebene.Hintergrund; break;
                    }
                    break;
                case 3050: switch (Editor.AuswahlTrigger.Enemytype)
                    {
                        case Enemytype.Greif: Editor.AuswahlTrigger.Enemytype = Enemytype.Messerkreatur; break;
                        case Enemytype.Messerkreatur: Editor.AuswahlTrigger.Enemytype = Enemytype.Schatten; break;
                        case Enemytype.Schatten: Editor.AuswahlTrigger.Enemytype = Enemytype.Wolf; break;
                        case Enemytype.Wolf: Editor.AuswahlTrigger.Enemytype = Enemytype.Greif; break;
                    }
                    break;
                case 3051: switch (Editor.AuswahlTrigger.Enemystate)
                    {
                        case Enemystate.Attack: Editor.AuswahlTrigger.Enemystate = Enemystate.Dead; break;
                        case Enemystate.Dead: Editor.AuswahlTrigger.Enemystate = Enemystate.Idle; break;
                        case Enemystate.Idle: Editor.AuswahlTrigger.Enemystate = Enemystate.Attack; break;
                    }
                    break;
                case 3052: Editor.AuswahlStatisch = !Editor.AuswahlStatisch; break;
                case 3053: Editor.AuswahlAction.Statisch = !Editor.AuswahlAction.Statisch; break;
                case 3054: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Editoreinstellungen), 6); break;
                case 3055: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 15); break;
                case 3056: Keymanager.NeueEingabe(Windowmanager.GetWindowOfType(Windowtype.Hintergrund), 17); break;

                //Zahlenbereiche abfragen-------------------------------------------------------------------------------------------------------------------------------
                default: Zahlenbereiche(gamestate); break;
            }
        }

        private void Zahlenbereiche(Gamestate gamestate)
        {
            if (Mathe.ZahlImBereich(310, 349, Windowmanager.Befehl))    //Level laden Fenster
            {
                Levelmanager.LoadLevel(Levelmanager.Levels[Windowmanager.Befehl - 310].Name);
                Editor.ChangeTool(Editortool.Start);
                Windowmanager.RemoveWindow(Windowtype.Laden);
            }
            if (Mathe.ZahlImBereich(410, 449, Windowmanager.Befehl))    //Hintergrundtextur
            {
                Editor.AuswahlHintergrund.Textur = Backgrounddatabase.Backgrounds[Windowmanager.Befehl - 410].Textur;
                Editor.AuswahlHintergrund.Name = Backgrounddatabase.Backgrounds[Windowmanager.Befehl - 410].Name;
                Editor.ChangeTool(Editortool.Hintergrund);
            }
            else if (Mathe.ZahlImBereich(610, 649, Windowmanager.Befehl))    //Hintergründeauswahlfenster
            {
                Editor.AuswahlHintergrund = Backgroundmanager.Backgrounds[Windowmanager.Befehl - 610];
                Editor.ChangeTool(Editortool.Hintergrund);
            }
            else if (Mathe.ZahlImBereich(810, 849, Windowmanager.Befehl))    //Objektkategorie
            {
                if (Editor.Tool == Editortool.Objektkategorie)
                {
                    Editor.AuswahlObjektkategorie = Objectdatabase.Objektdaten[Windowmanager.Befehl - 810];
                    Editor.ChangeTool(Editortool.Objektvariante);
                }
                else if (Editor.Tool == Editortool.Aktion)
                {
                    Editor.AuswahlAction.Objecttyp = Objectdatabase.Objektdaten[Windowmanager.Befehl - 810];
                    Editor.AuswahlAction.Wert = 0;
                    Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion);
                }
            }
            else if (Mathe.ZahlImBereich(910, 949, Windowmanager.Befehl))    //Objektvariante
            {
                if (Editor.Tool == Editortool.Objektvariante) Editor.AuswahlObjektvariation = Editor.AuswahlObjektkategorie.Variante[Windowmanager.Befehl - 910];
                else if (Editor.Tool == Editortool.Aktion) 
                {
                    Windowmanager.GetWindowOfType(Windowtype.Objektvariante).Feld = Windowmanager.GetWindowPosition(Windowtype.Objektkategorie);
                    Editor.AuswahlAction.Wert = Windowmanager.Befehl- 910;
                    Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion);
                }
            }
            else if (Mathe.ZahlImBereich(950, 990, Windowmanager.Befehl))    //Objektvariante Vorschau
            {
                Editor.VorschauObjektvariation = Editor.AuswahlObjektkategorie.Variante[Windowmanager.Befehl - 950];
            }
            else if (Mathe.ZahlImBereich(1010, 1049, Windowmanager.Befehl))    //Eventdetails
            {
                Editor.AuswahlEvent = Eventmanager.Events[Windowmanager.Befehl - 1010];
                Editor.ChangeTool(Editortool.Event);
            }
            else if (Mathe.ZahlImBereich(1100, 1124, Windowmanager.Befehl))    //Auslöser
            {
                Editor.AuswahlTrigger = Editor.AuswahlEvent.Triggers[Windowmanager.Befehl - 1100];
                Editor.ChangeTool(Editortool.Start); /*Editor.ChangeTool(Editortool.Event);*/ Editor.ChangeTool(Editortool.Ausloeser);
            }
            else if (Mathe.ZahlImBereich(1125, 1149, Windowmanager.Befehl))    //Bedingungen
            {
                Editor.AuswahlCondition = Editor.AuswahlEvent.Conditions[Windowmanager.Befehl - 1125];
                Editor.ChangeTool(Editortool.Start); /*Editor.ChangeTool(Editortool.Event);*/ Editor.ChangeTool(Editortool.Bedingung);
            }
            else if (Mathe.ZahlImBereich(1150, 1199, Windowmanager.Befehl))    //Aktion
            {
                Editor.AuswahlAction = Editor.AuswahlEvent.Actions[Windowmanager.Befehl - 1150];
                Editor.ChangeTool(Editortool.Start); /*Editor.ChangeTool(Editortool.Event);*/ Editor.ChangeTool(Editortool.Aktion);
            }
            else if (Mathe.ZahlImBereich(1510, 1549, Windowmanager.Befehl))    //Eventelementtyp
            {
                if (Editor.AuswahlTrigger != null) { Editor.AuswahlTrigger.Type = (Triggertype)Enum.Parse(typeof(Triggertype), Enum.GetName(typeof(Triggertype), Windowmanager.Befehl - 1510).ToString()); Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Ausloeser); }
                else if (Editor.AuswahlCondition != null) { Editor.AuswahlCondition.Type = (Conditiontype)Enum.Parse(typeof(Conditiontype), Enum.GetName(typeof(Conditiontype), Windowmanager.Befehl - 1510).ToString()); Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Bedingung); }
                else if (Editor.AuswahlAction != null) { Editor.AuswahlAction.Type = (Actiontype)Enum.Parse(typeof(Actiontype), Enum.GetName(typeof(Actiontype), Windowmanager.Befehl - 1510).ToString()); Editor.ChangeTool(Editortool.Event); Editor.ChangeTool(Editortool.Aktion); }
            }
            else if (Mathe.ZahlImBereich(1610, 1649, Windowmanager.Befehl))    //Gegnertyp
            {
                Editor.AuswahlGegner = Enemymanager.Enemydatabase.Enemys[Windowmanager.Befehl - 1610];
            }
            else if (Mathe.ZahlImBereich(1650, 1690, Windowmanager.Befehl))    //Gegnertyp Vorschau
            {
                Editor.VorschauGegner = Enemymanager.Enemydatabase.Enemys[Windowmanager.Befehl - 1650];
            }
            else if (Mathe.ZahlImBereich(1710, 1749, Windowmanager.Befehl))    //Itemtyp
            {
                Editor.AuswahlItem = Itemmanager.Datenbank[Windowmanager.Befehl - 1710];
            }
            else if (Mathe.ZahlImBereich(1750, 1790, Windowmanager.Befehl))    //Itemtyp Vorschau
            {
                Editor.VorschauItem = Itemmanager.Datenbank[Windowmanager.Befehl - 1750];
            }
            else if (Mathe.ZahlImBereich(1810, 1849, Windowmanager.Befehl))    //Schalter
            {
                Editor.AuswahlLever = Levermanager.Levers[Windowmanager.Befehl - 1810];
                Editor.ChangeTool(Editortool.Schaltereinstellungen);
            }
        }

        public void EingabezelleWertLesen(Windowelement eingabezelle)
        {
            switch (eingabezelle.IDLesen)
            {
                case 1: eingabezelle.Text = (Editor.AuswahlLayer - 1).ToString(); break;
                case 2: eingabezelle.Text = Editor.AuswahlScrolling.ToString(); break;
                case 3: eingabezelle.Text = Editor.Tool.ToString(); break;
                case 4: eingabezelle.Text = ((int)Cursor.Mouseposition.X + (int)Player.PositionCurrent.X).ToString(); break;
                case 5: eingabezelle.Text = ((int)Cursor.Mouseposition.Y).ToString(); break;
                case 6: eingabezelle.Text = (Editor.AuswahlSkalierung * 100).ToString(); break;
                case 7: eingabezelle.Text = Editor.Raster.ToString(); break;
                case 8: if (Editor.AuswahlHintergrund.Textur != null) eingabezelle.Text = Editor.AuswahlHintergrund.Textur.Width.ToString(); break;
                case 9: if (Editor.AuswahlHintergrund.Textur != null) eingabezelle.Text = Editor.AuswahlHintergrund.Textur.Height.ToString(); break;
                case 10: eingabezelle.Text = Editor.AuswahlHintergrund.versatzY.ToString(); break;
                case 11: eingabezelle.Text = (Editor.AuswahlHintergrund.Scrollgeschwindigkeit.X * 100).ToString(); break;
                case 12: eingabezelle.Text = (Editor.AuswahlHintergrund.Scrollgeschwindigkeit.Y * 100).ToString(); break;
                case 13: eingabezelle.Text = (Editor.AuswahlHintergrund.Transparenz * 100).ToString(); break;
                case 14: eingabezelle.Text = Levelmanager.AktuellesLevel.Name; break;
                case 15: eingabezelle.Text = Editor.AuswahlEvent.Name; break;
                case 16: eingabezelle.Text = Editor.AuswahlTrigger.Type.ToString(); break;
                case 17: eingabezelle.Text = Editor.AuswahlCondition.Type.ToString(); break;
                case 18: eingabezelle.Text = Editor.AuswahlAction.Type.ToString(); break;
                case 19: if (Editor.Rasterplatzierung) eingabezelle.Text = "Raster"; else eingabezelle.Text = "Pixel"; break;
                case 20: eingabezelle.Text = Editor.AuswahlRichtung.ToString(); break;
                case 21: eingabezelle.Text = Editor.AuswahlObjektebene.ToString(); break;
                case 22: if (Editor.AuswahlEvent.Repeat) eingabezelle.Text = "An"; else eingabezelle.Text = "aus"; break;
                case 23: eingabezelle.Text = Editor.AuswahlTrigger.Position.X.ToString(); break;
                case 24: eingabezelle.Text = Editor.AuswahlTrigger.Wert.ToString(); break;
                case 25: if (Editor.AuswahlTrigger.Ausrichtung) eingabezelle.Text = "ueberschreiten"; else eingabezelle.Text = "unterschreiten"; break;
                case 26: eingabezelle.Text = Editor.AuswahlCondition.Position.X.ToString(); break;
                case 27: eingabezelle.Text = Editor.AuswahlCondition.Depression.ToString(); break;
                case 28: if (Editor.AuswahlCondition.Ausrichtung) eingabezelle.Text = "ueberschreiten"; else eingabezelle.Text = "unterschreiten"; break;
                case 29: eingabezelle.Text = Editor.AuswahlEvent.Zustand.ToString(); break;
                case 30: eingabezelle.Text = (Editor.AuswahlEvent.CooldownStart / 60).ToString(); break;
                case 31: eingabezelle.Text = (Editor.AuswahlEvent.CooldownCurrent / 60).ToString(); break;
                case 32: if (Editor.AuswahlAction.Ausrichtung) eingabezelle.Text = "steigern um"; else eingabezelle.Text = "senken um"; break;
                case 33: eingabezelle.Text = Editor.AuswahlAction.Wert.ToString(); break;
                case 34: eingabezelle.Text = Editor.AuswahlAction.DepressionState.ToString(); break;
                case 35: eingabezelle.Text = Editor.AuswahlAction.Effektwechsel.ToString(); break;
                case 36: eingabezelle.Text = Editor.AuswahlAction.Gegnertyp.ToString(); break;
                case 37: eingabezelle.Text = "X:" + Editor.AuswahlAction.Position.X.ToString() + " Y:" + Editor.AuswahlAction.Position.Y.ToString(); break;
                case 38: eingabezelle.Text = Editor.AuswahlAction.Gegneraktion.ToString(); break;
                case 39: eingabezelle.Text = Editor.AuswahlAction.Richtung.ToString(); break;
                case 40: eingabezelle.Text = Editor.AuswahlAction.Gegnerstatus.ToString(); break;
                case 41: if (Editor.AuswahlCondition.Ausrichtung) eingabezelle.Text = "Rechts von"; else eingabezelle.Text = "Links von"; break;
                case 42: eingabezelle.Text = Editor.AuswahlAction.Name; break;
                case 43: if (Editor.AnzeigenKollision) eingabezelle.Text = "An"; else eingabezelle.Text = "Aus"; break;
                case 44: if (Editor.AnzeigenEvents) eingabezelle.Text = "An"; else eingabezelle.Text = "Aus"; break;
                case 45: if (Editor.AuswahlHeilung) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 46: eingabezelle.Text = "X:" + Levelmanager.AktuellesLevel.Startposition.X.ToString() + " Y:" + Levelmanager.AktuellesLevel.Startposition.Y.ToString(); break;
                case 47: eingabezelle.Text = Player.Ground.ToString(); break;
                case 48: eingabezelle.Text = Editor.AuswahlAction.Position.X.ToString(); break;
                case 49: eingabezelle.Text = Editor.AuswahlAction.Position.Y.ToString(); break;
                case 50: eingabezelle.Text = Levelmanager.AktuellesLevel.DepressionRate.ToString(); break;
                case 51: eingabezelle.Text = Editor.RasterOffset.ToString(); break;
                case 52: eingabezelle.Text = Math.Round((((float)Levelmanager.AktuellesLevel.VordergrundAbdunkelung / 255) * 100)).ToString(); break;
                case 53: eingabezelle.Text = Editor.AuswahlLever.Name; break;
                case 54: if (Editor.AuswahlLever.Einmalig) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 55: if (Editor.AuswahlTrigger.Lever != null) eingabezelle.Text = Editor.AuswahlTrigger.Lever.Name; else eingabezelle.Text = "Nicht gewaehlt"; break;
                case 56: if (Editor.AuswahlLever.Betätigt) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 57: if (Editor.AuswahlLever.Rücksetzen) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 58: if (Editor.AuswahlCondition.Lever != null) eingabezelle.Text = Editor.AuswahlCondition.Lever.Name; else eingabezelle.Text = "Nicht gewaehlt"; break;
                case 59: if (Editor.AuswahlCondition.Ausrichtung) eingabezelle.Text = "Betaetigt"; else eingabezelle.Text = "Unbetaetigt"; break;
                case 60: eingabezelle.Text = Editor.AuswahlAction.Objektaktion.ToString(); break;
                case 61: eingabezelle.Text = Editor.AuswahlAction.Objecttyp.ObjectClass.ToString(); break;
                case 62: eingabezelle.Text = Editor.AuswahlAction.Layer.ToString(); break;
                case 63: eingabezelle.Text = (Editor.AuswahlAction.Skalierung * 100).ToString(); break;
                case 64: eingabezelle.Text = Editor.AuswahlAction.Objecttyp.Variante[Editor.AuswahlAction.Wert].Name.ToString(); break;
                case 65: eingabezelle.Text = Editor.AuswahlAction.Objektebene.ToString(); break;
                case 66: eingabezelle.Text = Editor.AuswahlTrigger.Enemytype.ToString();break;
                case 67: eingabezelle.Text = "X:" + Editor.AuswahlTrigger.Position.X.ToString() + " Y:" + Editor.AuswahlTrigger.Position.Y.ToString(); break;
                case 68: eingabezelle.Text = Editor.AuswahlTrigger.Enemystate.ToString(); break;
                case 69: if (Editor.AuswahlStatisch) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 70: if (Editor.AuswahlAction.Statisch) eingabezelle.Text = "Ja"; else eingabezelle.Text = "Nein"; break;
                case 71: if (Editor.Autosaveintervall == 0) eingabezelle.Text = "deaktiviert"; else eingabezelle.Text = (Editor.Autosaveintervall / 60).ToString(); break;
                case 72: eingabezelle.Text = Editor.AuswahlHintergrund.Startposition.ToString(); break;
                case 73: eingabezelle.Text = Editor.AuswahlHintergrund.Endposition.ToString(); break;
                default: break;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Keymanager.Draw(spritebatch);
        }
    }
}
