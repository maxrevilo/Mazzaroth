using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
    public class StateShipAttacking : StateBehaviour {

        private ShipState shipState;

        void Start() {
            shipState = GetComponent<ShipState>();
        }

        void Update() {
            Debug.Log("shipState -> " + shipState);
            Debug.Log("shipState.EnemyOnLock -> " + (shipState.EnemyOnLock == null));
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

