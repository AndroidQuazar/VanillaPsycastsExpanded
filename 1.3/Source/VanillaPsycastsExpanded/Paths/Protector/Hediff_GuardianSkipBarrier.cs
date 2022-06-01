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
        }
        private void AddEntropy()
        {
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                pawn.psychicEntropy.TryAddEntropy(1f, overLimit: false);
            }
            if (pawn.psychicEntropy.EntropyValue - 1f >= pawn.psychicEntropy.MaxEntropy)
            {
                this.pawn.health.RemoveHediff(this);
                //Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.PsychicComa, pawn);
                //hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = GenDate.TicksPerDay * 5;
                //pawn.health.AddHediff(hediff);
                //this.comps.Add(new HediffComp_ShouldBeDestroyed());
            }
        }
    }
}