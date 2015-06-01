using UnityEngine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class Projectile : BaseMonoBehaviour {

        public bool Alive = true;
        public float BulletElongation = 1f;
        public Ship Shooter;
        public ProjectileControl ProjectileControl { get; private set; }
        public ProjectileMovement ProjectileMovement { get; private set; }
        public WeaponStats Stats { get; private set; }
        
        //// PRIVATE ////
        private Vector3 initialPosition;

        public void Die() {
            gameObject.DestroyAPS();
        }

        public void Initiate(Ship shooter) {
            Stats = GetComponent<WeaponStats>();
            ProjectileControl = GetComponent<ProjectileControl>();
            ProjectileMovement = GetComponent<ProjectileMovement>();
            Shooter = shooter;

            initialPosition = this.transform.position;

            ProjectileMovement.Initiate();
        }

        // Use this for initialization
        private void Start()
        {
            Vector3 Scale = this.transform.localScale;
            Scale.z *= BulletElongation;
            this.transform.localScale = Scale;
        }

        private void Update()
        {
            if (Vector3.SqrMagnitude(initialPosition - transform.position) >= Math2d.Pow2(Stats.Range)) {
                Die();
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            Ship ship = collider.GetComponent<Ship>();

            if (ship != null && Shooter.IsEnemy(ship)) {
                Die();
                SpawnImpactPrefab();
                ship.Damage(this);
            }
        }

        private void SpawnImpactPrefab()
        {
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

