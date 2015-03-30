using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipMoving : StateBehaviour {

		protected Ship ship;

        void OnEnable () {
        }

        // Called when the state is disabled
        void OnDisable () {
        }

        void Awake() {
            ship = GetComponent<Ship>();
        }

        void FixedUpdate() {
            const float MIN_DISTANCE_TO_DESTINY = 0.5f;
			float sqrSistanceToDestiny = Vector3.SqrMagnitude(this.transform.position - ship.ShipControl.DestinyLocation);
            if (sqrSistanceToDestiny < Mathf.Pow(MIN_DISTANCE_TO_DESTINY, 2)) {
				ship.ShipControl.OnDestiny();
                return;
            }

			ship.ShipControl.DriveToPosition();
        }
    }
}

