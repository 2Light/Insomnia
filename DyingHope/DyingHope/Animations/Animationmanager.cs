using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Animationsmanager
    {
        public List<Animation> Animation = new List<Animation>();

        public void AddAnimation(Animation animation)
        {
            Animation.Add(animation);  //Neue Animation erstellen
        }

        public void AddAnimation(Texture2D texture, bool wiederholen, int spalte, int breite, int höhe, float skalierung, int geschwindigkeit)
        {
            Animation.Add(new Animation(texture, wiederholen, spalte, breite, höhe, skalierung, geschwindigkeit));  //Neue Animation erstellen
        }

        public void RemoveAnimation(Animation animation)
        {
            Animation.Remove(animation);    //Animation löschen
        }

        public void Update(float elapsed)
        {
            for (int i = 0; i < Animation.Count; i++)
            {
                Animation[i].Update(elapsed);   //Animationen abspielen
            }
        }
    }
}
