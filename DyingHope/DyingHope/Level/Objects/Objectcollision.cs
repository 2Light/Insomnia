using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Objectcollision
    {
        public Rectangle Zone;
        public bool Zerstörbar;
        public bool Spieler;
        public bool Gegner;

        public Objectcollision(Rectangle zone)
        {
            this.Zone = zone;
        }
    }
}
