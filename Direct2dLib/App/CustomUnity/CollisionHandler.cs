using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity
{
    public class CollisionHandler
    {
        private static CollisionHandler _collisionHandler;
        public static CollisionHandler Instance => _collisionHandler;

        private List<BoxCollider2D> _boxColliders;
        private List<CircleCollider2D> _circleColliders;

        public CollisionHandler()
        {
            _boxColliders = new List<BoxCollider2D>();
            _circleColliders = new List<CircleCollider2D>();

            _collisionHandler = this;
        }

        public void AddBoxCollider2D(BoxCollider2D boxCollider2D)
        {
            _boxColliders.Add(boxCollider2D);
        }

        public void AddCircleCollider2D(CircleCollider2D circleCollider2D)
        {
            _circleColliders.Add(circleCollider2D);
        }

        public void CheckCollision(BoxCollider2D boxCollider2D)
        {
            foreach (var otherBoxCollider in _boxColliders)
            {
                if (otherBoxCollider == boxCollider2D) continue;

                if (boxCollider2D.GetRectangleF().Intersects(otherBoxCollider.GetRectangleF()))
                {
                    boxCollider2D.gameObject.OnCollision(otherBoxCollider);
                }
            }

            foreach (var otherCircleCollider in _circleColliders)
            {
                if (CollisionUtils.CircleIntersectsRectangle(otherCircleCollider.GetCircle2D(),
                    boxCollider2D.GetRectangleF()))
                {
                    boxCollider2D.gameObject.OnCollision(otherCircleCollider);
                }
            }
        }

        public void CheckCollision(CircleCollider2D circleCollider2D)
        {
            foreach (var otherBoxCollider in _boxColliders)
            {
                if (CollisionUtils.CircleIntersectsRectangle(circleCollider2D.GetCircle2D(),
                    otherBoxCollider.GetRectangleF()))
                {
                    circleCollider2D.gameObject.OnCollision(otherBoxCollider);
                }
            }

            foreach (var otherCircleCollider in _circleColliders)
            {
                if (otherCircleCollider == circleCollider2D) continue;

                if (CollisionUtils.CircleIntersectsCircle(circleCollider2D.GetCircle2D(),
                    otherCircleCollider.GetCircle2D()))
                {
                    circleCollider2D.gameObject.OnCollision(otherCircleCollider);
                }
            }
        }
    }
}
