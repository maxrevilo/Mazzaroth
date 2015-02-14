﻿using UnityEngine;
using System;

namespace Mazzaroth {
    public class BattleScene: SceneController{
        public Player[] Players;

        public BattleScene() {
        }

        public Player AddNewPlayer(Color color) {
            return null;
        }

        public Player MainPlayer;

        void Awake() {
            foreach(Player player in Players) {
                player.Initialize();
                if (player.IsTheMainPlayer)
                    MainPlayer = player;
            }
        }
    }
}

