namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
    public static class Pawn_HealthTracker_MakeDowned_Patch
    {
        private static void Postfix(Pawn ___pawn, DamageInfo? dinfo, Hediff hediff)
        {
            if (___pawn.Downed)
            {
                var hediffs = ___pawn.health.hediffSet?.hediffs;
                for (var i = hediffs.Count - 1; i >= 0; i--)
                {
                    var hediffEntry = hediffs[i];
                    if (hediffEntry.TryGetComp<HediffComp_DisappearsOnDowned>() != null)
                    {
                        ___pawn.health.RemoveHediff(hediffEntry);
                    }
                }
            }
        }
    }
}