using UnityEngine;
using System.Collections;

namespace Mazzaroth {

    /*
     * The base class of all Ships Stats.
     */
    public class ShipStats : BaseMonoBehaviour {

        // The max ammount of Health Points of the ship.
        public int HealthPoints;
        // The armor of the Ship. Each point will be substracted from the damage taken.
        public float Armor;
        // The factor of dissipation of heat (The rest will be converted on damage).
        public float HeatDissipation;
        // The amount of space the ship occupy on the carrier
        // Each cargo unit is equiavlent to the smallest ship volume.
        public int Cargo;
        // Size in meters.
        public float Size;
        // The max speed of the ship in m/s.
        public float Speed;
        // The sight distance in meters
        public int Sight;
        // The array of weapons of the ship
        public WeaponStats[] Weapons;

        // Less user friendly stats
        // The acceleration of the ship in m/s^2
        public float acceleration = 10f;
        // The deacceleration of the ship in m/s^2
        public float deacceleration = 20f;
        // The max rotational speed of the ship in rev/s
        public float angularSpeed = 0.6f;
        // The angular acceleration of the ship in rev/s^2
        public float angularAcceleration = 1f;

//        public float CooldownBetweenWeapons = 0f;

        // The prefab that will be instanced whit this stats.
        public Transform ShipPrefab;
        public Transform DebrisPrefab;

        private const float REV2RAD = 2f * Mathf.PI;

        public float angularAccelerationRad { get { return angularAcceleration * REV2RAD; } }
        public float angularSpeedRad { get { return angularSpeed * REV2RAD; } }
    }

}