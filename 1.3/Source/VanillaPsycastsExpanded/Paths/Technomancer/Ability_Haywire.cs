namespace VanillaPsycastsExpanded.Technomancer
{
    using System.Collections.Generic;
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;

    public class Ability_AffectMechs : Ability_HediffDuration
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            foreach (Thing thing in this.AllTargetsAt(target.Cell))
            {
                this.ApplyHediffs(new GlobalTargetInfo(thing));
                if (thing.TryGetComp<CompHaywire>() is { } comp) comp.GoHaywire(this.GetDurationForPawn());
            }
        }

        public override void DrawHighlight(LocalTargetInfo target)
        {
            base.DrawHighlight(target);
            foreach (Thing thing in this.AllTargetsAt(target.Cell)) GenDraw.DrawTargetHighlight(thing);
        }

        private IEnumerable<Thing> AllTargetsAt(IntVec3 cell)
        {
            foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.pawn.Map, this.GetRadiusForPawn(), true))
            {
                if (thing is Building_Turret) yield return thing;
                if (thing is Pawn {RaceProps: {IsMechanoid: true}}) yield return thing;
            }
        }
    }
}