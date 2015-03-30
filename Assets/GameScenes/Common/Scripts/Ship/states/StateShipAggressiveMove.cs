using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
	public class StateShipAggressiveMove: StateShipMoving {

		void OnEnable() {
			ship.DetectionArea.gameObject.SetActive(true);
		}

		void OnDisable() {
			ship.DetectionArea.gameObject.SetActive(false);
		}
	}
}

