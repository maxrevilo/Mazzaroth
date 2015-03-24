using System;

namespace Mazzaroth {
    [Serializable]
    public class Army {

		[NonSerialized] public Player Player;
        public ShipsGroup[] Groups;

        public void Initialize(Player player) {
            Player = player;
            foreach(ShipsGroup group in Groups) {
                group.Initialize(this);
            }
        }
    }
}

