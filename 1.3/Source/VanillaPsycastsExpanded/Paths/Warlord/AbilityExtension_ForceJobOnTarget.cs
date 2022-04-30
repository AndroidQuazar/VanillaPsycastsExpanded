namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class AbilityExtension_ForceJobOnTarget : AbilityExtension_AbilityMod
    {
        public JobDef jobDef;

        public StatDef durationMultiplier;
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            Pawn pawn = target.Thing as Pawn;
            if (pawn != null)
            {
                Job job = JobMaker.MakeJob(jobDef, ability.pawn);
                float num = 1f;
                if (durationMultiplier != null)
                {
                    num = pawn.GetStatValue(durationMultiplier);
                }
                job.expiryInterval = (int)(ability.GetDurationForPawn() * num);
                job.mote = MoteMaker.MakeThoughtBubble(pawn, ability.def.iconPath, maintain: true);
                pawn.jobs.StopAll();
                pawn.jobs.StartJob(job, JobCondition.InterruptForced);
            }
        }
    }
}