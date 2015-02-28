using UnityEngine;

namespace Mazzaroth {
	public class BaseTrigger : BaseMonoBehaviour {
		public GameObject[] ObjectsToEnable;
		public GameObject[] ObjectsToDisable;

		public bool SelfDeactivateOnTrigger = true;

		public void Trigger() {
			foreach (GameObject obj in ObjectsToEnable) {
				obj.SetActive(true);
			}
			foreach (GameObject obj in ObjectsToDisable) {
				obj.SetActive(false);
			}
			if(SelfDeactivateOnTrigger) gameObject.DestroyAPS();
		}
	}
}

