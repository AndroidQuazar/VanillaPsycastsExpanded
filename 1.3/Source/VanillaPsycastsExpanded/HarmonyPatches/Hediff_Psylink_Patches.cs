namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(Hediff_Psylink), nameof(Hediff_Psylink.PostAdd))]
    public static class Hediff_Psylink_PostAdd
    {
        public static void Postfix(Hediff_Psylink __instance)
        {
            ((Hediff_PsycastAbilities)__instance.pawn.health.AddHediff(VPE_DefOf.VPE_PsycastAbilityImplant, __instance.Part)).InitializeFromPsylink(__instance);
        }
    }

    [HarmonyPatch(typeof(Hediff_Psylink), nameof(Hediff_Psylink.ChangeLevel), typeof(int), typeof(bool))]
    public static class Hediff_Psylink_ChangeLevel
    {
        public static void Postfix(Hediff_Psylink __instance, int levelOffset)
        {
            ((Hediff_PsycastAbilities)__instance.pawn.health.AddHediff(VPE_DefOf.VPE_PsycastAbilityImplant)).ChangeLevel(__instance.level);
        }
    }
}
