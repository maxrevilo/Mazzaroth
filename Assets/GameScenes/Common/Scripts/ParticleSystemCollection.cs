using UnityEngine;
using System;

namespace Mazzaroth {
    public class ParticleSystemCollection: BaseMonoBehaviour {
        public float LifeTime = 3f;

        float currentLifeTime;

        void Awake() {
            currentLifeTime = LifeTime;
        }

        void Update () {
            currentLifeTime -= Time.deltaTime;

            if (currentLifeTime <= 0f) {
                gameObject.DestroyAPS();
                currentLifeTime = LifeTime;
            }
        }
    }
}


