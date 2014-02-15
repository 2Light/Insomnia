using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DyingHope
{
    class CoordinatenManager
    {
        public const float unitToPixel = 100.0f;
        public const float pixelToUnit = 1 / unitToPixel;


        public static Vector2 ToScreenCoordinat(Vector2 worldCoordinats)
        {
            return worldCoordinats * unitToPixel;
        }

        public static Vector2 toWorldCoordinat(Vector2 screenCoordinates)
        {
            return screenCoordinates * pixelToUnit;
        }

    }
}
