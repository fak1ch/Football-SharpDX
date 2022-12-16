using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class CircleCollider2D : Component
    {
        private float _circleRadius;
        private bool _isSearchCollision;

        public CircleCollider2D(GameObject gameObject, float radius, bool isSearchCollision = false) : base(gameObject)
        {
            _circleRadius = radius;
            _isSearchCollision = isSearchCollision;

            CollisionHandler.Instance.AddCircleCollider2D(this);
        }

        public override void Update()
        {
            if (Settings.IsDrawColliders)
            {
                DX2D.Instance.RenderTarget.DrawEllipse(
                    new Ellipse(transform.GetRawVector2(), _circleRadius, _circleRadius),
                    DX2D.Instance.RedBrush);
            }

            if (_isSearchCollision)
            {
                CollisionHandler.Instance.CheckCollision(this);
            }
        }

        public Circle2D GetCircle2D()
        {
            return new Circle2D(transform.position.X, transform.position.Y, _circleRadius);
        }

        public override Vector3 GetIndividualPosition()
        {
            Circle2D circle2D = GetCircle2D();
            Vector3 individualPosition = new Vector3(circle2D.x, circle2D.y, 0);

            return individualPosition;
        }
    }

    public class Circle2D
    {
        public float x;
        public float y;
        public float r;

        public Circle2D(float x1, float y1, float radius)
        {
            x = x1;
            y = y1;
            r = radius;
        }
    }
}
