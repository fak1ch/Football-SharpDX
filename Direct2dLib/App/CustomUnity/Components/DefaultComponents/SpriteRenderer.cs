using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class SpriteRenderer : Component
    {
        private Bitmap _bitmap;
        private float _width;
        private float _height;

        public SpriteRenderer(GameObject gameObject, Bitmap bitmap, float width, float height) : base(gameObject)
        {
            _bitmap = bitmap;
            _width = width;
            _height = height;
        }

        public override void Update()
        {
            DX2D.Instance.RenderTarget.DrawBitmap(_bitmap, GetBitmapRectangleF(), 1, BitmapInterpolationMode.NearestNeighbor);
        }

        private RectangleF GetBitmapRectangleF()
        {
            return new RectangleF
            {
                Left = transform.position.X - _width / 2,
                Top = transform.position.Y - _height / 2,
                Right = transform.position.X + _width / 2,
                Bottom = transform.position.Y + _height / 2,
            };
        }
    }
}
