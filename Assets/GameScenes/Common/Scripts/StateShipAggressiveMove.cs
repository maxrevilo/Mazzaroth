using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {
	public class StateShipAggressiveMove: StateShipMoving {

		void OnEnable() {
			shipState.DetectionArea.gameObject.SetActive(true);
		}

		void OnDisable() {
			shipState.DetectionArea.gameObject.SetActive(false);
		}
	}
}

