using UnityEngine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class MissileControl: ProjectileControl
    {
        public Ship Target;

        public override bool SetTarget(Ship target)
        {
            this.Target = target;

            return true;
        }

        // PROTECTED

        protected void Update() {
            base.Update();

            projectile.ProjectileMovement.MoveForward();
        }
    }
}