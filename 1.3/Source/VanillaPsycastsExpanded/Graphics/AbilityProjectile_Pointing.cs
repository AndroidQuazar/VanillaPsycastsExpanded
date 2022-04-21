namespace VanillaPsycastsExpanded.Graphics
{
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class AbilityProjectile_Pointing : AbilityProjectile
    {
        private Vector3 lastPos;
        private Vector3 curPos;

        public override Quaternion ExactRotation => Quaternion.LookRotation(this.lastPos, this.curPos);

        public override void Tick()
        {
            this.lastPos = this.ExactPosition.Yto0();
            base.Tick();
            this.curPos = this.ExactPosition.Yto0();
        }
    }
}