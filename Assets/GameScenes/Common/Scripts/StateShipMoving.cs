using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
    public class StateShipMoving : StateBehaviour {

		protected ShipState shipState;

        void OnEnable () {
        }

        // Called when the state is disabled
        void OnDisable () {
        }

        void Awake() {
            shipState = GetComponent<ShipState>();
        }

        void FixedUpdate() {
            const float MIN_DISTANCE_TO_DESTINY = 0.5f;
            float sqrSistanceToDestiny = Vector3.SqrMagnitude(this.transform.position - shipState.DestinyLocation);
            if (sqrSistanceToDestiny < Mathf.Pow(MIN_DISTANCE_TO_DESTINY, 2)) {
                blackboard.SendEvent(1202858853); //OnDestiny
                return;
            }

            shipState.HeadTowardPosition(shipState.DestinyLocation);
            shipState.MoveForwardToPosition(shipState.DestinyLocation);
        }
    }
}

