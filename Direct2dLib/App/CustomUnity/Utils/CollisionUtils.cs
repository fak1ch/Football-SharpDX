using Direct2dLib.App.CustomUnity.Components;
using SharpDX;
using System;

namespace Direct2dLib.App.CustomUnity.Utils
{
    public static class CollisionUtils
    {
        public static bool CircleIntersectsCircle(Circle2D first, Circle2D second)
        {
            float x1 = first.x;
            float y1 = first.y;
            float r1 = first.r;

            float x2 = second.x;
            float y2 = second.y;
            float r2 = second.r;

            float distance = Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));

            return distance <= r1 + r2;
        }

        public static bool CircleIntersectsRectangle(Circle2D circle, RectangleF rectangle)
        {
            float closestX = MathUtils.Clamp(circle.x, rectangle.Left, rectangle.Right);
            float closestY = MathUtils.Clamp(circle.y, rectangle.Top, rectangle.Bottom);

            float distanceX = circle.x - closestX;
            float distanceY = circle.y - closestY;

            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

            return distanceSquared < (circle.r * circle.r);
        }

        public static bool PointIntersectsRectangle(System.Drawing.Point point, RectangleF rectangle)
        {
            return rectangle.Contains(new Vector2(point.X, point.Y));
        }
    }
}
