using UnityEngine;
using System.Collections;

namespace Mazzaroth {
	public class OnDestroyTrigger : BaseTrigger {
		void Start () {
			(GetComponent<ShipState>() as ShipState).OnShipDestroyed += shipDestroyedListener;
		}

		void shipDestroyedListener(ShipState ship) {
			Trigger();
		}
	}
}
