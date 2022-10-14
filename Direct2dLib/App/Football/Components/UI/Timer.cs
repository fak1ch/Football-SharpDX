using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class Timer : Component
    {
        public event Action OnTimeEnd;

        private float _startSeconds = 180;
        private float _currentSeconds;

        public bool GameOnPause { get; set; }

        public Timer(GameObject go) : base(go)
        {

        }

        public override void Update()
        {
            UpdateTimeText();
        }

        private void UpdateTimeText()
        {
            if (!GameOnPause)
            {
                _currentSeconds -= 0.0166666667f;
                if (_currentSeconds <= 0)
                {
                    _currentSeconds = 0;
                    OnTimeEnd?.Invoke();
                }
            }

            int minutes = (int)_currentSeconds / 60;
            int seconds = (int)_currentSeconds - minutes * 60;

            string time = $"{minutes}:{seconds}";

            if (seconds < 10)
            {
                time = $"{minutes}:0{seconds}";
            }

            Vector3 screenCenter = DX2D.Instance.ScreenCenter;
            RectangleF textRectangle = new RectangleF(screenCenter.X - 200, 75, 400, 100);
            DX2D.Instance.RenderTarget.DrawText(time, DX2D.Instance.TextFormatMessageSmall, textRectangle, DX2D.Instance.WhiteBrush);
        }

        public void RestartTimer()
        {
            _currentSeconds = _startSeconds;
        }
    }
}
