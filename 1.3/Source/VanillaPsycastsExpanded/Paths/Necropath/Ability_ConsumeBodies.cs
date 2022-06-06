namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using Verse.AI;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_ConsumeBodies : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (!pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_BodiesConsumed))
            {
                pawn.health.AddHediff(VPE_DefOf.VPE_BodiesConsumed);
            }
        }
        public override Hediff ApplyHediff(Pawn targetPawn, HediffDef hediffDef, BodyPartRecord bodyPart, int duration, float severity)
        {
            var hediff = base.ApplyHediff(targetPawn, hediffDef, bodyPart, duration, severity) as Hediff_BodyConsumption;
            hediff.consumer = this.pawn;
            return hediff;
        }
    }
}