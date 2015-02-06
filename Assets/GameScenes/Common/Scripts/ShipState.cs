﻿using UnityEngine;
using System.Collections;
using System;
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
        public DetectionArea DetectionArea;
        public Vector3 DestinyLocation { get; private set; }
        public Vector3 RelativeVelocity { get; private set; }
        public ShipState EnemyOnLock { get; private set; }

        public DebugConf DebugConfigurations;

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
            DestinyLocation = destiny;
            blackboard.SendEvent(1761075472); //MoveOrder
        }

        public void AttackOrder(ShipState enemy) {
            EnemyOnLock = enemy;
            blackboard.SendEvent(573566531); //EnemyDetected
        }

        public void HeadTowardPosition(Vector3 position) {
            float angle = Math3d.SignedVectorAngle(transform.forward, position - transform.position, transform.up) * Mathf.Deg2Rad;
            float sign = Mathf.Sign(angle);
            float absAngle = sign * angle;

            float discriminant = Mathf.Pow(rigidbody.angularVelocity.y, 2) - 2f * stats.angularAccelerationRad * absAngle;

            bool reachable = isInsideSteeringLimit(position);

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
        void Awake () {
            stats = GetComponent<ShipStats>();
            blackboard = GetComponent<Blackboard>();
            TimeToFire = 0f;

            DestinyLocation = transform.position;
//            destinyFacingDirection = transform.rotation.eulerAngles.y;
            rigidbody.maxAngularVelocity = stats.angularSpeedRad;

            if (HealthPoints < 0f) {
                HealthPoints = stats.HealthPoints;
            }

            DetectionArea.transform.localScale *= stats.Sight;
            DetectionArea.ShipDetected += shipDetected;
        }

        void shipDetected(ShipState ship) {
            EnemyOnLock = ship;
            blackboard.SendEvent(573566531); //EnemyDetected
        }

        void Update () {
            TimeToFire -= Time.deltaTime;
            if(DebugConfigurations.ShowSteeringLimits) drawSteeringLimits();
        }

        void FixedUpdate() {
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


        float steeringLimitLateralRadius = 7.5f;

        void drawSteeringLimits() {
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

        bool isInsideSteeringLimit(Vector3 destiny) {
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


        [Serializable]
        public class DebugConf {
            public bool ShowSteeringLimits = false;
        }
    }
}

