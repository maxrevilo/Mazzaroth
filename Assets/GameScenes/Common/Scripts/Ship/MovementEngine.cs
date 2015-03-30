using System;
using UnityEngine;
using System.Collections;

namespace Mazzaroth.Ships {
	public class MovementEngine : BaseMonoBehaviour {
		public float LateralStabilizatorsFactor = 0.1f;
		public Vector3 RelativeVelocity { get; private set; }
		
		public void MoveForward() {
			addVelocity(transform.forward * stats.acceleration * Time.deltaTime);
		}
		
		public void UseBreaks() {
			float amount = stats.deacceleration * Time.deltaTime;
			amount = Mathf.Min(RelativeVelocity.z, amount);
			addVelocity(-transform.forward * amount);
		}
		
		public void UseAngularBreaks() {
			float angularVelocity = rigidbody.angularVelocity.y;
			float angularDirection = Mathf.Sign(angularVelocity);
			float absAngularVelocity = angularVelocity * angularDirection;
			
			float amount = stats.angularAccelerationRad * Time.deltaTime;
			amount = Mathf.Min(absAngularVelocity, amount);
			addAngularVelocity(- angularDirection * Vector3.up * amount);
		}

		public void HeadTowardPosition(Vector3 position) {
			float angle = Math3d.SignedVectorAngle(transform.forward, position - transform.position, transform.up) * Mathf.Deg2Rad;
			float sign = Mathf.Sign(angle);
			float absAngle = sign * angle;
			
			float discriminant = Mathf.Pow(rigidbody.angularVelocity.y, 2) - 2f * stats.angularAccelerationRad * absAngle;
			
			bool reachable = IsInsideSteeringLimit(position);
			
			if (reachable) {
				if (discriminant < 0f) {
					Debug.DrawLine(transform.position, transform.up * 3f + transform.position, Color.green);
					addAngularVelocity(Vector3.up * sign * stats.angularAccelerationRad * Time.deltaTime);
				} else {
					Debug.DrawLine(transform.position, transform.position - transform.up * 3f, Color.red);
					UseAngularBreaks();
				}
			} else {
				Debug.DrawLine(transform.position, transform.position - transform.up * 5f, Color.magenta);
				addAngularVelocity(-Vector3.up * sign * stats.angularAccelerationRad * Time.deltaTime);
			}
		}
		
		public void MoveForwardToPosition(Vector3 position) {
			float distance = Vector3.Magnitude(transform.position - position);
			float discriminant = Mathf.Pow(RelativeVelocity.z, 2) - 2f * stats.deacceleration * distance;
			
			
			if (discriminant < 0f) {
				Debug.DrawLine(transform.position, position, Color.green);
				MoveForward();
			} else {
				Debug.DrawLine(transform.position, position, Color.red);
				UseBreaks();
			}
			
		}

		public void DrawSteeringLimits() {
			float STEPS = 24;
			
			float deff = 2f * Mathf.PI / STEPS;
			Vector3 posA = this.transform.position + this.transform.right * steeringLimitLateralRadius;
			Vector3 previous = posA;
			Vector3 actual;
			for (float r = 0; r <= 2f * Mathf.PI; r += deff) {
				actual = posA + new Vector3(Mathf.Sin(r) * steeringLimitLateralRadius, 0f, Mathf.Cos(r) * steeringLimitLateralRadius);
				Debug.DrawLine(previous, actual);
				previous = actual;
			}
			
			Vector3 posB = this.transform.position - this.transform.right * steeringLimitLateralRadius;
			previous = posB;
			for (float r = 0; r <= 2f * Mathf.PI; r += deff) {
				actual = posB + new Vector3(Mathf.Sin(r) * steeringLimitLateralRadius, 0f, Mathf.Cos(r) * steeringLimitLateralRadius);
				Debug.DrawLine(previous, actual);
				previous = actual;
			}
		}

		public bool IsInsideSteeringLimit(Vector3 destiny) {
			Vector2 right2D = new Vector2(transform.right.x, transform.right.z);
			Circle steeringLimitRight = new Circle(steeringLimitLateralRadius, right2D * steeringLimitLateralRadius);
			Circle steeringLimitLeft = new Circle(steeringLimitLateralRadius, -right2D * steeringLimitLateralRadius);
			
			destiny = destiny - transform.position;
			Vector2 destiny2D = new Vector2(destiny.x, destiny.z);
			
			bool result = 
				steeringLimitRight.Contains(destiny2D) ||
					steeringLimitLeft.Contains(destiny2D);
			return !result;
		}

		//// PRIVATE ////
		private Ship ship;
		private ShipStats stats;
		private float steeringLimitLateralRadius = 7.5f;

		private void Awake () {
			ship = GetComponent<Ship>();
			stats = GetComponent<ShipStats> ();

			rigidbody.maxAngularVelocity = stats.angularSpeedRad;
		}
		
		private void Start() {}
		
		private void Update () {}
		
		private void FixedUpdate() {
			RelativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
			useLateralStabilizators();
			
			if (rigidbody.velocity.sqrMagnitude > Mathf.Pow(stats.Speed, 2)) {
				rigidbody.velocity = Math3d.SetVectorLength(rigidbody.velocity, stats.Speed);
			}
		}

		private void useLateralStabilizators() {
			Vector3 stabilizerVel = -RelativeVelocity;
			stabilizerVel.z = 0;
			addVelocity(transform.TransformDirection(stabilizerVel) * LateralStabilizatorsFactor);
		}
	}
}

