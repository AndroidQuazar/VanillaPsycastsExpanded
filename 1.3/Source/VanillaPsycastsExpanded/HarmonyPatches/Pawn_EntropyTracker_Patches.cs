namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using UI;
    using Verse;

    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), nameof(Pawn_PsychicEntropyTracker.GetGizmo))]
    public static class Pawn_EntropyTracker_GetGizmo_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix(Pawn_PsychicEntropyTracker __instance, ref Gizmo ___gizmo)
        {
            ___gizmo ??= new PsychicStatusGizmo(__instance);
        }
    }
}