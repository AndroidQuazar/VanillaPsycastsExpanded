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
        public override void Tick()
        {
            base.Tick();
            AddEntropy();
            if (sustainer == null || sustainer.Ended)
            {
                sustainer = VPE_DefOf.VPE_GuardianSkipbarrier_Sustainer.TrySpawnSustainer(SoundInfo.InMap(pawn));
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
            pawn.psychicEntropy.TryAddEntropy(1f, overLimit: true);
            if (pawn.psychicEntropy.EntropyValue >= pawn.psychicEntropy.MaxEntropy)
            {
                Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.PsychicComa, pawn);
                hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = GenDate.TicksPerDay * 5;
                pawn.health.AddHediff(hediff);
                this.comps.Add(new HediffComp_ShouldBeDestroyed());
            }
        }
    }
}