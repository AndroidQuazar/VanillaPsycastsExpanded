namespace VanillaPsycastsExpanded
{
    using Verse;

    public static class PsycastUtility
    {
        public static Hediff_PsycastAbilities Psycasts(this Pawn pawn) =>
            (Hediff_PsycastAbilities) pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
    }
}