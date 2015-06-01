using UnityEngine;

namespace Mazzaroth {
    public class ProjectileMovement : BaseMonoBehaviour
    {

        protected Projectile projectile;
        protected WeaponStats weaponStats;

        public void Initiate()
        {
            Debug.Log("HAAA");
            projectile = GetComponent<Projectile>();
            weaponStats = GetComponent<WeaponStats>();

            Vector3 InitialVelocity = new Vector3(0, 0, weaponStats.Speed);
            InitialVelocity = projectile.transform.TransformDirection(InitialVelocity);
            projectile.rigidbody.velocity = InitialVelocity;
        }
    }
}
