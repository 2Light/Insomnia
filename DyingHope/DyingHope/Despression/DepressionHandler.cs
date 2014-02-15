using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    public enum DepressionState
    {
        InvertMove = (1 << 0),
        Srink = (1 << 1),
        Slow = (1 << 2),
        InvertScreen = (1 << 3),
        ModifyWorld = (1 << 4),
        ReduceFOV = (1 << 5),
        GrayScal = (1 << 6),
        InvertHoriScreen =(1 << 7),
    }

    class DepressionHandler : IdebugObjekt
    {
        private static DepressionState Depression;
        private int onGo;

        private DepressionState toSet;

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

        public void DrawExtraDebug(DebugFlag extraFlag, SpriteBatch batch, Camera cam, Contents content)
        {

        }

        public void handelDebug()
        {
            addString("Aktuelle Depressions Effekte:" + Depression);
        }

        #endregion

        public DepressionHandler()
        {
            debugFlag = DebugFlag.DepressionStats;
            stringBuilder = new StringBuilder();
        }

        public static Boolean isSet(DepressionState state)
        {
            return ((state & Depression) == state);
        }

        public void AppendFlag(DepressionState state)
        {
            Depression |= state;
        }

        public void RemoveFlag(DepressionState state)
        {
            Depression &= ~state;
        }

        public void setFlag(DepressionState flag) // schaut ob die Flag vorhanden ist oder nicht und reagiert dementsprechend
        {
            if ((Depression & flag) == flag)
                RemoveFlag(flag);
            else
                AppendFlag(flag);
        }


        public void startDepression(DepressionState flag)
        {
            this.toSet = flag;
            onGo = 1;
        }


        public void Update()
        {
            if (onGo == 1)
            {

                Game1.glitch += 0.0001f;
                if (Game1.glitch >= 0.005)
                {
                    onGo = 2;
                    setFlag(toSet);

                }
            }
            else if (onGo == 2)
            {
                Game1.glitch -= 0.0001f;
                if (Game1.glitch <= 0.0001)
                {
                    onGo = 0;
                }
            }
        }
    }
}
