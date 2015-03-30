using System;
using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth.Ships {
	public class ShipControl : BaseMonoBehaviour {
		public enum ShipStances {
			Raid,
			HoldPosition,
			Defend, 
			Retreat,
		}
		
		public ShipStances Stance = ShipStances.Raid;
		public Vector3 DestinyLocation { get; private set; }
		//private float destinyFacingDirection { get; private set; }
		public Ship EnemyOnLock { get; private set; }
		
		public void MoveOrder(Vector3 destiny) {
			DestinyLocation = destiny;
			blackboard.SendEvent(1761075472); //MoveOrder
		}
		
		public void AttackOrder(Ship enemy) {
			EnemyOnLock = enemy;
			blackboard.SendEvent(1577261801); //EnemyDetected

		}
		
		public void AggressiveMoveOrder(Vector3 destiny) {
			DestinyLocation = destiny;
			blackboard.SendEvent(1965341386); //MoveOrder
		}
		
		public void ShipDetected(Ship ship) {
			if (EnemyOnLock == null || !EnemyOnLock.IsAlive()) {
				EnemyOnLock = ship;
			}
			blackboard.SendEvent(573566531); //EnemyDetected
		}

		public void EnemyDied() {
			blackboard.SendEvent(843881883); //EnemyDied
		}

		public void OnDestiny() {
			blackboard.SendEvent(1202858853); //OnDestiny
		}
		
		public void EnemyAtacked(Ship enemy) {
			ship.ShipDetected(enemy);
		}

		virtual public void DriveToPosition() {
			movementEngine.HeadTowardPosition(DestinyLocation, true);
			movementEngine.MoveForwardToPosition(DestinyLocation);
		}
		
		virtual public void ChaceEnemy() {
			Vector3 enemyPosition = EnemyOnLock.transform.position;
			movementEngine.HeadTowardPosition(enemyPosition, true);
			movementEngine.MoveForward();
			Debug.DrawLine(transform.position, enemyPosition, Color.red);
		}

		//// PROTECTED ////
		protected Blackboard blackboard;
		//protected ShipStats stats;
		protected MovementEngine movementEngine;
		protected Ship ship;
		
		//// PRIVATE ////
		private void Awake () {
			//stats = GetComponent<ShipStats>();
			ship = GetComponent<Ship>();
			movementEngine = GetComponent<MovementEngine>(); 
			blackboard = GetComponent<Blackboard>();
			
			DestinyLocation = transform.position;
			//destinyFacingDirection = transform.rotation.eulerAngles.y;
		}
		
		private void Start() {}
		
		private void Update () {}

	}
}

