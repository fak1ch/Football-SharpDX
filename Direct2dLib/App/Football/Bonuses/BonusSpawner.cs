using Direct2dLib.App.CustomUnity;
using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Scenes;
using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX;
using System;
using System.Collections.Generic;

namespace Direct2dLib.App.Football.Bonuses
{
    public class BonusSpawner : Component
    {
        private Ball _ball;

        private float _timeBetweenWaves = 3;
        private float _timeBetweenWavesTemp;

        private List<Bonus> _bonuses;
         
        public BonusSpawner(GameObject go, Ball ball) : base(go)
        {
            _bonuses = new List<Bonus>();
            _ball = ball;
        }

        public override void Start()
        {
            _timeBetweenWavesTemp = _timeBetweenWaves;

            _bonuses.Add(CreateBonusMaxSpeed());
            _bonuses.Add(CreateBonusMaxSpeed());
            _bonuses.Add(CreateBonusMaxSpeed());
            _bonuses.Add(CreateBonusMaxSpeed());
            _bonuses.Add(CreateBonusSpeedForPunch());
            _bonuses.Add(CreateBonusSpeedForPunch());
            _bonuses.Add(CreateBonusSpeedForPunch());
            _bonuses.Add(CreateBonusSpeedForPunch());
            _bonuses.Add(CreateBonusSpeedPlayer());
            _bonuses.Add(CreateBonusSpeedPlayer());
            _bonuses.Add(CreateBonusSpeedPlayer());
            _bonuses.Add(CreateBonusSpeedPlayer());
        }

        public override void Update()
        {
            if (!NetworkController.IsServer) return;

            _timeBetweenWavesTemp -= 0.0166666667f;
            if (_timeBetweenWavesTemp <= 0)
            {
                _timeBetweenWavesTemp = _timeBetweenWaves;
                SpawnBonus();
            }
        }

        private void SpawnBonus()
        {
            Random random = new Random();

            int index = random.Next(0, _bonuses.Count);

            Vector3 maxPosition = new Vector3(
                DX2D.Instance.RenderTarget.Size.Width,
                DX2D.Instance.RenderTarget.Size.Height, 0);
            maxPosition -= new Vector3(200, 200, 0);

            Vector3 newPosition = new Vector3( 
                random.NextFloat(200, maxPosition.X),
                random.NextFloat(200, maxPosition.Y),
                0);

            _bonuses[index].transform.position = newPosition;
            _bonuses[index].SetActiveBonus(true);
        }

        private BonusMaxSpeed CreateBonusMaxSpeed()
        {
            GameObject gm1 = new GameObject();
            SpriteRenderer sp1 = gm1.AddComponent(new SpriteRenderer(gm1,
                DX2D.Instance.LoadBitmap("fireBall.png"),
                75, 75));
            BoxCollider2D bc1 = gm1.AddComponent(
                new BoxCollider2D(gm1, 60, 60, new Vector2(0, 0), true));
            SceneManager.Instance.ActiveScene.Instantiate(gm1);
            return gm1.AddComponent(new BonusMaxSpeed(gm1, _ball));
        }

        private BonusSpeedForPunch CreateBonusSpeedForPunch()
        {
            GameObject gm1 = new GameObject();
            SpriteRenderer sp1 = gm1.AddComponent(new SpriteRenderer(gm1,
                DX2D.Instance.LoadBitmap("foothit.png"),
                75, 75));
            BoxCollider2D bc1 = gm1.AddComponent(
                new BoxCollider2D(gm1, 60, 60, new Vector2(0, 0), true));
            SceneManager.Instance.ActiveScene.Instantiate(gm1);
            return gm1.AddComponent(new BonusSpeedForPunch(gm1, _ball));
        }

        private BonusPlayerSpeed CreateBonusSpeedPlayer()
        {
            GameObject gm1 = new GameObject();
            SpriteRenderer sp1 = gm1.AddComponent(new SpriteRenderer(gm1,
                DX2D.Instance.LoadBitmap("arrows.png"),
                75, 75));
            BoxCollider2D bc1 = gm1.AddComponent(
                new BoxCollider2D(gm1, 60, 60, new Vector2(0, 0), true));
            SceneManager.Instance.ActiveScene.Instantiate(gm1);
            return gm1.AddComponent(new BonusPlayerSpeed(gm1));
        }

        public List<BonusData> GetBonusDataList()
        {
            List<BonusData> list = new List<BonusData>();

            foreach (var bonus in _bonuses)
            {
                list.Add(new BonusData
                {
                    position = bonus.transform.position,
                    isActive = bonus.IsActive,
                });
            }

            return list;
        }

        public void SetBonusData(List<BonusData> bonusDatas)
        {
            for (int i = 0; i < _bonuses.Count; i++)
            {
                _bonuses[i].transform.position = bonusDatas[i].position;
                _bonuses[i].SetActiveBonus(bonusDatas[i].isActive);
            }
        }
    }

    public class BonusData
    {
        public Vector3 position;
        public bool isActive;
    }
}
