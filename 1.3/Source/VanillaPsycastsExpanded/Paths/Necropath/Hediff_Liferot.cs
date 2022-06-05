namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;

    public class Hediff_Liferot : HediffWithComps
    {
        public override void Tick()
        {
            base.Tick();
            if (this.pawn.IsHashIntervalTick(60) && this.pawn.health.hediffSet.GetNotMissingParts().TryRandomElement(out var part))
            {
                this.pawn.TakeDamage(new DamageInfo(VPE_DefOf.VPE_Rot, 99999, hitPart: part));
            }
        }
    }
}