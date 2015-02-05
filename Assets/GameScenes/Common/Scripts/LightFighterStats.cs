using UnityEngine;
using System.Collections;

namespace Mazzaroth {

    public class LightFighterStats: ShipStats {
        public LightFighterStats()
        {
            HealthPoints = 35;
            Armor = 0;
            HeatDissipation = 0;
            Cargo = 1;
            Size = 4;
            Speed = 10;
            Sight = 20;
//            CooldownBetweenWeapons = 0.1f;
        }
    }
}

