using UnityEngine;
using System;
using System.Collections.Generic;
using Mazzaroth.Ships;

namespace Mazzaroth {
    [Serializable]
    public class ShipsGroup {
		public enum ShipFormations {
			Bird,
			Line,
			Square,
			Circle
		}

		[NonSerialized] public Army Army;

        public Ship[] Ships;

        public bool Selected { get; set; }

		public ShipFormations Formation = ShipFormations.Line;

        public void Initialize(Army army) {
            Army = army;
            Selected = false;

            foreach(Ship ship in Ships) {
                ship.Group = this;
				ship.OnEnemyDetected += enemyDetected;
            }
        }

        public void MoveOrder(Vector3 destiny) {
			Ship[] ships = aliveShips();
			sendOrderToShips(ships, destiny, false);
        }
			
		public void AggressiveMoveOrder(Vector3 destiny) {
			Ship[] ships = aliveShips();
			sendOrderToShips(ships, destiny, true);
		}

        public void AtackOrder(Ship enemyShip) {
            for (int i = 0; i < Ships.Length; i++) {
                Ship ship = Ships [i];
				ship.ShipControl.AttackOrder(enemyShip);
            }
        }

        public Vector3 Position() {
            Vector3 position = Vector3.zero;

            for (int i = 0; i < Ships.Length; i++) {
                position += Ships[i].transform.position;
            }

            return position / Ships.Length;
        }

		private void enemyDetected(Ship enemy) {
			for (int i = 0; i < Ships.Length; i++) {
				Ship ship = Ships [i];
				ship.ShipControl.ShipDetected(enemy);
			}
		}

		private Vector3[] sendOrderToShips(Ship[] ships, Vector3 destiny, bool aggresive) {
			Vector3[] positions = new Vector3[ships.Length];

			switch (Formation) {
				case ShipFormations.Line:
					float distance = 7f;
					float begining = -(ships.Length- 1f) * distance * 0.5f ;

					Vector3 forward = (destiny - Position()).normalized;
					Vector3 right =Vector3.Cross(forward, Vector3.up);

					for (int i = 0; i < ships.Length; i++) {
						Ship ship = ships[i];

						Vector3 shipDestiny = destiny + right * (begining + distance * i);
						positions[i] = shipDestiny;

						if(aggresive) ship.ShipControl.AggressiveMoveOrder(shipDestiny);
						else ship.ShipControl.MoveOrder(shipDestiny);
					}

					break;
				case ShipFormations.Bird:
				case ShipFormations.Circle:
				case ShipFormations.Square:
				default:
					throw new Exception("Formations not implemented");
			}

			return positions;
		}

        private Ship[] aliveShips() {
            return Array.FindAll(Ships, ship => ship.IsAlive());
        }
    }
}

