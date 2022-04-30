namespace VanillaPsycastsExpanded
{
    using Verse;
    using Verse.Sound;
    public class HediffCompProperties_PlaySustainer : HediffCompProperties
    {
        public SoundDef sustainer;
        public HediffCompProperties_PlaySustainer()
        {
            compClass = typeof(HediffComp_PlaySustainer);
        }
    }
    public class HediffComp_PlaySustainer : HediffComp
    {
        private Sustainer sustainer;
        public HediffCompProperties_PlaySustainer Props => (HediffCompProperties_PlaySustainer)props;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (sustainer == null || sustainer.Ended)
            {
                sustainer = Props.sustainer.TrySpawnSustainer(SoundInfo.InMap(Pawn));
            }
            sustainer.Maintain();
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (!sustainer.Ended)
            {
                sustainer?.End();
            }
        }
    }
}