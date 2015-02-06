using UnityEngine;

namespace Mazzaroth {
    public class TestGUI : BaseMonoBehaviour {
        public Transform obj;
        public ShipState Ship;
        public ShipState Ship2;
        Plane groundPlane;

        void Start () {
            groundPlane = new Plane(Vector3.up, 0);
        }

        void Update () {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Ship2.collider.Raycast(ray, out hit, float.MaxValue)) {
                    Ship.AttackOrder(Ship2);
                } else {
                    float rayDistance;
                    if (groundPlane.Raycast(ray, out rayDistance)) {
                        obj.position = ray.GetPoint(rayDistance);
                        Ship.MoveOrder(obj.position);
                    }
                }



            }
            /*
            if ( Input.GetMouseButtonDown(0)){
                var hit : RaycastHit;
                var ray : Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                var select = GameObject.FindWithTag("select").transform;
                if (Physics.Raycast (ray, hit, 100.0)){
                    select.tag = "none";
                    hit.collider.transform.tag = "select";
                }
            }
            */
        }
    }
}

