using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class Score : Component
    {
        private int _pointsForWin = 3;

        public int LeftTeamPoint { get; set; } = 0;
        public int RightTeamPoints { get; set; } = 0;

        public bool WinLeftTeam => LeftTeamPoint == _pointsForWin;
        public bool WinRightTeam => RightTeamPoints == _pointsForWin;

        public Score(GameObject go) : base(go)
        {
        }

        public override void Update()
        {
            UpdatePointsText();
        }

        private void UpdatePointsText()
        {
            Vector3 screenCenter = DX2D.Instance.ScreenCenter;
            RectangleF textRectangle = new RectangleF(screenCenter.X - 200, 0, 400, 100);
            DX2D.Instance.RenderTarget.DrawText($"{LeftTeamPoint} : {RightTeamPoints}", DX2D.Instance.TextFormatMessage, textRectangle, DX2D.Instance.WhiteBrush);
        }

        public void RestartScore()
        {
            LeftTeamPoint = 0;
            RightTeamPoints = 0;
        }
    }
}
