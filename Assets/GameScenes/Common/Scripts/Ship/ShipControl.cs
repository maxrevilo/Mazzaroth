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
			if (EnemyOnLock == null || !EnemyOnLock.isAlive()) {
				EnemyOnLock = ship;
			}
			blackboard.SendEvent(573566531); //EnemyDetected
		}

		//// PRIVATE ////
		private Blackboard blackboard;
		//private ShipStats stats;
		private MovementEngine movementEngine;

		private void Awake () {
			//stats = GetComponent<ShipStats>();
			movementEngine = GetComponent<MovementEngine>(); 
			blackboard = GetComponent<Blackboard>();
			
			DestinyLocation = transform.position;
			//destinyFacingDirection = transform.rotation.eulerAngles.y;
		}
		
		private void Start() {}
		
		private void Update () {}

	}
}

