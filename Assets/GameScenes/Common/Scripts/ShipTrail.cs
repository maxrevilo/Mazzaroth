using System;
using UnityEngine;

namespace Mazzaroth {
	public class ShipTrail : BaseMonoBehaviour {
		ShipState ship;

		void Start() {
			ship = GetComponentInParent<ShipState>();
			if (ship) {
				Color TeamColor = ship.Color;
				Material trailMaterial = GetComponent<TrailRenderer>().material;
				Color color = trailMaterial.GetColor("_TintColor");
				TeamColor.a = color.a;
				trailMaterial.SetColor("_TintColor", TeamColor);
			}


		}

	}
}

