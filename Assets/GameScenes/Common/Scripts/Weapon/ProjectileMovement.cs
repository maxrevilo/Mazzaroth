using System;
using UnityEngine;

namespace Mazzaroth {
    public class ProjectileMovement : BaseMonoBehaviour
    {

        protected Projectile projectile;
        protected WeaponStats weaponStats;

        public void Initiate()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            _PY_Initiate(rigidbody);
            _PY_AccelerateAndMove(weaponStats.Speed, rigidbody.transform.forward);
        }

        public void MoveForward()
        { 
        }


        public void HeadTowardPosition(Vector3 position, bool surroundSteeringLimits = false)
        {
        }


        // PROTECTED
        protected WeaponStats stats;

        protected void Awake()
        {
            projectile = GetComponent<Projectile>();
            weaponStats = GetComponent<WeaponStats>();
            stats = GetComponent<WeaponStats>();
        }

        protected void Update()
        {
            _PY_Update();
        }

        //PRIVATE

        //TESTING WITH A NEW MOVEMENT/PHYSICS MODEL
        private enum _PY_MovementStates
        {
            Idle, Moving, Accelerating, Deaccelerating, Turning
        }

        struct _PY_PhysicActor
        {
            public _PY_MovementStates MovementState;
            public float StartingTime;
            public float Speed;
            public float AccelerationTime;
            public Vector3 StartingPoint;
            public Vector3 AbsoluteDirection;

            static public void CreateIdle(Rigidbody body, out _PY_PhysicActor newState)
            {
                newState = new _PY_PhysicActor()
                {
                    MovementState = _PY_MovementStates.Idle,
                    StartingTime = Time.time,
                    Speed = 0,
                    AccelerationTime = 0,
                    StartingPoint = body.position,
                    AbsoluteDirection = Vector3.forward
                };
            }

            static public void CreateMoveForward(
                Rigidbody body, float speed, float accelerationTime, Vector3 absoluteDirection,
                out _PY_PhysicActor newState)
            {
                newState = new _PY_PhysicActor() {
                    MovementState = _PY_MovementStates.Moving,
                    StartingTime = Time.time,
                    Speed = speed,
                    AccelerationTime = accelerationTime,
                    StartingPoint = body.position,
                    AbsoluteDirection = absoluteDirection
                };
            }

            public bool IsEquivalent(ref _PY_PhysicActor newState, bool ignoreAplicationDiferences = false) {
                bool equivalent = true;

                equivalent &= this.MovementState == newState.MovementState;
                if (!equivalent) return false; //Early shortcut

                equivalent &= this.Speed == newState.Speed;
                equivalent &= this.AccelerationTime == newState.AccelerationTime;
                equivalent &= this.AbsoluteDirection == newState.AbsoluteDirection;

                if (!ignoreAplicationDiferences) {
                    equivalent &= this.StartingTime == newState.StartingTime;
                    equivalent &= this.StartingPoint == newState.StartingPoint;
                }

                return equivalent;
            }
        }

        // State
        private _PY_PhysicActor _PY_State;
        private Rigidbody _PY_rigidBody;
        private float _PY_accelerationState;

        private void _PY_Initiate(Rigidbody rigidBody)
        {
            this._PY_rigidBody = rigidBody;
            this._PY_rigidBody.isKinematic = true;

            _PY_PhysicActor.CreateIdle(this._PY_rigidBody, out _PY_State);
        }

        private void _PY_AccelerateAndMove(float maxSpeed, Vector3? direction = null, float acelerationTime = 0)
        {
            Vector3 directionUsed;
            if(direction == null) {
                directionUsed = _PY_State.AbsoluteDirection;
            } else {
                directionUsed = direction.Value;
            }

            _PY_PhysicActor newState;
            _PY_PhysicActor.CreateMoveForward(_PY_rigidBody, maxSpeed, acelerationTime, directionUsed, out newState);

            if (IsDifferentFromCurrentState(ref newState))
            {
                _PY_State = newState;
            }
        }

        private bool IsDifferentFromCurrentState(ref _PY_PhysicActor newState)
        {
            return !_PY_State.IsEquivalent(ref newState, true);
        }

        private void _PY_Update()
        {
            float dt = Time.time - _PY_State.StartingTime;
            Vector3 finalPosition = _PY_State.StartingPoint;

            switch (_PY_State.MovementState)
            {
                case _PY_MovementStates.Idle:
                    break;
                case _PY_MovementStates.Accelerating:
                    throw new NotImplementedException();
                    break;
                case _PY_MovementStates.Moving:
                    finalPosition = _PY_State.StartingPoint + _PY_State.AbsoluteDirection * _PY_State.Speed * dt;
                    break;
                case _PY_MovementStates.Deaccelerating:
                    throw new NotImplementedException();
                    break;
                case _PY_MovementStates.Turning:
                    throw new NotImplementedException();
                    break;
            }

            _PY_rigidBody.position = finalPosition;
        }
    }
}
