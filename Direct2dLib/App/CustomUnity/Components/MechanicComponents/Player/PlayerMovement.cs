using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using SharpDX;
using SharpDX.DirectInput;
using System.ComponentModel;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class PlayerMovement : Component
    {
        private float _speed;
        private Vector3 _lastFramePosition;

        private bool _gameOnPause = false;

        public PlayerMovement(GameObject go, float speed) : base(go)
        {
            _speed = speed;
        }

        public override void Update()
        {
            if (_gameOnPause) return;

            Vector3 input = new Vector3();
            _lastFramePosition = transform.position;

            if (Input.Instance.GetKey(Key.A))
            {
                input.X = -_speed;
            }

            if (Input.Instance.GetKey(Key.D))
            {
                input.X = +_speed;
            }

            if (Input.Instance.GetKey(Key.W))
            {
                input.Y = -_speed;
            }

            if (Input.Instance.GetKey(Key.S))
            {
                input.Y = +_speed;
            }

            input.Normalize();
            transform.position += input * _speed;
        }

        public void SetGameOnPause(bool value)
        {
            _gameOnPause = value;
        }

        public override void OnCollision(Component component)
        {
            if (component.gameObject.TryGetComponent(out VerticalColliders verticalColliders))
            {
                transform.position = _lastFramePosition;
            }

            if (component.gameObject.TryGetComponent(out HorizontalColliders horizontalColliders))
            {
                transform.position = _lastFramePosition;
            }
        }
    }
}
