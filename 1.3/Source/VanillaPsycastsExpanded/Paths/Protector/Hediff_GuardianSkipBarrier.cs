namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;

    public class Hediff_GuardianSkipBarrier : Hediff_Overshield
    {
        private Sustainer sustainer;
        public override Color OverlayColor => new ColorInt(79, 141, 247).ToColor;
        public override float OverlaySize => 9;
        protected override void DestroyProjectile(Projectile projectile)
        {
            base.DestroyProjectile(projectile);
            AddEntropy();
        }
        public override void PostTick()
        {
            base.PostTick();
            AddEntropy();
            if (sustainer == null || sustainer.Ended)
            {
                sustainer = VPE_DefOf.VPE_GuardianSkipbarrier_Sustainer.TrySpawnSustainer(SoundInfo.InMap(pawn, MaintenanceType.PerTick));
            }
            sustainer.Maintain();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!sustainer.Ended)
            {
                sustainer?.End();
            }
            if (pawn.Psycasts().currentlyChanneling == this.ability)
            {
                pawn.Psycasts().currentlyChanneling = null;
            }
        }
        private void AddEntropy()
        {
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                pawn.psychicEntropy.TryAddEntropy(1f, overLimit: true);
            }
            if (pawn.psychicEntropy.EntropyValue >= pawn.psychicEntropy.MaxEntropy)
            {
                this.pawn.health.RemoveHediff(this);
            }
        }
    }
}