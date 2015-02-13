using System;

namespace Mazzaroth {
    [Serializable]
    public class ShipsGroup {
        public Army Army { get; set; }

        public ShipState[] Ships;

        public void Initialize(Army army) {
            Army = army;

            foreach(ShipState ship in Ships) {
                ship.Group = this;
            }
        }
    }
}

