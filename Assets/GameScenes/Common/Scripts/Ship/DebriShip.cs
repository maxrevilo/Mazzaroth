using UnityEngine;
using System;

namespace Mazzaroth {
	public class DebriShip: BaseMonoBehaviour {
		public GameObject Explosion;

		void Start() {
			GameObject explosion = Instantiate(Explosion, transform.position, transform.rotation) as GameObject;
			explosion.transform.parent = transform;
//			explosion.transform.position = Vector3.zero;
//			explosion.transform.rotation = Quaternion.identity;
//			explosion.SetActive(true);
		}
	}
}

