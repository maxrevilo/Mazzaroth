﻿using System;
using UnityEngine;

namespace Mazzaroth {
    public class TestGUI : BaseMonoBehaviour {
        public Transform obj;
        public BattleScene Scene;
        public RTSCamera RTSCamera;

        public ShipsGroup SelectedGroup {
            get { return Scene.MainPlayer.Army.Groups[groupSelected]; }
        }

        Plane groundPlane;

        int groupSelected = -1;

        void Start () {
            groundPlane = new Plane(Vector3.up, 0);
        }

        void Update () {
            UpdateCameraControls();

            UpdateGroupSelection();

            if(groupSelected != -1) UpdateGroupOrders();
        }

        void UpdateGroupOrders() {
            ShipsGroup Group = SelectedGroup;

            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool pointed = false;


                foreach (Player player in Scene.Players) {
                    if (player.IsEnemy(Scene.MainPlayer)) {
                        foreach (ShipsGroup playerGroup in player.Army.Groups) {
                            foreach (ShipState ship in playerGroup.Ships) {
                                if (ship.collider.Raycast(ray, out hit, float.MaxValue)) {
                                    Debug.Log("Ship hitted: " + ship.name);
                                    SelectedGroup.AtackOrder(ship);
                                    pointed = true;
                                    break;
                                }
                            }
                            if(pointed) break;
                        }
                    }
                    if(pointed) break;
                }

                if(!pointed) {
                    float rayDistance;
                    if (groundPlane.Raycast(ray, out rayDistance)) {
                        obj.position = ray.GetPoint(rayDistance);
                        SelectedGroup.MoveOrder(obj.position);
                    }
                }
            }
        }

        void UpdateGroupSelection() {
            for (int i = 0; i < 5; i++) {
                if(Input.GetButtonDown("Group" + (i + 1))){
                    SelectGroup(i);
                }
            }
        }

        void SelectGroup(int groupIndex) {
            groupSelected = groupIndex;
            ShipsGroup[] groups = Scene.MainPlayer.Army.Groups;
            for (int i = 0; i < groups.Length; i++) {
                ShipsGroup group = groups[i];
                group.Selected = i == groupIndex;
            }
        }

        void UpdateCameraControls() {
            float horizontalSpeed = Input.GetAxisRaw("HorizontalAxis");
            float verticalSpeed = Input.GetAxisRaw("VerticalAxis");
            float deepSpeed = Input.GetAxisRaw("DepthAxis");
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
        }
    }
}

