using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using SharpDX;
using SharpDX.DirectInput;
using System.ComponentModel;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class PlayerMovement : Component
    {
        private float _speed = 3;
        private Vector3 _lastFramePosition;
        private Player _player;

        public PlayerMovement(GameObject go, Player player) : base(go)
        {
            _player = player;
        }

        public override void Update()
        {
            if (_player.GameOnPause) return;

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

            if (component.gameObject.TryGetComponent(out Player player))
            {
                transform.position = _lastFramePosition;
            }
        }
    }
}
