namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using Verse;

    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnAt")]
    public static class PawnRenderer_RenderPawnAt_Patch
    {
        public static void Postfix(Pawn ___pawn)
        {
            foreach (var hediff in ___pawn.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_Overshield overshield)
                {
                    overshield.Draw();
                }
            }
        }
    }
}