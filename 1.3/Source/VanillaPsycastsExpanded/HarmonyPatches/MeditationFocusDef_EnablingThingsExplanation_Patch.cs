namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(MeditationFocusDef), nameof(MeditationFocusDef.EnablingThingsExplanation))]
    public static class MeditationFocusDef_EnablingThingsExplanation_Patch
    {
        public static bool Prefix(ref string __result)
        {
            __result = "  - " + "VPE.UnlockedByPoints".Translate();
            return false;
        }
    }
}