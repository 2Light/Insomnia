using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    class Levelmanager
    {
        public Level AktuellesLevel;
        public List<Level> Levels = new List<Level>();
        public Vector2 Checkpoint;
        private Player Player;
        private Objectmanager Objectmanager;
        private Animationsmanager Animationsmanager;
        private Backgroundmanager Backgroundmanager;
        private Enemymanager Enemymanager;
        private Levermanager Levermanager;
        private Eventmanager Eventmanager;
        private Itemmanager Itemmanager;
        private Contents Contents;
        private string Quellpfad = @"Levels";

        public Levelmanager(Player player, Objectmanager objectmanager, Animationsmanager animationsmanager, Backgroundmanager backgroundmanager, Enemymanager enemymanager, Itemmanager itemmanager, Levermanager levermanager, Eventmanager eventmanager, Contents contents)
        {
            this.Player = player;
            this.Objectmanager = objectmanager;
            this.Objectmanager.Levelmanager = this;
            this.Animationsmanager = animationsmanager;
            this.Backgroundmanager = backgroundmanager;
            this.Enemymanager = enemymanager;
            this.Levermanager = levermanager;
            this.Itemmanager = itemmanager;
            this.Eventmanager = eventmanager;
            this.Eventmanager.Levelmanager = this;
            this.Contents = contents;
            player.Levelmanager = this;
            if (System.IO.Directory.Exists(Quellpfad))
            {
                string[] folders = System.IO.Directory.GetDirectories(Quellpfad);
                foreach (string name in folders)
                {
                    Levels.Add(new Level(name.Substring(7)));
                }
            }
            LoadLevel("Stadt");
        }

        public void NewLevel(string name)
        {
            AktuellesLevel = new Level(name);
            Levels.Add(AktuellesLevel);
            System.IO.Directory.CreateDirectory(@"Levels\" + AktuellesLevel.Name);
            Objectmanager.ObjectsHintergrund.Clear();
            Objectmanager.ObjectsSpielebene.Clear();
            Objectmanager.ObjectsVordergrund.Clear();
            Backgroundmanager.Backgrounds.Clear();
            Enemymanager.Enemys.Clear();
            Itemmanager.Items.Clear();
            Eventmanager.Events.Clear();
            Levermanager.Levers.Clear();
            SaveLevelEditor();
        }

        public void LoadLevel(string name)
        {
            foreach (Level level in Levels)
            {
                if (level.Name == name) AktuellesLevel = level;
            }
            AktuellesLevel.LoadLevel(@"Levels\" + AktuellesLevel.Name + @"\Level.xml");
            Checkpoint = new Vector2(AktuellesLevel.Startposition.X, AktuellesLevel.Startposition.Y);
            Player.PositionCurrent = new Vector2(AktuellesLevel.Startposition.X, AktuellesLevel.Startposition.Y);
            Player.Ground = AktuellesLevel.Walkline;
            Player.DepressionRate = AktuellesLevel.DepressionRate;
            //if (Player.Ground == 0) Player.Ground = 715;
            Objectmanager.LoadObjects(@"Levels\" + AktuellesLevel.Name + @"\Objects.xml");
            Backgroundmanager.LoadBackgrounds(@"Levels\" + AktuellesLevel.Name + @"\Backgrounds.xml");
            Enemymanager.LoadEnemys(@"Levels\" + AktuellesLevel.Name + @"\Enemys.xml");
            Levermanager.LoadLevers(@"Levels\" + AktuellesLevel.Name + @"\Levers.xml");
            Eventmanager.LoadEvents(@"Levels\" + AktuellesLevel.Name + @"\Events.xml");
            Itemmanager.LoadItems(@"Levels\" + AktuellesLevel.Name + @"\Items.xml");
        }

        public void SaveLevelProgress()
        {

        }

        public void SaveLevelEditor()
        {
            AktuellesLevel.SaveLevel(@"Levels\" + AktuellesLevel.Name + @"\Level.xml");
            Objectmanager.SaveObjects(@"Levels\" + AktuellesLevel.Name + @"\Objects.xml");
            Backgroundmanager.SaveBackgrounds(@"Levels\" + AktuellesLevel.Name + @"\Backgrounds.xml");
            Enemymanager.SaveEnemys(@"Levels\" + AktuellesLevel.Name + @"\Enemys.xml");
            Eventmanager.SaveEvents(@"Levels\" + AktuellesLevel.Name + @"\Events.xml");
            Itemmanager.SaveItems(@"Levels\" + AktuellesLevel.Name + @"\Items.xml");
            Levermanager.SaveLevers(@"Levels\" + AktuellesLevel.Name + @"\Levers.xml");
        }

        public void AutoSaveEditor()
        {
            AktuellesLevel.SaveLevel(@"Levels\Autosave\Level.xml");
            Objectmanager.SaveObjects(@"Levels\Autosave\Objects.xml");
            Backgroundmanager.SaveBackgrounds(@"Levels\Autosave\Backgrounds.xml");
            Enemymanager.SaveEnemys(@"Levels\Autosave\Enemys.xml");
            Eventmanager.SaveEvents(@"Levels\Autosave\Events.xml");
            Itemmanager.SaveItems(@"Levels\Autosave\Items.xml");
            Levermanager.SaveLevers(@"Levels\Autosave\Levers.xml");
        }
    }
}
