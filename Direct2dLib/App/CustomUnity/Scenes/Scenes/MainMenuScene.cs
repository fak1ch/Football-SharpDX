using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;

namespace Direct2dLib.App.CustomUnity.Scenes
{
    public class MainMenuScene : Scene
    {
        public override void Initialize()
        {
            #region UI

            GameObject mainMenuPopUp = new GameObject();
            mainMenuPopUp.AddComponent(new MainMenuPopUp(mainMenuPopUp));
            Instantiate(mainMenuPopUp);

            #endregion
        }
    }
}
