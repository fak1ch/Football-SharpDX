using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components;
using SharpDX;

namespace Direct2dLib.App.CustomUnity.Scenes
{
    public class MatchScene : Scene
    {
        public override void Initialize()
        {
            #region Backround

            GameObject background = new GameObject();
            background.AddComponent(new SpriteRenderer(background,
                DX2D.Instance.LoadBitmap("bg.jpg"),
                DX2D.Instance.ScreenSize.Right, DX2D.Instance.ScreenSize.Bottom));
            _gameObjects.Add(background);

            #endregion

            #region Players

            Vector3 leftPlayer1Position = DX2D.Instance.ScreenCenter;
            leftPlayer1Position.X -= 100;
            GameObject leftPlayer1 = new GameObject(leftPlayer1Position);
            leftPlayer1.AddComponent(new SpriteRenderer(leftPlayer1,
                DX2D.Instance.LoadBitmap("usa.png"),
                100, 100));
            leftPlayer1.AddComponent(new CircleCollider2D(leftPlayer1, 40, true));
            leftPlayer1.AddComponent(new Player(leftPlayer1, 0));
            _gameObjects.Add(leftPlayer1);

            Vector3 leftPlayer2Position = new Vector3(200, DX2D.Instance.ScreenCenter.Y, 0);
            GameObject leftPlayer2 = new GameObject(leftPlayer2Position);
            leftPlayer2.AddComponent(new SpriteRenderer(leftPlayer2,
                DX2D.Instance.LoadBitmap("usa.png"),
                100, 100));
            leftPlayer2.AddComponent(new CircleCollider2D(leftPlayer2, 40, true));
            leftPlayer2.AddComponent(new Player(leftPlayer2, 1));
            _gameObjects.Add(leftPlayer2);

            Vector3 rightPlayer1Position = DX2D.Instance.ScreenCenter;
            rightPlayer1Position.X += 100;
            GameObject rightPlayer1 = new GameObject(rightPlayer1Position);
            rightPlayer1.AddComponent(new SpriteRenderer(rightPlayer1,
                DX2D.Instance.LoadBitmap("belarus.png"),
                95, 95));
            rightPlayer1.AddComponent(new CircleCollider2D(rightPlayer1, 40, true));
            rightPlayer1.AddComponent(new Player(rightPlayer1, 2));
            _gameObjects.Add(rightPlayer1);

            Vector3 rightPlayer2Position = new Vector3(DX2D.Instance.ScreenSize.Right - 200, DX2D.Instance.ScreenCenter.Y, 0);
            GameObject rightPlayer2 = new GameObject(rightPlayer2Position);
            rightPlayer2.AddComponent(new SpriteRenderer(rightPlayer2,
                DX2D.Instance.LoadBitmap("belarus.png"),
                95, 95));
            rightPlayer2.AddComponent(new CircleCollider2D(rightPlayer2, 40, true));
            rightPlayer2.AddComponent(new Player(rightPlayer2, 3));
            _gameObjects.Add(rightPlayer2);

            #endregion

            #region Ball

            GameObject ball = new GameObject();
            ball.AddComponent(new SpriteRenderer(ball,
                DX2D.Instance.LoadBitmap("ball.png"),
                50, 50));
            ball.AddComponent(new CircleCollider2D(ball, 25, true));
            ball.AddComponent(new Ball(ball, 20, 0.02f, 7));
            _gameObjects.Add(ball);

            #endregion

            #region Gates

            Vector3 rightGatePosition = new Vector3(DX2D.Instance.ScreenSize.Right - 40, DX2D.Instance.ScreenSize.Bottom / 2, 0);
            GameObject rightGate = new GameObject(rightGatePosition);
            rightGate.AddComponent(new SpriteRenderer(rightGate,
                DX2D.Instance.LoadBitmap("RightGate.png"),
                100, 250));
            rightGate.AddComponent(new BoxCollider2D(rightGate, 10, 225, new Vector2(-25, 0), true));
            rightGate.AddComponent(new RightGate(rightGate));
            _gameObjects.Add(rightGate);

            Vector3 leftGatePosition = new Vector3(35, DX2D.Instance.ScreenSize.Bottom / 2, 0);
            GameObject leftGate = new GameObject(leftGatePosition);
            leftGate.AddComponent(new SpriteRenderer(leftGate,
                DX2D.Instance.LoadBitmap("LeftGate.png"),
                100, 250));
            leftGate.AddComponent(new BoxCollider2D(leftGate, 10, 225, new Vector2(25, 0), true));
            leftGate.AddComponent(new LeftGate(leftGate));
            _gameObjects.Add(leftGate);

            #endregion

            #region Colliders

            GameObject collidersVertical = new GameObject(new Vector3(0, DX2D.Instance.ScreenCenter.Y - 20, 0));
            collidersVertical.AddComponent(new VerticalColliders(collidersVertical));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 30, 140)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 30, -100)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(30, 140)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, 85, 10, new Vector2(30, -100)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, DX2D.Instance.ScreenSize.Right, 10,
                new Vector2(DX2D.Instance.ScreenSize.Right / 2, 20 + DX2D.Instance.ScreenSize.Bottom / 2)));
            collidersVertical.AddComponent(new BoxCollider2D(collidersVertical, DX2D.Instance.ScreenSize.Right, 10,
                new Vector2(DX2D.Instance.ScreenSize.Right / 2, 20 + DX2D.Instance.ScreenSize.Bottom / -2)));
            _gameObjects.Add(collidersVertical);

            GameObject collidersHorizontal = new GameObject(new Vector3(0, DX2D.Instance.ScreenCenter.Y - 20, 0));
            collidersHorizontal.AddComponent(new HorizontalColliders(collidersHorizontal));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 80, 140)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(DX2D.Instance.ScreenSize.Right - 80, -100)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(80, 140)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, 10, new Vector2(80, -100)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, DX2D.Instance.ScreenSize.Right,
                new Vector2(0, 0)));
            collidersHorizontal.AddComponent(new BoxCollider2D(collidersHorizontal, 10, DX2D.Instance.ScreenSize.Right,
                new Vector2(DX2D.Instance.ScreenSize.Right, 0)));
            _gameObjects.Add(collidersHorizontal);

            #endregion

            #region UI

            GameObject gameEndPopUp = new GameObject();
            gameEndPopUp.AddComponent(new GameEndPopUp(gameEndPopUp));
            _gameObjects.Add(gameEndPopUp);

            GameObject score = new GameObject();
            score.AddComponent(new Score(score));
            _gameObjects.Add(score);

            GameObject timer = new GameObject();
            timer.AddComponent(new Timer(timer));
            _gameObjects.Add(timer);

            #endregion

            #region Match

            MatchData matchData = new MatchData
            {
                ball = ball.GetComponent<Ball>(),
                gameEndPopUp = gameEndPopUp.GetComponent<GameEndPopUp>(),
                leftGate = leftGate.GetComponent<LeftGate>(),
                rightGate = rightGate.GetComponent<RightGate>(),
                score = score.GetComponent<Score>(),
                timer = timer.GetComponent<Timer>(),
            };

            GameObject match = new GameObject();
            Match matchComponent = match.AddComponent(new Match(match, matchData));
            matchComponent.AddPlayerToTeamByName("Left", leftPlayer1.GetComponent<Player>());
            matchComponent.AddPlayerToTeamByName("Left", leftPlayer2.GetComponent<Player>());
            matchComponent.AddPlayerToTeamByName("Right", rightPlayer1.GetComponent<Player>());
            matchComponent.AddPlayerToTeamByName("Right", rightPlayer2.GetComponent<Player>());
            _gameObjects.Add(match);

            #endregion
        }
    }
}
