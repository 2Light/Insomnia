using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DyingHope
{
    class Camera
    {
        private Viewport viewport;
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position;

        public Matrix projection
        {
            get
            {
                return Matrix.CreateOrthographicOffCenter(0f, CoordinatenManager.pixelToUnit * this.viewport.Width, CoordinatenManager.pixelToUnit * this.viewport.Height , 0f, 0f, 1f);
            }
        } 
       // Matrix view = Matrix.CreateTranslation(new Vector3((_cameraPosition / MeterInPixels) - (_screenCenter / MeterInPixels), 0f)) * Matrix.CreateTranslation(new Vector3((_screenCenter / MeterInPixels), 0f));
        
        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(Position - Origin, 0.0f)) *
                       Matrix.CreateScale(Zoom, Zoom, 1) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
            }
        }

        public Camera(Viewport viewport, bool widescreen)
        {
            if (widescreen) Origin = new Vector2((viewport.Width / 2.0f) + 64, (viewport.Height / 2.0f) + 640);
            else Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Zoom = 1.0f;
            this.viewport = viewport;
        }

        public void Update(Richtung richtung)
        {
            switch (richtung)
            {
                case Richtung.Links: this.Position.X += 1.5f; break;
                case Richtung.Rechts: this.Position.X -= 1.5f; break; 
                case Richtung.Hoch: this.Position.Y += 1.5f; break;
                case Richtung.Runter: this.Position.Y -= 1.5f; break;
            }
        }
    }
}
