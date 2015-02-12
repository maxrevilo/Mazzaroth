using UnityEngine;

namespace Mazzaroth {
    public class ProjectileState : BaseMonoBehaviour {

        public bool Alive = true;
        public float BulletElongation = 1f;
        public ShipState Breader;

        WeaponStats stats;
        Vector3 initialPosition;

        public void Die() {
            gameObject.DestroyAPS();
        }

        public void Initiate(ShipState breader) {
            stats = GetComponent<WeaponStats>();
            Breader = breader;

            initialPosition = this.transform.position;

            Vector3 InitialVelocity = new Vector3(0, 0, stats.Speed);
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
            if (Vector3.SqrMagnitude(initialPosition - transform.position) >= Math2d.Pow2(stats.Range)) {
                Die();
            }
        }

        void OnTriggerEnter (Collider collider) {
            GameObject gameObject = collider.gameObject;
            ShipState ship = collider.GetComponent<ShipState>();

            if (ship != null && gameObject != Breader.gameObject) {
                Die();
                SpawnImpactPrefab();
                ship.Damage(stats);
            }
        }

        void SpawnImpactPrefab() {
            PoolingSystem pS = PoolingSystem.Instance;

            GameObject impactGameObject = pS.InstantiateAPS(
                stats.ImpactPrefab.name,
                transform.position,
                transform.rotation,
                transform.parent.gameObject
            );

            impactGameObject.rigidbody.velocity = rigidbody.velocity;
        }
    }
}

