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

    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), nameof(Pawn_PsychicEntropyTracker.GainPsyfocus))]
    public static class Pawn_EntropyTracker_GainPsyfocus_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn_PsychicEntropyTracker __instance, Thing focus)
        {
            __instance.Pawn.Psycasts()?.GainExperience(MeditationUtility.PsyfocusGainPerTick(__instance.Pawn, focus) * 100f);
        }
    }

    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), nameof(Pawn_PsychicEntropyTracker.OffsetPsyfocusDirectly))]
    public static class Pawn_EntropyTracker_OffsetPsyfocusDirectly_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn_PsychicEntropyTracker __instance, float offset)
        {
            __instance.Pawn.Psycasts()?.GainExperience(offset * 100f);
        }
    }

    [HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), nameof(Pawn_PsychicEntropyTracker.RechargePsyfocus))]
    public static class Pawn_EntropyTracker_RechargePsyfocus_Postfix
    {
        [HarmonyPrefix]
        public static void Prefix(Pawn_PsychicEntropyTracker __instance)
        {
            __instance.Pawn.Psycasts()?.GainExperience((1f - __instance.CurrentPsyfocus) * 100f);
        }
    }
}