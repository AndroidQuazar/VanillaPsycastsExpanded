namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Linq;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_ForceJobOnTargetBase : AbilityExtension_AbilityMod
    {
        public JobDef jobDef;

        public StatDef durationMultiplier;

        public FleckDef fleckOnTarget;

        protected void ForceJob(LocalTargetInfo target, Ability ability)
        {
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
                if (fleckOnTarget != null)
                {
                    Ability.MakeStaticFleck(pawn.DrawPos, pawn.Map, fleckOnTarget, 1f, 0f);
                }
            }
        }
    }
    public class AbilityExtension_ForceJobOnTarget : AbilityExtension_ForceJobOnTargetBase
    {
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            ForceJob(target, ability);
        }
    }

    public class AbilityExtension_ForceJobOnTargetInRadius : AbilityExtension_ForceJobOnTargetBase
    {
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            foreach (var pawn in GenRadial.RadialDistinctThingsAround(target.Cell, ability.pawn.Map, ability.GetRadiusForPawn(), true)
                .OfType<Pawn>().Where(x => x != ability.pawn && x.HostileTo(ability.pawn)))
            {
                ForceJob(pawn, ability);
            }
        }
    }
}