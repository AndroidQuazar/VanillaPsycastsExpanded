namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using System.Linq;
    using Verse;
    using VFECore.Abilities;

    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Pawn_Kill_Patch
    {
        private static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (dinfo.HasValue && dinfo.Value.Instigator is Pawn attacker)
            {
                var hediff = attacker.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_ControlledFrenzy) as Hediff_Ability;
                if (hediff != null)
                {
                    attacker.psychicEntropy.TryAddEntropy(-10f);
                    hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = hediff.ability.GetDurationForPawn();
                }
            }
        }
    }
}