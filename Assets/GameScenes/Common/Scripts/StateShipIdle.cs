using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
    public class StateShipIdle : StateBehaviour {

        private ShipState shipState;

        void Start() {
            shipState = GetComponent<ShipState>();
        }

        void FixedUpdate() {
            shipState.UseBreaks();
            shipState.UseAngularBreaks();
        }
    }
}

