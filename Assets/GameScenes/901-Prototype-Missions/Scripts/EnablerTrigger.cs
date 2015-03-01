using System;
using UnityEngine;

namespace Mazzaroth {
	public class EnablerTrigger : BaseTrigger {
		public GameObject OnlyTriggerWith;

		void OnCollisionEnter(Collision collision) {
			if (OnlyTriggerWith == null || collision.gameObject == OnlyTriggerWith) {
				Trigger();
			}
		}

		void OnTriggerEnter(Collider collider) {
			if (OnlyTriggerWith == null || collider.gameObject == OnlyTriggerWith) {
				Trigger();
			}
		}

	}
}

