using UnityEngine;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class ProjectileControl : BaseMonoBehaviour
    {
        public virtual bool SetTarget(Ship ship)
        {
            return false;
        }

        //// PROTECTED ////
        protected ProjectileMovement movementEngine;
        protected Projectile projectile;

        protected void Awake()
        {
            projectile = GetComponent<Projectile>();
            movementEngine = GetComponent<ProjectileMovement>();
        }

        protected void Update() { }

        //// PRIVATE ////
    }
}
