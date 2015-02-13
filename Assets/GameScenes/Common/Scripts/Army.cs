using System;

namespace Mazzaroth {
    [Serializable]
    public class Army {

        public Player Player { get; set; }
        public ShipsGroup[] Groups;

        public void Initialize(Player player) {
            Player = player;
            foreach(ShipsGroup group in Groups) {
                group.Initialize(this);
            }
        }
    }
}

