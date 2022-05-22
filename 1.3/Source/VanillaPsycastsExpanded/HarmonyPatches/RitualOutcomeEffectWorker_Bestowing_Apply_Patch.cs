namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_Bestowing), nameof(RitualOutcomeEffectWorker_Bestowing.Apply))]
    public class RitualOutcomeEffectWorker_Bestowing_Apply_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo            info1 = AccessTools.Method(typeof(Thing),   nameof(Thing.Destroy));
            MethodInfo            info2 = AccessTools.Method(typeof(History), nameof(History.Notify_PsylinkAvailable));
            int                   idx1  = codes.FindIndex(ins => ins.Calls(info1)) + 1;
            codes.RemoveRange(idx1, 4);
            int idx3 = codes.FindIndex(ins => ins.Calls(info2)) + 1;
            int idx4 = codes.FindIndex(idx3, ins => ins.opcode == OpCodes.Blt_S);
            codes.RemoveRange(idx3, idx4 - idx3 + 1);
            return codes;
        }
    }
}