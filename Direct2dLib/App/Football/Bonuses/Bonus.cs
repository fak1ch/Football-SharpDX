using Direct2dLib.App.CustomUnity;
using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;

namespace Direct2dLib.App.Football.Bonuses
{
    public class Bonus : Component
    {
        public bool IsActive { get; private set; }

        public Bonus(GameObject go) : base(go)
        {
            SetActiveBonus(false);
        }

        public override void OnCollision(Component component)
        {
            if (component.gameObject.TryGetComponent(out Player player))
            {
                ActivateBonus(player);
            }
        }

        protected virtual void ActivateBonus(Player player)
        {

        }

        public void SetActiveBonus(bool value)
        {
            IsActive = value;
            gameObject.SetActive(value);
        }
    }
}
