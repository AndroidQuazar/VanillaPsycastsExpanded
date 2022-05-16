namespace VanillaPsycastsExpanded
{
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

		public bool applyToSelf;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
			foreach (var target in targets)
            {
				Pawn pawn = (applyToSelf ? ability.pawn : (target.Thing as Pawn));
				if (pawn != null && !pawn.InMentalState)
				{
					TryGiveMentalStateWithDuration(pawn.RaceProps.IsMechanoid ? (stateDefForMechs ?? stateDef) : stateDef, pawn, ability, durationMultiplier);
					RestUtility.WakeUp(pawn);
				}
			}
        }

		public override bool Valid(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages))
			{
				return false;
			}
			return true;
		}

		public static void TryGiveMentalStateWithDuration(MentalStateDef def, Pawn p, Ability ability, StatDef multiplierStat)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, forceWake: true, causedByMood: false, null, transitionSilently: false, 
				causedByDamage: false, ability.def.GetModExtension<AbilityExtension_Psycast>() != null))
			{
				float num = ability.GetDurationForPawn();
				if (multiplierStat != null)
				{
					num *= p.GetStatValue(multiplierStat);
				}
				p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
			}
		}
	}
}