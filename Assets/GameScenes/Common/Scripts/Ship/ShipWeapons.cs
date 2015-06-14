using UnityEngine;

namespace Mazzaroth.Ships
{
    public class ShipWeapons : BaseMonoBehaviour
    {
        public float TimeToFire;
        public Transform[] FiringLocations;

        public bool Fire(Ship target, WeaponStats weapon = null)
        {
            if (weapon == null)
                weapon = getWeaponAt(0);

            if (!IsReadyToFire() || !IsTargetInRange(target.transform, weapon))
            {
                return false;
            }

            PoolingSystem pS = PoolingSystem.Instance;
            Transform firingLocation = getActualFiringLocation(true);


            GameObject projectileEntity = pS.InstantiateAPS(
                weapon.BulletPrefab.name,
                firingLocation.position,
                firingLocation.rotation,
                this.transform.parent.gameObject
                );

            Projectile projectile = projectileEntity.GetComponent<Projectile>();
            projectile.Initiate(ship);
            projectile.ProjectileControl.SetTarget(target);

            TimeToFire = weapon.Cooldown;

            return true;
        }

        public bool IsReadyToFire()
        {
            return ship.IsAlive() && TimeToFire <= 0f;
        }

        public bool IsTargetInRange(Transform target, WeaponStats weapon = null)
        {
            if (weapon == null)
                weapon = getWeaponAt(0);

            float distSqr = Vector3.SqrMagnitude(this.transform.position - target.position);

            return distSqr <= Mathf.Pow(weapon.Range, 2);
        }

        //// PROTECTED ////
        protected Ship ship;
        protected ShipStats stats;
        protected int actualFiringLocation;

        //// PRIVATE ////
        private void Awake()
        {
            ship = GetComponent<Ship>();
            stats = GetComponent<ShipStats>();
            TimeToFire = 0f;
        }

        private void Update()
        {
            TimeToFire -= Time.deltaTime;
        }

        WeaponStats getWeaponAt(int index)
        {
            return stats.Weapons[index];
        }

        Transform getActualFiringLocation(bool MoveToTheNextFiringLocation = false)
        {
            Transform firingLocation = FiringLocations[actualFiringLocation];

            if (MoveToTheNextFiringLocation)
            {
                actualFiringLocation++;
                if (actualFiringLocation >= FiringLocations.Length)
                {
                    actualFiringLocation = 0;
                }
            }

            return firingLocation;
        }
    }
}
