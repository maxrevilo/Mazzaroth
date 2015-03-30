using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipIdle : StateBehaviour {

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

        void FixedUpdate() {
			shipState.MovementEngine.UseBreaks();
			shipState.MovementEngine.UseAngularBreaks();
        }
    }
}

