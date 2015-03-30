using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipMoving : StateBehaviour {

		protected Ship shipState;

        void OnEnable () {
        }

        // Called when the state is disabled
        void OnDisable () {
        }

        void Awake() {
            shipState = GetComponent<Ship>();
        }

        void FixedUpdate() {
            const float MIN_DISTANCE_TO_DESTINY = 0.5f;
			float sqrSistanceToDestiny = Vector3.SqrMagnitude(this.transform.position - shipState.ShipControl.DestinyLocation);
            if (sqrSistanceToDestiny < Mathf.Pow(MIN_DISTANCE_TO_DESTINY, 2)) {
				GetComponent<Blackboard>().SendEvent(1202858853); //OnDestiny
                return;
            }

			shipState.MovementEngine.HeadTowardPosition(shipState.ShipControl.DestinyLocation);
			shipState.MovementEngine.MoveForwardToPosition(shipState.ShipControl.DestinyLocation);
        }
    }
}

