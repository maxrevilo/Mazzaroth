using UnityEngine;
using System.Collections;

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

        public bool Fire(Transform target, WeaponStats weapon = null) {
            if (weapon == null)
                weapon = getWeaponAt(0);

            if (!isReadyToFire() || !isTargetInRange(target, weapon)) {
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

        public void goToLocation(Vector3 destiny) {
            destinyLocation = destiny;
//            moving = true;
        }

        bool isTargetInRange(Transform target, WeaponStats weapon = null) {
            if (weapon == null)
                weapon = getWeaponAt(0);

            float distSqr = Vector3.SqrMagnitude(this.transform.position - target.position);

            return distSqr <= Mathf.Pow(weapon.Range, 2);
        }


        private ShipStats stats;
        private int actualFiringLocation;
        private Vector3 destinyLocation;
        private float destinyFacingDirection;
//        private bool moving = false;
        private Vector3 relativeVel;

        // Use this for initialization
        void Start () {
            stats = GetComponent<ShipStats>();
            TimeToFire = 0f;

            destinyLocation = transform.position;
            destinyFacingDirection = transform.rotation.eulerAngles.y;
            rigidbody.maxAngularVelocity = stats.angularSpeedRad;

            if (HealthPoints < 0f) {
                HealthPoints = stats.HealthPoints;
            }
        }

        // Update is called once per frame
        void Update () {
            TimeToFire -= Time.deltaTime;

            Fire(this.transform);
        }

        void FixedUpdate() {
            const float MIN_DISTANCE_TO_DESTINY = 0.5f;
            float sqrSistanceToDestiny = Vector3.SqrMagnitude(this.transform.position - destinyLocation);

            relativeVel = transform.InverseTransformDirection(rigidbody.velocity);

            useLateralStabilizators();
//            multiplyAngularVelocity(0.9f);

            if (sqrSistanceToDestiny >= Mathf.Pow(MIN_DISTANCE_TO_DESTINY, 2)) {
                headTowardPosition(destinyLocation);
                moveForwardToPosition(destinyLocation);
            } else {
                useBreaks();
                useAngularBreaks();
            }
        }

        WeaponStats getWeaponAt(int index) {
            return stats.Weapons[index];
        }

        void headTowardPosition(Vector3 position) {
            float angle = Math3d.SignedVectorAngle(transform.forward, position - transform.position, transform.up) * Mathf.Deg2Rad;
            float sign = Mathf.Sign(angle);
            float absAngle = sign * angle;

            float discriminant = Mathf.Pow(rigidbody.angularVelocity.y, 2) - 2f * stats.angularAccelerationRad * absAngle;

            Debug.Log("rigidbody.angularVelocity.y " + rigidbody.angularVelocity.y);
            Debug.Log("stats.angularAccelerationRad "+ stats.angularAccelerationRad);
            Debug.Log("absAngle" + absAngle);

            if (discriminant < 0f) {
                Debug.DrawLine(transform.position, transform.up * 3f + transform.position, Color.green);
                addAngularVelocity(Vector3.up * sign * stats.angularAccelerationRad * Time.deltaTime);
            } else {
                Debug.DrawLine(transform.position, transform.position - transform.up * 5f, Color.red);
                useAngularBreaks();
            }
        }

        void moveForwardToPosition(Vector3 position) {
            float distance = Vector3.Magnitude(transform.position - position);
            float discriminant = Mathf.Pow(relativeVel.z, 2) - 2f * stats.deacceleration * distance;


            if (discriminant < 0f) {
                Debug.DrawLine(transform.position, destinyLocation, Color.green);
                addVelocity(transform.forward * stats.acceleration * Time.deltaTime);
            } else {
                Debug.DrawLine(transform.position, destinyLocation, Color.red);
                useBreaks();
            }

            if (rigidbody.velocity.sqrMagnitude > Mathf.Pow(stats.Speed, 2)) {
                rigidbody.velocity = Math3d.SetVectorLength(rigidbody.velocity, stats.Speed);
            }
        }

        void useLateralStabilizators() {
            Vector3 stabilizerVel = -relativeVel;
            stabilizerVel.z = 0;
            addVelocity(transform.TransformDirection(stabilizerVel) * LateralStabilizatorsFactor);
        }

        void useBreaks() {
            float amount = stats.deacceleration * Time.deltaTime;
            amount = Mathf.Min(relativeVel.z, amount);
            addVelocity(-transform.forward * amount);
        }

        void useAngularBreaks() {
            float angularVelocity = rigidbody.angularVelocity.y;
            float angularDirection = Mathf.Sign(angularVelocity);
            float absAngularVelocity = angularVelocity * angularDirection;

            float amount = stats.angularAccelerationRad * Time.deltaTime;
            amount = Mathf.Min(absAngularVelocity, amount);
            addAngularVelocity(- angularDirection * Vector3.up * amount);
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

