using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    interface IdebugObjekt
    {
        String debugString { get; set; }
        StringBuilder stringBuilder { get; set; }
        DebugFlag debugFlag { get; set; }

        void addString(String text);
        void clearString();
        void DrawExtraDebug(DebugFlag extraFlag,SpriteBatch batch,Camera cam,Contents content); // alles was nicht in das Debug Feld soll, wird hier gezeichnet
        void handelDebug();
    }
}
