using SharpDX;
using SharpDX.Mathematics.Interop;

namespace Direct2dLib.App.CustomUnity
{
    public class Transform : Component
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 eulerAngles = Vector3.Zero;

        public Transform(GameObject gameObject) : base(gameObject)
        {

        }

        public RawVector2 GetRawVector2()
        {
            return new RawVector2(position.X, position.Y);
        }
    }
}
