using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DyingHope
{
    class Item
    {
        private Player Player;
        private DepressionHandler DepressionHandler;
        private ContentManager Contentmanager;
        public string Name;
        public Texture2D Textur;
        public Vector2 Position;
        public Rectangle Feld;
        public bool Kuriert;

        public Item(ContentManager contentmanager, string name)  //Kontruktor Datenbank
        {
            this.Name = name;
            this.Textur = contentmanager.Load<Texture2D>(@"Items\" + name);
            this.Feld = new Rectangle(0, 0, Textur.Width, Textur.Height);
        }

        //public Item(ContentManager contentmanager, Player player)  //Kontruktor Editor
        public Item(Player player)  //Kontruktor Editor
        {
            this.Player = player;
            //this.Contentmanager = contentmanager;
        }

        //public Item(ContentManager contentmanager, Player player, string textur, Vector2 position, bool kuriert)   //Konstruktor Manager
        //{
        //    this.Player = player;
        //    this.Contentmanager = contentmanager;
        //    this.Textur = contentmanager.Load<Texture2D>(@"Items\" + textur);
        //    this.Position = position;
        //    this.Feld = new Rectangle((int)(Position.X - (Textur.Width / 2)), (int)(Position.Y - (Textur.Height / 2)), Textur.Width, Textur.Height);
        //    this.Kuriert = kuriert;
        //}

        public Item(Player player, DepressionHandler depressionhandler, string name, Texture2D textur, Vector2 position, bool kuriert)   //Konstruktor Manager
        {
            this.Player = player;
            this.DepressionHandler = depressionhandler;
            //this.Contentmanager = contentmanager;
            //this.Textur = contentmanager.Load<Texture2D>(@"Items\" + textur);
            this.Name = name;
            this.Textur = textur;
            this.Position = position;
            this.Feld = new Rectangle((int)(Position.X - (Textur.Width / 2)), (int)(Position.Y - (Textur.Height / 2)), Textur.Width, Textur.Height);
            this.Kuriert = kuriert;
        }

        public bool Update()
        {
            if (Feld.Intersects(new Rectangle((int)(Player.PositionCurrent.X + Player.Kollision.X + 540), (int)(Player.PositionLast.Y + Player.Kollision.Y), Player.Kollision.Width, Player.Kollision.Height)))
            {
                Player.DepressionSoll -= 0.3f;
                if (Kuriert)
                {
                    if (DepressionHandler.isSet(DepressionState.GrayScal)) DepressionHandler.setFlag(DepressionState.GrayScal);
                    if (DepressionHandler.isSet(DepressionState.InvertMove)) DepressionHandler.setFlag(DepressionState.InvertMove);
                    if (DepressionHandler.isSet(DepressionState.InvertScreen)) DepressionHandler.setFlag(DepressionState.InvertScreen);
                    if (DepressionHandler.isSet(DepressionState.ModifyWorld)) DepressionHandler.setFlag(DepressionState.ModifyWorld);
                    if (DepressionHandler.isSet(DepressionState.ReduceFOV)) DepressionHandler.setFlag(DepressionState.ReduceFOV);
                    if (DepressionHandler.isSet(DepressionState.Slow)) DepressionHandler.setFlag(DepressionState.Slow);
                    if (DepressionHandler.isSet(DepressionState.Srink)) DepressionHandler.setFlag(DepressionState.Srink);
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Textur, new Vector2(Feld.X - Player.PositionCurrent.X, Feld.Y), Color.White);  
        }
    }
}
