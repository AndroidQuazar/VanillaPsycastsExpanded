namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;

    public class Hediff_PsychicPain : HediffWithComps
    {
        public override float PainOffset => pawn.GetStatValue(StatDefOf.PsychicSensitivity);
    }
}