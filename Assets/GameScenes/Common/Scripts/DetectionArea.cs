using UnityEngine;
using System;

namespace Mazzaroth {
    public class DetectionArea: BaseMonoBehaviour {
        public delegate void EventHandler (ShipState ship);
        public event EventHandler ShipDetected;

        void OnTriggerEnter (Collider collider) {
            GameObject gameObject = collider.gameObject;
            ShipState ship = collider.GetComponent<ShipState>();
            if (ship != null && gameObject != transform.parent.gameObject) {
                if(ShipDetected != null) ShipDetected(ship);
            }
        }
    }
}

