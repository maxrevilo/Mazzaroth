using UnityEngine;
using System.Collections;
using Mazzaroth.Ships;

namespace Mazzaroth {
	public class OnDestroyTrigger : BaseTrigger {
		void Start () {
			(GetComponent<Ship>() as Ship).OnShipDestroyed += shipDestroyedListener;
		}

		void shipDestroyedListener(Ship ship) {
			Trigger();
		}
	}
}
