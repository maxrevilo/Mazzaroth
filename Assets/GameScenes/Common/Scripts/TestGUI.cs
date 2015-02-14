using System;
using UnityEngine;

namespace Mazzaroth {
    public class TestGUI : BaseMonoBehaviour {
        public Transform obj;
        public ShipState Ship;
        public ShipState Ship2;
        public RTSCamera RTSCamera;

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

            float horizontalSpeed = Input.GetAxisRaw("Horizontal");
            float verticalSpeed = Input.GetAxisRaw("Vertical");
            float deepSpeed = Input.GetAxis("Mouse ScrollWheel");
            float CameraSpeed = 80f;
            float ZoomSpeed = 1f;
            if (Mathf.Abs(verticalSpeed) > 0.05f) {
                RTSCamera.MoveDiff(Vector3.forward * verticalSpeed * CameraSpeed * Time.deltaTime);
            }

            if (Mathf.Abs(horizontalSpeed) > 0.05f) {
                RTSCamera.MoveDiff(Vector3.right * horizontalSpeed * CameraSpeed * Time.deltaTime);
            }


            if (deepSpeed > 0) {
                RTSCamera.ZoomFactor(1f / (1f + deepSpeed) * ZoomSpeed);
            }

            if (deepSpeed < 0) {
                RTSCamera.ZoomFactor(1f - deepSpeed * ZoomSpeed);
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

