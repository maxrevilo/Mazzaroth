using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mazzaroth {
    [Serializable]
    public class ShipsGroup {
        public Army Army { get; set; }

        public ShipState[] Ships;

        public bool Selected { get; set; }

        public void Initialize(Army army) {
            Army = army;
            Selected = false;

            foreach(ShipState ship in Ships) {
                ship.Group = this;
            }
        }

        public void MoveOrder(Vector3 destiny) {
            ShipState[] ships = aliveShips();
            float distance = 7f;
            float begining = -(ships.Length- 1f) * distance * 0.5f ;

            Vector3 forward = (destiny - Position()).normalized;
            Vector3 right =Vector3.Cross(forward, Vector3.up);

            for (int i = 0; i < ships.Length; i++) {
                ShipState ship = ships[i];

                Vector3 shipDestiny = destiny + right * (begining + distance * i);

                ship.MoveOrder(shipDestiny);
            }
        }

        public void AtackOrder(ShipState enemyShip) {
            for (int i = 0; i < Ships.Length; i++) {
                ShipState ship = Ships [i];
                ship.AttackOrder(enemyShip);
            }
        }

        public Vector3 Position() {
            Vector3 position = Vector3.zero;

            for (int i = 0; i < Ships.Length; i++) {
                position += Ships[i].transform.position;
            }

            return position / Ships.Length;
        }

        private ShipState[] aliveShips() {
            return Array.FindAll(Ships, ship => ship.isAlive());
        }
    }
}

