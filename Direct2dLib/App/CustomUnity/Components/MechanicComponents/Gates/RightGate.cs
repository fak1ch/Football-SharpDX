using System;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates
{
    public class RightGate : Component
    {
        public event Action<Type> OnWasScoredGoal;

        public RightGate(GameObject go) : base(go)
        {
        }

        public override void OnCollision(Component component)
        {
            if (component.gameObject.TryGetComponent(out Ball ball))
            {
                OnWasScoredGoal?.Invoke(typeof(RightGate));
            }
        }
    }
}
