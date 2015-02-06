using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth {

    public class ShipState : BaseMonoBehaviour {

        public enum ShipStances {
            Raid,
            HoldPosition,
            Defend, 
            Retreat,
        }

        //TODO: Esto debería estar en el Group, no en ShipState
        public enum ShipFormations {
            Bird,
            Line,
            Square,
            Circle
        }

        public float HealthPoints = -1f;
        public ShipStances Stance = ShipStances.Raid;
        public ShipFormations Formation = ShipFormations.Bird;
        public float TimeToFire;

        public float LateralStabilizatorsFactor = 0.1f;
        public Transform[] FiringLocations;
        public Vector3 DestinyLocation { get; private set; }
        public Vector3 RelativeVelocity { get; private set; }

        public bool Fire(Transform target, WeaponStats weapon = null) {
            if (weapon == null)
                weapon = getWeaponAt(0);

            if (!isReadyToFire() || !IsTargetInRange(target, weapon)) {
                return false;
            }

            PoolingSystem pS = PoolingSystem.Instance;
            Transform firingLocation = getActualFiringLocation(true);


            pS.InstantiateAPS(
                weapon.BulletPrefab.name,
                firingLocation.position,
                firingLocation.rotation,
                this.transform.parent.gameObject
            );

            TimeToFire = weapon.Cooldown;

            return true;
        }

        public bool isReadyToFire() {
            return isAlive() && TimeToFire <= 0f;
        }

        public bool isAlive() {
            return HealthPoints > 0f;
        }

        public void MoveOrder(Vector3 destiny) {
            blackboard.SendEvent(1761075472); //MoveOrder
            DestinyLocation = destiny;
        }

        public void HeadTowardPosition(Vector3 position) {
            float angle = Math3d.SignedVectorAngle(transform.forward, position - transform.position, transform.up) * Mathf.Deg2Rad;
            float sign = Mathf.Sign(angle);
            float absAngle = sign * angle;

            float discriminant = Mathf.Pow(rigidbody.angularVelocity.y, 2) - 2f * stats.angularAccelerationRad * absAngle;

            if (discriminant < 0f) {
                Debug.DrawLine(transform.position, transform.up * 3f + transform.position, Color.green);
                addAngularVelocity(Vector3.up * sign * stats.angularAccelerationRad * Time.deltaTime);
            } else {
                Debug.DrawLine(transform.position, transform.position - transform.up * 5f, Color.red);
                UseAngularBreaks();
            }
        }

        public void MoveForwardToPosition(Vector3 position) {
            float distance = Vector3.Magnitude(transform.position - position);
            float discriminant = Mathf.Pow(RelativeVelocity.z, 2) - 2f * stats.deacceleration * distance;


            if (discriminant < 0f) {
                Debug.DrawLine(transform.position, position, Color.green);
                addVelocity(transform.forward * stats.acceleration * Time.deltaTime);
            } else {
                Debug.DrawLine(transform.position, position, Color.red);
                UseBreaks();
            }

            if (rigidbody.velocity.sqrMagnitude > Mathf.Pow(stats.Speed, 2)) {
                rigidbody.velocity = Math3d.SetVectorLength(rigidbody.velocity, stats.Speed);
            }
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

        bool IsTargetInRange(Transform target, WeaponStats weapon = null) {
            if (weapon == null)
                weapon = getWeaponAt(0);

            float distSqr = Vector3.SqrMagnitude(this.transform.position - target.position);

            return distSqr <= Mathf.Pow(weapon.Range, 2);
        }


        ////////////////////////// PRIVATE //////////////////////////

        private ShipStats stats;
        private Blackboard blackboard;
        private int actualFiringLocation;
//        private float destinyFacingDirection;

        // Use this for initialization
        void Start () {
            stats = GetComponent<ShipStats>();
            blackboard = GetComponent<Blackboard>();
            TimeToFire = 0f;

            DestinyLocation = transform.position;
//            destinyFacingDirection = transform.rotation.eulerAngles.y;
            rigidbody.maxAngularVelocity = stats.angularSpeedRad;

            if (HealthPoints < 0f) {
                HealthPoints = stats.HealthPoints;
            }
        }

        void Update () {
            TimeToFire -= Time.deltaTime;
        }

        void FixedUpdate() {
            RelativeVelocity = transform.InverseTransformDirection(rigidbody.velocity);
            useLateralStabilizators();
        }

        private void useLateralStabilizators() {
            Vector3 stabilizerVel = -RelativeVelocity;
            stabilizerVel.z = 0;
            addVelocity(transform.TransformDirection(stabilizerVel) * LateralStabilizatorsFactor);
        }

        WeaponStats getWeaponAt(int index) {
            return stats.Weapons[index];
        }

        Transform getActualFiringLocation(bool MoveToTheNextFiringLocation = false) {
            Transform firingLocation = FiringLocations [actualFiringLocation];

            if (MoveToTheNextFiringLocation) {
                actualFiringLocation++;
                if (actualFiringLocation >= FiringLocations.Length) {
                    actualFiringLocation = 0;
                }
            }

            return firingLocation;
        }
    }
}

