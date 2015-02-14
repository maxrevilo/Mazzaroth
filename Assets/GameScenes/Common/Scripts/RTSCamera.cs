using System;
using UnityEngine;
using System.Collections;

namespace Mazzaroth {
    public class RTSCamera : BaseMonoBehaviour {
        public Vector3 TargetPosition;
        public float TargetZoom = 1f;
        public float MaxZoom = 4f;
        public float MinZoom = 0.5f;


        public float MovementAdaptationSpeed = 0.6f;
        public float ZoomAdaptationSpeed = 0.1f;

        private Vector3 forward;
        private Vector3 right;

        float currentZoom {
            get { return  20f / camera.orthographicSize; }
            set { camera.orthographicSize = 1f / value * 20f; }
        }

        void Awake() {
            TargetPosition = transform.position;

            currentZoom = TargetZoom;

            Vector3 mask = new Vector3(1, 0, 1);
            forward = Vector3.Scale(transform.forward, mask).normalized;
            right = Vector3.Scale(transform.right, mask).normalized;
        }

        void FixedUpdate() {
            transform.position = transform.position * (1f - MovementAdaptationSpeed) + TargetPosition * MovementAdaptationSpeed;

            currentZoom = currentZoom * (1f - ZoomAdaptationSpeed) + TargetZoom * ZoomAdaptationSpeed;
        }

        public void MoveTo(Vector3 targetPosition) {
            TargetPosition = targetPosition;
        }

        public void MoveDiff(Vector3 addPosition) {
            Vector3 addPositionTransformed = right * addPosition.x + forward * addPosition.z;
            MoveTo(TargetPosition + addPositionTransformed);
        }

        public void ZoomTo(float targetZoom) {
            TargetZoom = Mathf.Min(Mathf.Max(targetZoom, MinZoom), MaxZoom);
        }

        public void ZoomFactor(float factorZoom) {
            ZoomTo(TargetZoom * factorZoom);
        }

    }
}

