namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_BerserkPulse : Ability
    {
        public IntVec3 targetCell;
        protected override void ModifyTargets(ref GlobalTargetInfo[] targets)
        {
            targetCell = targets[0].Cell;
            base.ModifyTargets(ref targets);
        }

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            Log.Message("Ability_BerserkPulse");
            MakeStaticFleck(targetCell, pawn.Map, VPE_DefOf.PsycastAreaEffect, this.GetRadiusForPawn(), 0);
        }
    }
}