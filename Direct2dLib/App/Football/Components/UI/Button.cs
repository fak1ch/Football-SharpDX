using Direct2dLib.App.CustomUnity.Utils;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class Button : Component
    {
        public event Action OnClick;

        private RectangleF _background;

        public Button(GameObject go, RectangleF bg) : base(go)
        {
            _background = bg;
        }

        public override void Update()
        {
            if (Input.Instance.GetMouseClicked())
            {
                if (CollisionUtils.PointIntersectsRectangle(Cursor.Position, _background))
                {
                    OnClick?.Invoke();
                }
            }
        }
    }
}
