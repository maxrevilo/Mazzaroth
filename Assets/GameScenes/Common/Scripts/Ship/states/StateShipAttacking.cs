using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipAttacking : StateBehaviour {

        private Ship shipState;

        void OnEnable () {
            shipState.DetectionArea.gameObject.SetActive(true);
        }

        // Called when the state is disabled
        void OnDisable () {
            shipState.DetectionArea.gameObject.SetActive(false);
        }

        void Awake() {
            shipState = GetComponent<Ship>();
        }

        void Update() {
			shipState.Fire(shipState.ShipControl.EnemyOnLock.transform, null);
        }

        void FixedUpdate() {
			if (!shipState.ShipControl.EnemyOnLock.isAlive()) {
				GetComponent<Blackboard>().SendEvent(843881883); //EnemyDied
                return;
            }

			Vector3 enemyPosition = shipState.ShipControl.EnemyOnLock.transform.position;
			shipState.MovementEngine.HeadTowardPosition(enemyPosition);
            shipState.MovementEngine.MoveForward();

			Debug.DrawLine(transform.position, enemyPosition, Color.red);
        }
    }
}

