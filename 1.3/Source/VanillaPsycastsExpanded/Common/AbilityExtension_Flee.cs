namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using Verse.AI;
    using Verse.AI.Group;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_Flee : AbilityExtension_AbilityMod
    {
        public bool onlyHostile = true;

        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            Pawn pawn = target.Pawn;
            if (!this.onlyHostile || !pawn.HostileTo(ability.pawn)) return;
            pawn.GetLord()?.RemovePawn(pawn);
            if (pawn.mindState != null) pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest) {locomotion = LocomotionUrgency.Jog, canDig = false};
            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
        }
    }
}