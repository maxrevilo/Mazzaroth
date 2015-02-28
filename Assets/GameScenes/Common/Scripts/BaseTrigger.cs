using UnityEngine;

namespace Mazzaroth {
	public class BaseTrigger : BaseMonoBehaviour {
		public GameObject[] ObjectsToEnable;
		public GameObject[] ObjectsToDisable;

		public MonoBehaviour[] BehavioursToEnable;
		public MonoBehaviour[] BehavioursToDisable;

		public bool SelfDeactivateOnTrigger = true;

		public void Trigger() {
			foreach (GameObject obj in ObjectsToEnable) {
				obj.SetActive(true);
			}
			foreach (GameObject obj in ObjectsToDisable) {
				obj.SetActive(false);
			}

			foreach (MonoBehaviour behaviour in BehavioursToEnable) {
				behaviour.enabled = true;
			}
			foreach (MonoBehaviour behaviour in BehavioursToDisable) {
				behaviour.enabled = false;
			}

			if(SelfDeactivateOnTrigger) gameObject.DestroyAPS();
		}
	}
}

