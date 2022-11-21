using System.Collections.Generic;

namespace Direct2dLib.App.CustomUnity.Scenes
{
    public abstract class Scene
    {
        public abstract void Initialize();

        protected static List<GameObject> _gameObjects;

        public Scene()
        {
            _gameObjects = new List<GameObject>();
            Initialize();
            Start();
        }

        public void Start()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].Start();
            }
        }

        public void Update()
        {
            Input.Instance.UpdateKeyboardState();
            Input.Instance.UpdateMouseState();

            DX2D.Instance.Timer();

            DX2D.Instance.RenderTarget.BeginDraw();

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update();
            }

            DX2D.Instance.RenderTarget.EndDraw();
        }

        public GameObject Instantiate(GameObject gameObject)
        {
            gameObject.Start();
            _gameObjects.Add(gameObject);

            return gameObject;
        }
    }
}
