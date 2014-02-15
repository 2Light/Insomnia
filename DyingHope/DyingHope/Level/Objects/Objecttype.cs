using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class ObjectType
    {
        public ObjectClass ObjectClass;
        public List<Objectvariation> Variante = new List<Objectvariation>(); //Liste der verschieden Objektvarianten einer Art
        public string Texturpfad;    //Bilddatei
        public Texture2D Textur;

        public ObjectType() { }
        public ObjectType(ObjectClass objectclass, string texturpfad)
        {
            this.ObjectClass = objectclass;
            this.Texturpfad = texturpfad;
        }
    }
}
