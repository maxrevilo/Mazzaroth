using UnityEngine;
using System;

namespace Mazzaroth {
    [Serializable]
    public class Player {

        public Army Army;
        public Color Color = Color.grey;
        public String Name;

        public bool IsTheMainPlayer;

        public Player() {
            if (Army == null)
                Army = new Army(){ Player = this };
        }

        public void Initialize() {
            if (Army != null) Army.Initialize(this);
        }

        public bool IsEnemy(Player player) {
            return player != this;
        }
    }
}

