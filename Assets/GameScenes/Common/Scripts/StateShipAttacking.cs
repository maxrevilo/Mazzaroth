using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
    public class StateShipAttacking : StateBehaviour {

        private ShipState shipState;

        void OnEnable () {
            shipState.DetectionArea.gameObject.SetActive(true);
        }

        // Called when the state is disabled
        void OnDisable () {
            shipState.DetectionArea.gameObject.SetActive(false);
        }

        void Awake() {
            shipState = GetComponent<ShipState>();
        }

        void Update() {
            shipState.Fire(shipState.EnemyOnLock.transform, null);
        }

        void FixedUpdate() {
            if (!shipState.EnemyOnLock.isAlive()) {
                blackboard.SendEvent(843881883); //EnemyDied
                return;
            }

            Vector3 enemyPosition = shipState.EnemyOnLock.transform.position;
            shipState.HeadTowardPosition(enemyPosition);
            shipState.MoveForward();
        }
    }
}

