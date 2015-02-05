using UnityEngine;
using System.Collections;

namespace Mazzaroth {

    public class HotIronBulletStats: WeaponStats {
        public HotIronBulletStats()
        {
            Type = WeaponTypes.PlasmaBullet;
            Damage = 5;
            Cooldown = 0.3f;
            Range = 10;
            HeatConversion = 0.4f;
        }
    }
}

