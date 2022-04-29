﻿namespace VanillaPsycastsExpanded.Harmonist
{
    using System;
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_RandomEvent : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            int i = 0;
            while (true)
            {
                try
                {
                    IncidentDef incident = DefDatabase<IncidentDef>.AllDefs.RandomElement();
                    if (incident.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incident.category, this.pawn.Map))) break;
                }
                catch (Exception e)
                {
                    // ignored
                }

                i++;
                if (i > 1000)
                {
                    Log.Error("[VPE] Exceeded 1000 tries to spawn random event");
                    break;
                }
            }
        }
    }
}