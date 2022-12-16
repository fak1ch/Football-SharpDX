using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Utils;
using SharpDX;
using System;
using System.Diagnostics;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents
{
    public class Ball : Component
    {
        private float _startMaxSpeed = 10;
        private float _fadeSpeed = 0.02f;
        private float _startSpeedForPunch = 4;
        private float _correctAngleValue = 0.25f;

        private Vector2 _velocity;
        private bool _needToSavePosition;
        private Vector3 _lastFramePosition;

        private Vector3 _startPosition;

        private bool _bonusMaxSpeedIsWork = false;
        private float _bonusMaxSpeedDurationTemp;
        private float _maxSpeed;

        private bool _bonusSpeedForPunchIsWork = false;
        private float _bonusSpeedForPunchDuration;
        private float _speedForPunch;

        public Ball(GameObject go) : base(go)
        {
            _startPosition = transform.position;
            _maxSpeed = _startMaxSpeed;
            _speedForPunch = _startSpeedForPunch;
        }

        public override void Update()
        {
            if (_needToSavePosition)
            {
                _lastFramePosition = transform.position;
            }
            else
            {
                _needToSavePosition = true;
            }

            FadeBallSpeed();

            transform.position.X += _velocity.X;
            transform.position.Y += _velocity.Y;


            Random rnd = new Random();
            if (_velocity.X != 0 && _velocity.Y == 0)
            {
                float value = rnd.NextFloat(-_correctAngleValue, _correctAngleValue);
                _velocity.Y += value;
            }

            if (_velocity.Y != 0 && _velocity.X == 0)
            {
                float value = rnd.NextFloat(-_correctAngleValue, _correctAngleValue);
                _velocity.X += value;
            }

            if (_bonusMaxSpeedIsWork)
            {
                _bonusMaxSpeedDurationTemp -= 0.0166666667f;
                if (_bonusMaxSpeedDurationTemp <= 0)
                {
                    _bonusMaxSpeedIsWork = false;
                    _maxSpeed = _startMaxSpeed;
                }
            }

            if (_bonusSpeedForPunchIsWork)
            {
                _bonusSpeedForPunchDuration -= 0.0166666667f;
                if (_bonusSpeedForPunchDuration <= 0)
                {
                    _bonusSpeedForPunchIsWork = false;
                    _speedForPunch = _startSpeedForPunch;
                }
            }
        }

        public override void OnCollision(Component component)
        {
            if(component.gameObject.TryGetComponent(out Player player))
            {
                Vector3 directionVector = transform.position - player.transform.position;
                directionVector.Normalize();

                Vector2 newVelocity = new Vector2(directionVector.X, directionVector.Y);

                newVelocity *= _speedForPunch;
                newVelocity += _velocity;

                newVelocity.X = MathUtils.Clamp(newVelocity.X, -_maxSpeed, _maxSpeed);
                newVelocity.Y = MathUtils.Clamp(newVelocity.Y, -_maxSpeed, _maxSpeed);

                _velocity = newVelocity;
            }

            if (component.gameObject.TryGetComponent(out VerticalColliders verticalColliders))
            {
                _needToSavePosition = false;
                transform.position = _lastFramePosition;
                _velocity.Y *= -1;
            }
            else
            if (component.gameObject.TryGetComponent(out HorizontalColliders horizontalColliders))
            {
                _needToSavePosition = false;
                transform.position = _lastFramePosition;
                _velocity.X *= -1;
            }
        }

        private void FadeBallSpeed()
        {
            _velocity.X = MathUtil.Lerp(_velocity.X, 0, _fadeSpeed);
            _velocity.Y = MathUtil.Lerp(_velocity.Y, 0, _fadeSpeed);
        }

        public void IncreaseMaxSpeedBonus(float addMaxSpeedValue, float bonusDuration)
        {
            if (_bonusMaxSpeedIsWork) return;
            _bonusMaxSpeedIsWork = true;

            _maxSpeed += addMaxSpeedValue;
            _bonusMaxSpeedDurationTemp = bonusDuration;
        }

        public void IncreaseSpeedForPunchBonus(float addSpeedForPunchValue, float bonusDuration)
        {
            if (_bonusSpeedForPunchIsWork) return;
            _bonusSpeedForPunchIsWork = true;

            _speedForPunch += addSpeedForPunchValue;
            _bonusSpeedForPunchDuration = bonusDuration;
        }

        public void ReturnToStartPosition()
        {
            transform.position = _startPosition;
            _velocity = Vector2.Zero;
        }
    }
}
