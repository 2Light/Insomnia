using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    [Flags]
    public enum DebugFlag
    {
        CollisonShape = (1 << 0),
        PlayerStats = (1 << 1),
        PhysikDebug = (1 << 2),
        ObjectInformation = (1 << 3),
        EditorStats = (1 << 4),
        Inputstats = (1 << 5),
        KammeraStats = (1 << 6),
        ObjectmanagerStats = (1<<7),
        BackgrundStats = (1<<8),
        WindowStats = (1<<9),
        DepressionStats = (1<<10),
        Pertikle = (1 << 11)
    }

    class DebugView
    {
        public DebugFlag Flags { get; set; }
        public DebugFlag extraFlags { get; set; }

        private String DebugText;
        public StringBuilder debugTextBuilder;
        public List<IdebugObjekt> debugObjects;
        public Camera cam;
        public Contents content;
        public Vector2 Position;

        public DebugView(Contents content,Camera cam)
        {
            debugObjects = new List<IdebugObjekt>();
            debugTextBuilder = new StringBuilder();
            this.content = content;
            this.cam = cam;
            this.Position = new Vector2(0, 0);

            setFlag(DebugFlag.DepressionStats,false);
        }

        public void Update()
        {
            this.Clear(); // Vor neuem hinzufügen die Strings Leeren 
          
            foreach (IdebugObjekt dO in this.debugObjects)
            {
                dO.handelDebug(); // vom DebugObjekt die Daten bearbeiten lassen
               
                if ((dO.debugFlag & this.Flags) >0) // wenn die Flag gesetzt ist
                {
                    debugTextBuilder.AppendLine(dO.stringBuilder.ToString());
                    debugTextBuilder.AppendLine(String.Empty);
                }
                dO.clearString(); // daten wieder löschen 
               
            }
            this.DebugText = this.debugTextBuilder.ToString();
        }


        public void Clear()
        {
            this.debugTextBuilder.Clear();
            this.DebugText = "";
        }

        public void Add(IdebugObjekt objekt)
        {
            this.debugObjects.Add(objekt);
        }

        public void AppendFlag(DebugFlag flag, bool extra) // fügt mit dem OR operator die Flag hinzu
        {
            if (extra) extraFlags |= flag;
            else Flags |= flag;
        }

        public void RemoveFlag(DebugFlag flag, bool extra)// entfernt mit dem NAND operator die Flag 
        {
            if (extra) extraFlags &= ~flag;
            else Flags &= ~flag;
        }

        public void setFlag(DebugFlag flag, bool extra) // schaut ob die Flag vorhanden ist oder nicht und reagiert dementsprechend
        {
            if ((Flags & flag) == flag ||( extraFlags & flag) == flag)
                RemoveFlag(flag, extra);
            else
                AppendFlag(flag, extra);
        }
        
        public void Draw(SpriteBatch batch) // Debug String ausgabe
        {

            batch.Begin();
            batch.DrawString(this.content.Meiryo8, this.DebugText, this.Position, Color.White);
            batch.End();

            foreach (IdebugObjekt dO in this.debugObjects) // Falls extra Flags gesetzt, werden sie abgearbeitet
            {
                if ((dO.debugFlag & extraFlags)>0) dO.DrawExtraDebug(this.extraFlags,batch,this.cam,this.content);
            }

        }
    }
}
