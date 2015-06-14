using UnityEngine;
using BehaviourMachine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class ProjectileControl : BaseMonoBehaviour
    {
        public bool SetTarget(Ship ship)
        {
            return false;
        }

        //// PROTECTED ////
        protected Blackboard blackboard;
        protected ProjectileMovement movementEngine;
        protected Projectile projectile;

        //// PRIVATE ////
        private void Awake()
        {
            projectile = GetComponent<Projectile>();
            movementEngine = GetComponent<ProjectileMovement>();
            blackboard = GetComponent<Blackboard>();
        }
    }
}
