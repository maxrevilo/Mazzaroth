using System;
using UnityEngine;

namespace Mazzaroth {
	public class LevelWon : BaseMonoBehaviour {
		public float ExitTime = 2f;
		public string NextLevel = "";
		public SceneController SceneController;

		private bool exiting = false;
		private float timeToExit;

		void Start() {
			timeToExit = ExitTime;
		}

		void Update() {
			if (timeToExit > 0) {
				timeToExit -= Time.deltaTime;

				if (timeToExit <= 0) {
					SceneController.FadeOutAndChangeScene(NextLevel);
				}
			}
		}
	}
}

