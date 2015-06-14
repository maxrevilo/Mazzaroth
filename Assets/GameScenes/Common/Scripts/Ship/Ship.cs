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
        public ShipWeapons ShipWeapons { get; private set; }
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
            ShipWeapons = GetComponent<ShipWeapons>();
			
			if (HealthPoints < 0f) {
				HealthPoints = stats.HealthPoints;
			}
			
			DebrisInstance = (Instantiate(stats.DebrisPrefab) as Transform).gameObject;
			DebrisInstance.SetActive(false);
			DebrisInstance.transform.parent = transform;
			
			DetectionArea.transform.localScale *= stats.Sight;
			DetectionArea.ShipDetected += ShipDetected;
			
			GUIShipStatus gui = (Instantiate(GUIPrefab) as GameObject).GetComponent<GUIShipStatus>();
			gui.ship = this;
		}
		
		private void Start() {
			if (Group != null) {
				renderer.material.SetColor("_TeamColor", this.Color);
			}
		}
		
		private void Update () {
			if(DebugConfigurations.ShowSteeringLimits) MovementEngine.DrawSteeringLimits();
		}
		
		////////////////// HULL /////////////////////
		public delegate void ShipDestroyed(Ship Ship);
		public event ShipDestroyed OnShipDestroyed;
		public float HealthPoints = -1f;
		
		public bool IsAlive() {
			return HealthPoints > 0f;
		}
		
		public float Damage(Projectile weapon) {
            float computedDamage = 0;
            if (IsAlive())
            {
                WeaponStats weaponStats = weapon.Stats;
                float heatRawDamage = weaponStats.HeatConversion * weaponStats.Damage;
                float physicalRawDamage = weaponStats.Damage - heatRawDamage;

                computedDamage = Math.Max(physicalRawDamage - stats.Armor, 0f) + heatRawDamage * (1f - stats.HeatDissipation);

                HealthPoints -= computedDamage;

                AngularImpact(weaponStats.Impact);

                if (!IsAlive())
                {
                    Die();
                }
                else
                {
                    ShipControl.EnemyAtacked(weapon.Shooter);
                }
            }
			
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
		public delegate void EnemyDetected(Ship Enemy);
		public event EnemyDetected OnEnemyDetected;
		public void ShipDetected(Ship ship) {
			ShipControl.ShipDetected(ship);
			if (OnEnemyDetected != null) OnEnemyDetected(ship);
		}

		public DetectionArea DetectionArea;
	}
}

