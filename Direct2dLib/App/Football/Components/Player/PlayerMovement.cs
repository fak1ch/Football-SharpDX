using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX;
using SharpDX.DirectInput;
using System.ComponentModel;

namespace Direct2dLib.App.CustomUnity.Components
{
    public class PlayerMovement : Component
    {
        private float _startSpeed = 3;
        private Vector3 _lastFramePosition;
        private Player _player;

        private bool _isSpeedBonusWork;
        private float _timeSpeedBonusDurationTemp;

        private float _speedTemp;

        public PlayerMovement(GameObject go, Player player) : base(go)
        {
            _player = player;
            _speedTemp = _startSpeed;
        }

        public override void Update()
        {
            if (_player.GameOnPause) return;

            Vector3 input = new Vector3();
            _lastFramePosition = transform.position;

            if (Input.Instance.GetKey(Key.A))
            {
                input.X = -_speedTemp;
            }

            if (Input.Instance.GetKey(Key.D))
            {
                input.X = +_speedTemp;
            }

            if (Input.Instance.GetKey(Key.W))
            {
                input.Y = -_speedTemp;
            }

            if (Input.Instance.GetKey(Key.S))
            {
                input.Y = +_speedTemp;
            }

            input.Normalize();
            transform.position += input * _speedTemp;

            if (NetworkController.IsServer)
            {
                NetworkController.Server.StartWriteAndReadMatchInNewThread();
            }
            else
            {
                NetworkController.Client.StartWriteAndReadMatchInNewThread();
            }

            if (_isSpeedBonusWork)
            {
                _timeSpeedBonusDurationTemp -= 0.0166666667f;
                if (_timeSpeedBonusDurationTemp <= 0)
                {
                    _isSpeedBonusWork = false;
                    _speedTemp = _startSpeed;
                }
            }
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

        public void SetSpeedTemp(float addSpeedValue, float bonusDuration)
        {
            if (_isSpeedBonusWork) return;

            _isSpeedBonusWork = true;
            _timeSpeedBonusDurationTemp = bonusDuration;
            _speedTemp += addSpeedValue;
        }
    }
}
