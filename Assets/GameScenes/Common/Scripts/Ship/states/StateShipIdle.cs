using UnityEngine;
using System.Collections;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class StateShipIdle : StateBehaviour {

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

        void FixedUpdate() {
			ship.MovementEngine.UseBreaks();
			ship.MovementEngine.UseAngularBreaks();
        }
    }
}

