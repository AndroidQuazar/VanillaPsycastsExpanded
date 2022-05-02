using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace VanillaPsycastsExpanded
{


    public class CompBreakLink : ThingComp, AnimalBehaviours.PawnGizmoProvider
    {
        public CompProperties_BreakLink Props => base.props as CompProperties_BreakLink;


        public IEnumerable<Gizmo> GetGizmos()
        {

            

                yield return new Command_Action
                {
                    defaultLabel = Props.gizmoLabel.Translate(),
                    defaultDesc = Props.gizmoDesc.Translate(),
                    icon = ContentFinder<Texture2D>.Get(Props.gizmoImage, true),
                    action = delegate
                    {
                        this.parent.Kill(null);
                    }
                };
            
        }

       
    }


}
