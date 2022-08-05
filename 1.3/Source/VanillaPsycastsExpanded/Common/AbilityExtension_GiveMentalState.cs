﻿namespace VanillaPsycastsExpanded
{
    using System.Linq;
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_GiveMentalState : AbilityExtension_AbilityMod
    {
        public MentalStateDef stateDef;

        public MentalStateDef stateDefForMechs;

        public StatDef durationMultiplier;
        public bool durationScalesWithCaster;
        public bool applyToSelf;
        public bool clearOthers;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                Pawn pawn = this.applyToSelf ? ability.pawn : target.Thing as Pawn;
                if (pawn is null) continue;
                if (pawn.InMentalState)
                {
                    if (this.clearOthers) pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                    else continue;
                }

                TryGiveMentalStateWithDuration(pawn.RaceProps.IsMechanoid ? this.stateDefForMechs ?? this.stateDef : this.stateDef, 
                    pawn, ability, this.durationMultiplier, this.durationScalesWithCaster);
                RestUtility.WakeUp(pawn);
            }
        }

        public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
        {
            Pawn pawn = targets.Select(t => t.Thing).OfType<Pawn>().FirstOrDefault();
            if (pawn != null && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages)) return false;
            return true;
        }

        public static void TryGiveMentalStateWithDuration(MentalStateDef def, Pawn p, Ability ability, StatDef multiplierStat, bool durationScalesWithCaster)
        {
            if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, true, false, null, false,
                                                                   false, ability.def.GetModExtension<AbilityExtension_Psycast>() != null))
            {
                float num                       = ability.GetDurationForPawn();
                if (multiplierStat != null)
                {
                    if (durationScalesWithCaster)
                    {
                        num *= p.GetStatValue(multiplierStat);
                    }
                    else
                    {
                        num *= ability.pawn.GetStatValue(multiplierStat);
                    }
                }
                p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = (int)num;
            }
        }
    }
}