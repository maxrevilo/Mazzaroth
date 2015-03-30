using UnityEngine;
using System;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class DetectionArea: BaseMonoBehaviour {
        public delegate void EventHandler (Ship ship);
        public event EventHandler ShipDetected;

        private Ship Ship;

        void Awake() {
            Ship = GetComponentInParent<Ship>();
        }

        void OnTriggerEnter (Collider collider) {
            Ship otherShip = collider.GetComponent<Ship>();
            if (otherShip != null && Ship.IsEnemy(otherShip)) {
                if(ShipDetected != null) ShipDetected(otherShip);
            }
        }
    }
}

