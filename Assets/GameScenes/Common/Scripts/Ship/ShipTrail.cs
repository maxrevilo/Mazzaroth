using System;
using UnityEngine;
using Mazzaroth.Ships;

namespace Mazzaroth {
	public class ShipTrail : BaseMonoBehaviour {
		Ship ship;

		void Start() {
			ship = GetComponentInParent<Ship>();
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

