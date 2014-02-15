using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    enum Windowtype
    {
        MainMenue,
        Topbar,
        Editor,
        Infofenster,
        Editoreinstellungen,
        Neu,
        Laden,
        Hintergrundtextur,
        Leveleinstellungen,
        Hintergruende,
        Hintergrund,
        Objektkategorie,
        Objektvariante,
        Levers,
        Lever,
        Events,
        Event,
        Ausloeser,
        Bedingung,
        Aktion,
        Typ,
        Gegner,
        Items,
    }

    class Windowdatabase
    {
        private Contents Contents;
        private Player Player;
        private Inputmanager Inputmanager;
        public List<Window> Windows = new List<Window>();
        private Windowtype Type;
        private Rectangle Feld;
        private List<Windowtype> Schließen = new List<Windowtype>();
        private List<Windowelement> WindowElement = new List<Windowelement>();
        private List<Button> Buttons = new List<Button>();

        public Windowdatabase(Contents contents, Player player, Inputmanager inputmanager)
        {
            this.Contents = contents;
            this.Player = player;
            this.Inputmanager = inputmanager;
            FensterLaden();
        }

        private void FensterLaden()
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.MainMenue;
            Feld = new Rectangle(0, 0, 1280, 1024);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 1280, 1024), Contents.Menue));
            Buttons.Add(new Button(new Rectangle(440, 650, 100, 100), Contents.MenueButtons, new Rectangle(0, 0, 201, 185), new Rectangle(201, 0, 201, 185), Color.White, 2, 0, 0, true)); //Renew
            Buttons.Add(new Button(new Rectangle(370, 650, 100, 100), Contents.MenueButtons, new Rectangle(0, 185, 201, 200), new Rectangle(201, 185, 201, 200), Color.White, 1, 0, 0, true)); //Play
            Buttons.Add(new Button(new Rectangle(720, 650, 100, 100), Contents.MenueButtons, new Rectangle(0, 185 + 200, 201, 199), new Rectangle(201, 185 + 200, 201, 199), Color.White, 3, 0, 0, true)); //Settings
            //Buttons.Add(new Button(new Rectangle(905, 420, 100, 100), Contents.MenueButtons, new Rectangle(0, 185 + 200 + 199, 201, 199), new Rectangle(201, 185 + 200+199, 201, 199), Color.White, 4, 0, 0, true)); //playding
            Buttons.Add(new Button(new Rectangle(830, 650, 100, 100), Contents.MenueButtons, new Rectangle(0, 185 + 200 + 199 + 199, 201, 200), new Rectangle(201, 185 + 200 + 199 + 199, 201, 200), Color.White, 3, 0, 0, true)); //exit

            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Topbar;
            Feld = new Rectangle(0, 0, 226, 21);
            Schließen.Add(Windowtype.MainMenue);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 21), Contents.Bar));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Menue", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(0, 0, 226, 21), 10, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Editor;
            Feld = new Rectangle(0, 0, 226, 388);
            WindowElement.Add(new Windowelement(new Vector2(0, 0),WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Menue", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 11, 0, 0, true)); //Fenster schließen
            int LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Info & Einstellungen", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Infofenster", false, Color.Blue, 12, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Editoreinstellungen", false, Color.Blue, 13, 0, 0)); LäuferY += 30;
            WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Laden & Speichern", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Neues Level erstellen", false, Color.Blue, 31, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Level laden", false, Color.Blue, 14, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Level speichern", false, Color.Blue, 15, 0, 0)); LäuferY += 30;
            WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Tools", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Leveleinstellungen", false, Color.Blue, 16, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Hintergruende", false, Color.Blue, 17, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Objekte", false, Color.Blue, 18, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Events", false, Color.Blue, 19, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Gegner", false, Color.Blue, 20, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Items", false, Color.Blue, 21, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Schalter", false, Color.Blue, 22, 0, 0)); LäuferY += 15;
            //WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Verlassen", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            //Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Ins Spiel springen", false, Color.Blue, 0, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Programm beenden", false, Color.Red, 30, 0, 0)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Infofenster;
            Feld = new Rectangle(0, 636, 226, 388);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Info", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 100, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 101, 0, 0, true)); //Fenster schließen
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Werkzeug", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(85, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 3, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 30;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Layer", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 1, 3001, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Zeigerposition X", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 4, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Zeigerposition Y", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 5, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Skalierung                                 %", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 6, 3003, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Rasteroffset Y", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 51, 3037, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Pixelraster", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 7, 3004, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Platzierart", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 19, 3012, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Blickrichtung", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 20, 3013, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ebene", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 21, 3014, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Statisch", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 69, 3052, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Editoreinstellungen;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Editoreinstellungen", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 200, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 201, 0, 0, true)); //Fenster schließen
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Allgemeines", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Scrollspeed", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 2, 3002, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 2)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Autosaveintervall                       s", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 71, 3054, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 2)); LäuferY += 30;
            WindowElement.Add(new Windowelement(new Vector2(113, LäuferY), WindowElementType.Text, "Anzeigeeinstellungen", Contents.Meiryo8, Color.White, true)); LäuferY += 25;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Kollision", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 43, 3000, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Events", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 44, 3031, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Neu;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Neu", Contents.Meiryo8, Color.White, true));
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Name", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 14, 3009, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 298, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 299, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Laden;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Laden", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 300, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 301, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Hintergrundtextur;
            Feld = new Rectangle(226, 0, 226, 388); 
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Hintergrundtextur", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 400, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 401, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Leveleinstellungen;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Leveleinstellungen", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 500, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 501, 0, 0, true)); //Fenster schließen
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Name", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 0, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Startposition", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 46, 3016, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Walkline", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 47, 3033, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Depressionsrate", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 50, 3036, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Vordergrundfarbe                       %", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 52, 3038, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Karte loeschen", false, Color.Red, 502, 0, 0)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Hintergruende;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Hintergruende", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 600, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 601, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Hintergrund hinzufuegen", true, Color.Blue, 650, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Hintergrund;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Hintergrund", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 700, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 701, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 702, 0, 0, true)); //Zurück
            LäuferY = 40;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Textur aendern", false, Color.Blue, 750, 0, 0)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Texturbreite", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 8, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Texturhoehe", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 9, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Y Position", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 10, 3005, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Scrollfaktor X                             %", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 11, 3006, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Scrollfaktor Y                             %", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 12, 3007, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Sichtbarkeit                               %", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 13, 3008, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 3)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Startposition", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 72, 3034, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Endposition", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 73, 3035, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 5)); LäuferY += 30;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Nach vorne verschieben", false, Color.Blue, 751, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Nach hinten verschieben", false, Color.Blue, 752, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Hintergrund loeschen", false, Color.Red, 753, 0, 0)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Objektkategorie;
            Feld = new Rectangle(226, 0, 226, 388);
            //EditorFensterSchließen();
            Schließen.Add(Windowtype.Editoreinstellungen);
            Schließen.Add(Windowtype.Laden);
            Schließen.Add(Windowtype.Hintergrundtextur);
            Schließen.Add(Windowtype.Leveleinstellungen);
            Schließen.Add(Windowtype.Hintergruende);
            Schließen.Add(Windowtype.Hintergrund);
            Schließen.Add(Windowtype.Objektkategorie);
            Schließen.Add(Windowtype.Objektvariante);
            Schließen.Add(Windowtype.Events);
            Schließen.Add(Windowtype.Levers);
            Schließen.Add(Windowtype.Lever);
            Schließen.Add(Windowtype.Ausloeser);
            Schließen.Add(Windowtype.Bedingung);
            Schließen.Add(Windowtype.Aktion);
            Schließen.Add(Windowtype.Typ);
            Schließen.Add(Windowtype.Gegner);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Objektkategorie", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 800, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 801, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Objektvariante;
            Feld = new Rectangle(226, 0, 226, 388);
            Schließen.Add(Windowtype.Objektkategorie);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Objektvariante", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 900, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 902, 0, 0, true)); //Zurück
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 901, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Events;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Events", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1000, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1001, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Event hinzufuegen", false, Color.Blue, 1050, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Event;
            Feld = new Rectangle(226, 0, 904, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 904, 388), Contents.EventWindow));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Event", Contents.Meiryo8, Color.White, true));
            WindowElement.Add(new Windowelement(new Vector2(339, 3), WindowElementType.Text, "Ausloeser", Contents.Meiryo8, Color.White, true));
            WindowElement.Add(new Windowelement(new Vector2(565, 3), WindowElementType.Text, "Bedingungen", Contents.Meiryo8, Color.White, true));
            WindowElement.Add(new Windowelement(new Vector2(791, 3), WindowElementType.Text, "Aktionen", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1060, 0, 0)); //Fenster verschieben
            LäuferY = 40;
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1061, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1062, 0, 0, true)); //Zurück
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Name", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(85, LäuferY + 1, 115, 14), WindowElementType.Eingabezelle, 15, 3010, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 30;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Aktiv", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 29, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Wiederholen", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 22, 3015, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Abklingzeit Start", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Vector2(204, LäuferY), WindowElementType.Text, "s", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 30, 3022, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Abklingzeit Rest", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Vector2(204, LäuferY), WindowElementType.Text, "s", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 31, 0, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 20)); LäuferY += 30;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Ausloeser hinzufuegen", false, Color.Blue, 1063, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Bedingungen hinzufuegen", false, Color.Blue, 1064, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Aktionen hinzufuegen", false, Color.Blue, 1065, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Event loeschen", false, Color.Red, 1066, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Ausloeser;
            Feld = new Rectangle(452, 0, 226, 388);
            Schließen.Add(Windowtype.Bedingung);
            Schließen.Add(Windowtype.Aktion);
            Schließen.Add(Windowtype.Typ);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Ausloeser", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1200, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1201, 0, 0, true)); //Zurück
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ausloesertyp", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 16, 3011, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Ausloeser loeschen", false, Color.Red, 1209, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Bedingung;
            Feld = new Rectangle(678, 0, 226, 388);
            Schließen.Add(Windowtype.Ausloeser); 
            Schließen.Add(Windowtype.Aktion);
            Schließen.Add(Windowtype.Typ);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Bedingung", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1300, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1301, 0, 0, true)); //Zurück
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Bedingungstyp", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 17, 3011, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Bedingung loeschen", false, Color.Red, 1309, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Aktion;
            Feld = new Rectangle(904, 0, 226, 388);
            Schließen.Add(Windowtype.Ausloeser);
            Schließen.Add(Windowtype.Bedingung);
            Schließen.Add(Windowtype.Typ);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Aktion", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1400, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1401, 0, 0, true)); //Zurück
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Aktionstyp", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 18, 3011, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Aktion loeschen", false, Color.Red, 1409, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Typ;
            Feld = new Rectangle(904, 0, 226, 388);
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Typ", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1500, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1501, 0, 0, true)); //Zurück
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Gegner;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Gegner", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1600, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1601, 0, 0, true)); //Fenster schließen
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Items;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Items", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1700, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1701, 0, 0, true)); //Fenster schließen
            WindowElement.Add(new Windowelement(new Vector2(10, 350), WindowElementType.Text, "Heilt Effekte", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, 350 + 1, 85, 14), WindowElementType.Eingabezelle, 45, 3032, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, true, 1)); LäuferY += 15;
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Levers;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Schalter", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1800, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1801, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Schalter hinzufuegen", false, Color.Blue, 1850, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Type = Windowtype.Lever;
            Feld = new Rectangle(226, 0, 226, 388);
            EditorFensterSchließen();
            WindowElement.Add(new Windowelement(new Vector2(0, 0), WindowElementType.Texture, new Rectangle(0, 0, 226, 388), Contents.Window));
            WindowElement.Add(new Windowelement(new Vector2(113, 3), WindowElementType.Text, "Schaltereinstellungen", Contents.Meiryo8, Color.White, true));
            Buttons.Add(new Button(new Rectangle(1, 4, 222, 13), 1900, 0, 0)); //Fenster verschieben
            Buttons.Add(new Button(new Rectangle(210, 6, 11, 11), Contents.GUIParts, new Rectangle(0, 0, 11, 11), new Rectangle(11, 0, 11, 11), Color.White, 1801, 0, 0, true)); //Fenster schließen
            Buttons.Add(new Button(new Rectangle(5, 6, 11, 11), Contents.GUIParts, new Rectangle(96, 0, 13, 13), new Rectangle(96, 15, 13, 13), Color.White, 1802, 0, 0, true)); //Zurück
            LäuferY = 40;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Name", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 53, 3039, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 12)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Einmalig", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 54, 3040, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 12)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Betaetigt", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 56, 3042, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 12)); LäuferY += 15;
            WindowElement.Add(new Windowelement(new Vector2(10, LäuferY), WindowElementType.Text, "Ruecksetzen", Contents.Meiryo8, Color.White, false));
            WindowElement.Add(new Windowelement(new Rectangle(115, LäuferY + 1, 85, 14), WindowElementType.Eingabezelle, 57, 3043, Contents.Block, Contents.Meiryo8, Color.Blue, Color.White, false, 12)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, LäuferY, 140, 15), Contents.Meiryo8, "Position aendern", false, Color.Blue, 1803, 0, 0)); LäuferY += 15;
            Buttons.Add(new Button(new Rectangle(10, 350, 140, 15), Contents.Meiryo8, "Schalter loeschen", false, Color.Red, 1809, 0, 0));
            FensterErstellen();
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }

        private void FensterErstellen()
        {
            Windows.Add(new Window(Contents, Type, Feld, Schließen, WindowElement, Buttons));
            Schließen = new List<Windowtype>();
            WindowElement = new List<Windowelement>();
            Buttons = new List<Button>();
        }

        private void EditorFensterSchließen()
        {
            Schließen.Add(Windowtype.Editoreinstellungen);
            Schließen.Add(Windowtype.Laden);
            Schließen.Add(Windowtype.Hintergrundtextur);
            Schließen.Add(Windowtype.Leveleinstellungen);
            Schließen.Add(Windowtype.Hintergruende);
            Schließen.Add(Windowtype.Hintergrund);
            Schließen.Add(Windowtype.Objektkategorie);
            Schließen.Add(Windowtype.Objektvariante);
            Schließen.Add(Windowtype.Events);
            Schließen.Add(Windowtype.Event);
            Schließen.Add(Windowtype.Levers);
            Schließen.Add(Windowtype.Lever);
            Schließen.Add(Windowtype.Ausloeser);
            Schließen.Add(Windowtype.Bedingung);
            Schließen.Add(Windowtype.Aktion);
            Schließen.Add(Windowtype.Typ);
            Schließen.Add(Windowtype.Gegner);
        }

        public Window GetWindowOfType(Windowtype windowtype) //Fenster eines bestimmten Typs abfragen
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].Windowtype == windowtype)
                {
                    return Windows[i];
                }
            }
            return null;
        }
    }
}
