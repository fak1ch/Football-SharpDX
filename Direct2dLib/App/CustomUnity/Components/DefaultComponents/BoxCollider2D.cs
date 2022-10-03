using SharpDX;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class BoxCollider2D : Component
    {
        private float _width;
        private float _height;
        private Vector2 _offset;
        private bool _isSearchCollision;

        public BoxCollider2D(GameObject gameObject, float width, float height, Vector2 offset, bool isSearchCollision = false) : base(gameObject)
        {
            _width = width;
            _height = height;
            _offset = offset;
            _isSearchCollision = isSearchCollision;

            CollisionHandler.Instance.AddBoxCollider2D(this);
        }

        public override void Update()
        {
            if (Settings.IsDrawColliders)
            {
                DX2D.Instance.RenderTarget.DrawRectangle(
                    GetRectangleF(),
                    DX2D.Instance.RedBrush);
            }

            if (_isSearchCollision)
            {
                CollisionHandler.Instance.CheckCollision(this);
            }
        }

        public RectangleF GetRectangleF()
        {
            return new RectangleF
            {
                Left = (transform.position.X - _width / 2) + _offset.X,
                Top = (transform.position.Y - _height / 2) + _offset.Y,
                Right = (transform.position.X + _width / 2) + _offset.X,
                Bottom = (transform.position.Y + _height / 2) + _offset.Y,
            };
        }

        public override Vector3 GetIndividualPosition()
        {
            RectangleF rectangleF = GetRectangleF();
            Vector3 individualPosition = new Vector3(rectangleF.Right - _width/2, rectangleF.Bottom - _height/2, 0);

            return individualPosition;
        }
    }
}
