namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using System;
    using Verse;

    [HarmonyPatch(typeof(Tool), "AdjustedCooldown", new Type[] { typeof(Thing) })]
    public static class Tool_AdjustedCooldown_Patch
    {
        public static void Postfix(Thing ownerEquipment, ref float __result)
        {
            if (ownerEquipment?.ParentHolder is Pawn_EquipmentTracker eq)
            {
                __result /= eq.pawn.GetStatValue(VPE_DefOf.VPE_MeleeAttackSpeedFactor);
            }
        }
    }
}