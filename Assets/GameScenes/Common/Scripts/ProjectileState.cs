using UnityEngine;

namespace Mazzaroth {
    public class ProjectileState : BaseMonoBehaviour {

        public bool Alive = true;
        public float BulletElongation = 1f;

        private WeaponStats stats;


        // Use this for initialization
        void Start () {
            stats = GetComponent<WeaponStats>();

            Vector3 InitialVelocity = new Vector3(0, 0, stats.Speed);
            InitialVelocity = this.transform.TransformDirection(InitialVelocity);
            this.rigidbody.velocity = InitialVelocity;

            Vector3 Scale = this.transform.localScale;
            Scale.z *= BulletElongation;
            this.transform.localScale = Scale;
        }
    }
}

