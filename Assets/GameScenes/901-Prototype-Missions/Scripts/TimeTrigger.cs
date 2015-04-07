using UnityEngine;
using System.Collections;
using Mazzaroth.Ships;

namespace Mazzaroth {
	public class TimeTrigger : BaseTrigger {
		public float TimeToTrigger;

		private float time;

		void Start () {
			time = 0;
		}

		void Update() {
			time += Time.deltaTime;
			if (time > TimeToTrigger) {
				Trigger ();
			}
		}
	}
}


