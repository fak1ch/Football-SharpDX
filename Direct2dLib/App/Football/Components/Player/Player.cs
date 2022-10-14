﻿using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players
{
    public class Player : Component
    {
        private Vector3 _startPosition;

        public bool GameOnPause { get; set; }

        public Player(GameObject go, int playerIndex) : base(go)
        {
            if (playerIndex == NetworkController.PlayerIndex)
            {
                gameObject.AddComponent(new PlayerMovement(gameObject, this));

                if (NetworkController.IsServer)
                {

                }
            }
        }

        public override void Start()
        {
            _startPosition = transform.position;
        }

        public void ReturnToStartPosition()
        {
            transform.position = _startPosition;
        }
    }
}
