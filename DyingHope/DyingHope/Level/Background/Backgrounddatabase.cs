using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace DyingHope
{
    class Backgrounddatabase
    {
        public List<Background> Backgrounds;
        private ContentManager Contentmanager;
        private string Quellpfad = @"Content\Backgrounds\";

        public Backgrounddatabase(ContentManager contentmanager)
        {
            this.Contentmanager = contentmanager;
            this.Backgrounds = new List<Background>();
            Load();
        }

        public void Load()
        {
            if (System.IO.Directory.Exists(Quellpfad))
            {
                string[] folders = System.IO.Directory.GetFiles(Quellpfad);
                foreach (string s in folders)
                {
                    Backgrounds.Add(new Background(s.Substring(20, s.Length - 24), Contentmanager));
                }
            }
        }
    }
}
