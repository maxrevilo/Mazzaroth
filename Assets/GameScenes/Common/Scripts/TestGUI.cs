using UnityEngine;

namespace Mazzaroth {
    public class TestGUI : BaseMonoBehaviour {
        public Transform obj;
        public ShipState Ship;
        Plane groundPlane;

        void Start () {
            groundPlane = new Plane(Vector3.up, 0);
        }

        void Update () {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (groundPlane.Raycast(ray, out rayDistance)) {
                    obj.position = ray.GetPoint(rayDistance);
                    Ship.MoveOrder(obj.position);
                }
            }
        }
    }
}

