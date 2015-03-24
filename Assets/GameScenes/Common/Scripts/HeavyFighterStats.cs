using UnityEngine;
using System.Collections;

namespace Mazzaroth {
	
	public class HeavyFighterStats: ShipStats {
		public HeavyFighterStats()
		{
			HealthPoints = 200;
			Armor = 2;
			HeatDissipation = 0.3f;
			Cargo = 4;
			Size = 6;
			Speed = 5;
			Sight = 50;
			//            CooldownBetweenWeapons = 0.1f;
		}
	}
}

