using UnityEngine;
using System;

namespace Mazzaroth {
    public class DetectionArea: BaseMonoBehaviour {
        public delegate void EventHandler (ShipState ship);
        public event EventHandler ShipDetected;

        private ShipState Ship;

        void Awake() {
            Ship = GetComponentInParent<ShipState>();
        }

        void OnTriggerEnter (Collider collider) {
            ShipState otherShip = collider.GetComponent<ShipState>();
            if (otherShip != null && Ship.IsEnemy(otherShip)) {
                if(ShipDetected != null) ShipDetected(otherShip);
            }
        }
    }
}

