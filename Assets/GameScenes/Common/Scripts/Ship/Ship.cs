using UnityEngine;
using System.Collections;
using System;

namespace Mazzaroth.Ships {
	
	public class Ship : BaseMonoBehaviour {
		////////////////// SHIP /////////////////////
		public GameObject GUIPrefab;
		[NonSerialized] public ShipsGroup Group;
		
		public DebugConf DebugConfigurations;
		[Serializable]
		public class DebugConf {
			public bool ShowSteeringLimits = false;
		}

		public MovementEngine MovementEngine { get; private set; }
		public ShipControl ShipControl { get; private set; }
		public Color Color { get { return Group.Army.Player.Color; } }
		
		public bool IsEnemy(Ship ship) {
			return Group == null || Group.Army.Player.IsEnemy(ship.Group.Army.Player);
		}

		//// PRIVATE ////
		private ShipStats stats;
		private GameObject DebrisInstance;
		
		private void Awake () {
			stats = GetComponent<ShipStats>();
			MovementEngine = GetComponent<MovementEngine>();
			ShipControl = GetComponent<ShipControl>();
			TimeToFire = 0f;
			
			if (HealthPoints < 0f) {
				HealthPoints = stats.HealthPoints;
			}
			
			DebrisInstance = (Instantiate(stats.DebrisPrefab) as Transform).gameObject;
			DebrisInstance.SetActive(false);
			DebrisInstance.transform.parent = transform;
			
			DetectionArea.transform.localScale *= stats.Sight;
			DetectionArea.ShipDetected += shipDetected;
			
			GUIShipStatus gui = (Instantiate(GUIPrefab) as GameObject).GetComponent<GUIShipStatus>();
			gui.ship = this;
		}
		
		private void Start() {
			if (Group != null) {
				renderer.material.SetColor("_TeamColor", this.Color);
			}
		}
		
		private void Update () {
			TimeToFire -= Time.deltaTime;
			if(DebugConfigurations.ShowSteeringLimits) MovementEngine.DrawSteeringLimits();
		}
		
		////////////////// WEAPONS /////////////////////
		public float TimeToFire;
		public Transform[] FiringLocations;
		
		public bool Fire(Transform target, WeaponStats weapon = null) {
			if (weapon == null)
				weapon = getWeaponAt(0);
			
			if (!isReadyToFire() || !IsTargetInRange(target, weapon)) {
				return false;
			}
			
			PoolingSystem pS = PoolingSystem.Instance;
			Transform firingLocation = getActualFiringLocation(true);
			
			
			GameObject projectile = pS.InstantiateAPS(
				weapon.BulletPrefab.name,
				firingLocation.position,
				firingLocation.rotation,
				this.transform.parent.gameObject
				);
			
			projectile.GetComponent<ProjectileState>().Initiate(this);
			
			TimeToFire = weapon.Cooldown;
			
			return true;
		}
		
		public bool isReadyToFire() {
			return isAlive() && TimeToFire <= 0f;
		}
		
		//// PRIVATE ////
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

		////////////////// HULL /////////////////////
		public delegate void ShipDestroyed(Ship Ship);
		public event ShipDestroyed OnShipDestroyed;
		public float HealthPoints = -1f;
		
		public bool isAlive() {
			return HealthPoints > 0f;
		}
		
		public float Damage(WeaponStats weapon) {
			float heatRawDamage = weapon.HeatConversion * weapon.Damage;
			float physicalRawDamage = weapon.Damage - heatRawDamage;
			
			float computedDamage = Math.Max(physicalRawDamage - stats.Armor, 0f) + heatRawDamage * (1f - stats.HeatDissipation);
			
			HealthPoints -= computedDamage;
			
			AngularImpact(weapon.Impact);
			
			if (!isAlive()) { Die(); }
			
			return computedDamage;
		}
		
		public void Die() {
			if (OnShipDestroyed != null) OnShipDestroyed(this);
			
			DebrisInstance.transform.parent = transform.parent;
			DebrisInstance.transform.position = transform.position;
			DebrisInstance.transform.rotation = transform.rotation;
			DebrisInstance.rigidbody.velocity = rigidbody.velocity;
			DebrisInstance.rigidbody.angularVelocity = rigidbody.angularVelocity;
			DebrisInstance.SetActive(true);
			gameObject.DestroyAPS();
		}
		
		public void AngularImpact(float impact) {
			float impactStr = impact / rigidbody.mass * Mathf.Sign(UnityEngine.Random.Range(-1, 1));
			addAngularVelocity(Vector3.up * 3f * impactStr);
			transform.Rotate(new Vector3(0, 20f * impactStr, 0));
		}

		////////////////// DETECTORS /////////////////////
		public DetectionArea DetectionArea;
		//// PRIVATE ////
		private int actualFiringLocation;
		
		private bool IsTargetInRange(Transform target, WeaponStats weapon = null) {
			if (weapon == null)
				weapon = getWeaponAt(0);
			
			float distSqr = Vector3.SqrMagnitude(this.transform.position - target.position);
			
			return distSqr <= Mathf.Pow(weapon.Range, 2);
		}
		
		private void shipDetected(Ship ship) {
			ShipControl.ShipDetected(ship);
		}
	}
}

