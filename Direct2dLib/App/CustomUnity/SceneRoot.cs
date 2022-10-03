using Direct2dLib.App;
using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;
using SharpDX;
using System.Collections.Generic;

namespace Direct2dLib
{
    public class SceneRoot
    {
        private static List<GameObject> _gameObjects;

        public SceneRoot()
        {
            _gameObjects = new List<GameObject>();
            Initialize();
        }

        private void Initialize()
        {
            GameObject background = new GameObject();
            background.AddComponent(new SpriteRenderer(background,
                DX2D.Instance.LoadBitmap("bg.jpg"),
                DX2D.Instance.ScreenSize.Right, DX2D.Instance.ScreenSize.Bottom));
            _gameObjects.Add(background);

            GameObject player = new GameObject(new Vector3 (250, 250, 0));
            player.AddComponent(new SpriteRenderer(player,
                DX2D.Instance.LoadBitmap("usa.png"),
                100, 100));
            player.AddComponent(new PlayerMovement(player, 3));
            player.AddComponent(new CircleCollider2D(player, 40, true));
            _gameObjects.Add(player);

            GameObject ball = new GameObject();
            ball.AddComponent(new SpriteRenderer(ball,
                DX2D.Instance.LoadBitmap("ball.png"),
                50, 50));
            ball.AddComponent(new CircleCollider2D(ball, 25, true));
            ball.AddComponent(new Ball(ball, 20, 0.02f, 7));
            _gameObjects.Add(ball);

            Vector3 rightGatePosition = new Vector3(DX2D.Instance.ScreenSize.Right - 40, 372, 0);
            GameObject rightGate = new GameObject(rightGatePosition);
            rightGate.AddComponent(new SpriteRenderer(rightGate,
                DX2D.Instance.LoadBitmap("RightGate.png"),
                100, 250));
            rightGate.AddComponent(new BoxCollider2D(rightGate, 10, 225, new Vector2(-25, 0), true));
            rightGate.AddComponent(new RightGate(rightGate));
            _gameObjects.Add(rightGate);

            Vector3 leftGatePosition = new Vector3(35, 372, 0);
            GameObject leftGate = new GameObject(leftGatePosition);
            leftGate.AddComponent(new SpriteRenderer(leftGate,
                DX2D.Instance.LoadBitmap("LeftGate.png"),
                100, 250));
            leftGate.AddComponent(new BoxCollider2D(leftGate, 10, 225, new Vector2(25, 0), true));
            leftGate.AddComponent(new LeftGate(leftGate));
            _gameObjects.Add(leftGate);

            GameObject collidersVertical = new GameObject(new Vector3(0, DX2D.Instance.ScreenCenter.Y - 20, 0));
            collidersVertical.AddComponent(new VerticalColliders(collidersVertical));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 30, 120)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 30, -120)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(30, 120)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(30, -120)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, DX2D.Instance.ScreenSize.Right, 10,
                new Vector2(DX2D.Instance.ScreenSize.Right / 2, 20 + DX2D.Instance.ScreenSize.Bottom / 2)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, DX2D.Instance.ScreenSize.Right, 10,
                new Vector2(DX2D.Instance.ScreenSize.Right / 2, 20 + DX2D.Instance.ScreenSize.Bottom / -2)));
            _gameObjects.Add(collidersVertical);

            GameObject collidersHorizontal = new GameObject(new Vector3(0, DX2D.Instance.ScreenCenter.Y - 20, 0));
            collidersHorizontal.AddComponent(new HorizontalColliders(collidersHorizontal));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 80, 120)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 80, -120)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(80, 120)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(80, -120)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, DX2D.Instance.ScreenSize.Right,
                new Vector2(0, 0)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, DX2D.Instance.ScreenSize.Right,
                new Vector2(DX2D.Instance.ScreenSize.Right, 0)));
            _gameObjects.Add(collidersHorizontal);

            GameObject gameEndPopUp = new GameObject();
            gameEndPopUp.AddComponent(new GameEndPopUp(gameEndPopUp));
            _gameObjects.Add(gameEndPopUp);

            GameObject match = new GameObject();
            match.AddComponent(new Match(match, rightGate.GetComponent<RightGate>(),
                leftGate.GetComponent<LeftGate>(), gameEndPopUp.GetComponent<GameEndPopUp>(), 
                ball.GetComponent<Ball>()));
            Match matchComponent = match.GetComponent<Match>();
            matchComponent.AddPlayerToTeamByName("Left", player.GetComponent<PlayerMovement>());
            _gameObjects.Add(match);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Start();
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

        public static GameObject Instantiate(GameObject gameObject)
        {
            gameObject.Start();
            _gameObjects.Add(gameObject);

            return gameObject;
        }
    }
} 