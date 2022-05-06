namespace VanillaPsycastsExpanded
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;
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
            if (offset > 0f) __instance.Pawn.Psycasts()?.GainExperience(offset * 100f);
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

    [HarmonyPatch]
    public static class MinHeatPatches
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            Type type = typeof(Pawn_PsychicEntropyTracker);
            yield return AccessTools.Method(type, nameof(Pawn_PsychicEntropyTracker.TryAddEntropy));
            yield return AccessTools.Method(type, nameof(Pawn_PsychicEntropyTracker.PsychicEntropyTrackerTick));
            yield return AccessTools.Method(type, nameof(Pawn_PsychicEntropyTracker.RemoveAllEntropy));
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (CodeInstruction instruction in instructions)
                if (!found && instruction.opcode == OpCodes.Ldc_R4 && instruction.operand is 0f)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld,  AccessTools.Field(typeof(Pawn_PsychicEntropyTracker), "pawn"));
                    yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(VPE_DefOf), nameof(VPE_DefOf.VPE_PsychicEntropyMinimum)));
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StatExtension), nameof(StatExtension.GetStatValue)));
                }
                else yield return instruction;
        }
    }
}