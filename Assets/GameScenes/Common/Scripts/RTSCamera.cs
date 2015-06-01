using System;
using UnityEngine;
using System.Collections;

namespace Mazzaroth {
    public class RTSCamera : BaseMonoBehaviour {
        public Vector3 TargetPosition;
        public float TargetZoom = 0f;

        public float MovementAdaptationSpeed = 0.6f;
        public float ZoomAdaptationSpeed = 0.1f;

        private Vector3 forward;
        private Vector3 right;
		private Camera camera;

		private const float bottomLimit = -60f;
		private const float topLimit = -300f;
		public float currentZoom {
			get {
				return Mathf.InverseLerp(topLimit, bottomLimit, transform.InverseTransformPoint(camera.transform.position).z);
			}
			set {
				camera.transform.position = transform.position + camera.transform.forward * Mathf.Lerp(topLimit, bottomLimit, value);
			}
		}

		void Awake() {
			camera = GetComponentInChildren<Camera>();
            TargetPosition = transform.position;
			TargetZoom = currentZoom;

            Vector3 mask = new Vector3(1, 0, 1);
            forward = Vector3.Scale(transform.forward, mask).normalized;
            right = Vector3.Scale(transform.right, mask).normalized;
        }

        void FixedUpdate() {
            transform.position = transform.position * (1f - MovementAdaptationSpeed) + TargetPosition * MovementAdaptationSpeed;

			currentZoom = currentZoom * (1f - ZoomAdaptationSpeed) + TargetZoom * ZoomAdaptationSpeed;
        }
		
		void Update() {
		}

        public void MoveTo(Vector3 targetPosition) {
            TargetPosition = targetPosition;
        }

        public void MoveDiff(Vector3 addPosition) {
            Vector3 addPositionTransformed = right * addPosition.x + forward * addPosition.z;
            MoveTo(TargetPosition + addPositionTransformed);
        }

        public void ZoomTo(float targetZoom) {
			TargetZoom = Mathf.Clamp01(targetZoom);
        }

        public void AddZoom(float diff) {
            ZoomTo(TargetZoom + diff);
        }

    }
}

