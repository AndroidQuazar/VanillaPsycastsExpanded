namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using AnimalBehaviours;
    using UnityEngine;
    using Verse;

    public class CompBreakLink : ThingComp, PawnGizmoProvider
    {
        public Pawn                     Pawn;
        public CompProperties_BreakLink Props => this.props as CompProperties_BreakLink;

        public IEnumerable<Gizmo> GetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = this.Props.gizmoLabel.Translate(),
                defaultDesc  = this.Props.gizmoDesc.Translate(),
                icon         = ContentFinder<Texture2D>.Get(this.Props.gizmoImage),
                action = delegate
                {
                    this.parent.Kill();
                    this.Pawn.Psycasts()?.OffsetMinHeat(-20f);
                }
            };
        }
    }
}