using System;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates
{
    public class LeftGate : Component
    {
        public event Action<Type> OnWasScoredGoal;

        public LeftGate(GameObject go) : base(go)
        {
        }

        public override void OnCollision(Component component)
        {
            if (component.gameObject.TryGetComponent(out Ball ball))
            {
                OnWasScoredGoal?.Invoke(typeof(LeftGate));
            }
        }
    }
}
