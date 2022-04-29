namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using Verse;

    [HarmonyPatch(typeof(StatExtension), nameof(StatExtension.GetStatValue))]
    public static class StatExtension_GetStatValue_Patch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Thing thing, StatDef stat, bool applyPostProcess, ref float __result)
        {
            if (stat == StatDefOf.RangedWeapon_Cooldown && thing?.ParentHolder is Pawn_EquipmentTracker eq)
            {
                __result /= eq.pawn.GetStatValue(VPE_DefOf.VPE_RangeAttackSpeedFactor);
            }
        }
    }
}