using UnityEngine;
using System.Collections;

namespace Mazzaroth {

    public class WeaponStats : BaseMonoBehaviour {
        public enum WeaponTypes {
            PlasmaBullet,
            KineticMissile,
            HeatLaser,
        }

        public WeaponTypes Type;
        // The amount of hitpoints 
        public int Damage;
        // The amount of seconds to fire again.
        public float Cooldown;
        // The max distance of the target to engange.
        public int Range;
        // The factor of conversion from damage to heat.
        public float HeatConversion;


        // Projectile Speed in m/s.
        public float Speed = 100;
        //
        public float Impact = 0.3f;

        // The prefab that will be instanced whit this stats.
        public Transform BulletPrefab;
        public Transform ImpactPrefab;
    }

}