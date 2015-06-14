using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipAttacking : StateBehaviour {

        private Ship ship;

        void OnEnable () {
            ship.DetectionArea.gameObject.SetActive(true);
        }

        // Called when the state is disabled
        void OnDisable () {
            ship.DetectionArea.gameObject.SetActive(false);
        }

        void Awake() {
            ship = GetComponent<Ship>();
        }

        void Update() {
			ship.ShipWeapons.Fire(ship.ShipControl.EnemyOnLock, null);
        }

        void FixedUpdate() {
			if (!ship.ShipControl.EnemyOnLock.IsAlive()) {
				ship.ShipControl.EnemyDied();
                return;
            }

			ship.ShipControl.ChaceEnemy();
        }
    }
}

