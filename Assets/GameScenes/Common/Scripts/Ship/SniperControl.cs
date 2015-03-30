using System;
using UnityEngine;
using System.Collections;
using BehaviourMachine;

namespace Mazzaroth.Ships {
	public class SniperControl : ShipControl {

		override public void DriveToPosition() {
			movementEngine.HeadTowardPosition(DestinyLocation);
			if (Mathf.Abs(movementEngine.angleToPosition (DestinyLocation)) < Mathf.Deg2Rad * 3f) {
				movementEngine.MoveForwardToPosition (DestinyLocation);
			} else {
				movementEngine.UseBreaks();
			}
		}
		
		override public void ChaceEnemy() {
			Vector3 enemyPosition = EnemyOnLock.transform.position;
			movementEngine.HeadTowardPosition(enemyPosition);
			if (ship.IsTargetInRange(EnemyOnLock.transform)) {
				movementEngine.UseBreaks();
			} else {
				movementEngine.MoveForward ();
			}
			Debug.DrawLine(transform.position, enemyPosition, Color.red);
		}
	}
}

