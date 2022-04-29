namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
using UnityEngine.Networking;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_GuardianSkipBarrier : Ability_HediffDuration
    {
        public override Gizmo GetGizmo()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_GuardianSkipBarrier);
            if (hediff != null)
            {
                return new Command_Action
                {
                    defaultLabel = "VPE.CancelSkipbarrier".Translate(),
                    defaultDesc = "VPE.CancelSkipbarrierDesc".Translate(),
                    icon = this.def.icon,
                    action = delegate
                    {
                        pawn.health.RemoveHediff(hediff);
                    },
                    order = 10f + (this.def.requiredHediff?.hediffDef?.index ?? 0) + (this.def.requiredHediff?.minimumLevel ?? 0)
                };
            }
            else
            {
                return base.GetGizmo();
            }
        }
    }
}