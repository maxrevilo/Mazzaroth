using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
    public class StateShipIdle : StateBehaviour {

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

        void FixedUpdate() {
            shipState.UseBreaks();
            shipState.UseAngularBreaks();
        }
    }
}

