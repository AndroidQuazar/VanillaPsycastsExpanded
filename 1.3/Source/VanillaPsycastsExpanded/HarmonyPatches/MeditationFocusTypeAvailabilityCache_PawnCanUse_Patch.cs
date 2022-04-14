namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
    public static class MeditationFocusTypeAvailabilityCache_PawnCanUse_Patch
    {
        public static bool Prefix(Pawn p, MeditationFocusDef type, ref bool __result)
        {
            __result = (p.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant) as Hediff_PsycastAbilities)?.unlockedMeditationFoci.Contains(type) ?? false;
            return false;
        }
    }
}