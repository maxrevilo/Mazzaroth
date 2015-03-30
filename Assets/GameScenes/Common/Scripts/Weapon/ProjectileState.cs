using UnityEngine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class ProjectileState : BaseMonoBehaviour {

        public bool Alive = true;
        public float BulletElongation = 1f;
        public Ship Shooter;
		public WeaponStats Stats;
        Vector3 initialPosition;

        public void Die() {
            gameObject.DestroyAPS();
        }

        public void Initiate(Ship shooter) {
            Stats = GetComponent<WeaponStats>();
            Shooter = shooter;

            initialPosition = this.transform.position;

            Vector3 InitialVelocity = new Vector3(0, 0, Stats.Speed);
            InitialVelocity = this.transform.TransformDirection(InitialVelocity);
            this.rigidbody.velocity = InitialVelocity;
        }

        // Use this for initialization
        void Start () {
            Vector3 Scale = this.transform.localScale;
            Scale.z *= BulletElongation;
            this.transform.localScale = Scale;
        }

        void Update () {
            if (Vector3.SqrMagnitude(initialPosition - transform.position) >= Math2d.Pow2(Stats.Range)) {
                Die();
            }
        }

        void OnTriggerEnter (Collider collider) {
            Ship ship = collider.GetComponent<Ship>();

            if (ship != null && Shooter.IsEnemy(ship)) {
                Die();
                SpawnImpactPrefab();
                ship.Damage(this);
            }
        }

        void SpawnImpactPrefab() {
            PoolingSystem pS = PoolingSystem.Instance;

            GameObject impactGameObject = pS.InstantiateAPS(
                Stats.ImpactPrefab.name,
                transform.position,
                transform.rotation,
                transform.parent.gameObject
            );

            impactGameObject.rigidbody.velocity = rigidbody.velocity;
        }
    }
}

