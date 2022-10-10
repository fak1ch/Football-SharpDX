
namespace Direct2dLib.App.CustomUnity.Scenes
{
    public class SceneManager
    {
        private static SceneManager _instance;
        public static SceneManager Instance => _instance;

        private Scene _activeScene;

        public Scene ActiveScene => _activeScene;

        public SceneManager()
        {
            _instance = this;
            LoadScene<MainMenuScene>();
        }

        public T LoadScene<T>() where T : Scene, new()
        {
            _activeScene = new T();
            return (T)_activeScene;
        }
    }
}
