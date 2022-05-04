namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System;
    using System.Security.Cryptography;
    using Verse;
    using VFECore.Abilities;
    using static HarmonyLib.Code;
    using Ability = VFECore.Abilities.Ability;
    public class AbilityExtension_WordOfLove : AbilityExtension_AbilityMod
	{
		public TargetingParameters targetParams => new TargetingParameters
		{
			canTargetSelf = true,
			canTargetBuildings = false,
			canTargetAnimals = false,
			canTargetMechs = false,
			canTargetHumans = true
		};

        public override void PreCast(LocalTargetInfo target, Ability ability, ref bool startAbilityJobImmediately, Action startJobAction)
        {
			startAbilityJobImmediately = false;
			Log.Message("PRECAST: " + target.Thing);
			Find.Targeter.StopTargeting();
			Find.Targeter.BeginTargeting(targetParams, delegate (LocalTargetInfo dest)
			{
				Log.Message("STARTED 1: " + dest.Thing);
				Find.Targeter.BeginTargeting(targetParams, delegate (LocalTargetInfo dest)
				{
					Log.Message("STARTED 2: " + dest.Thing);
					if (ValidateTarget(dest, ability))
					{
						ability.selectedTarget = dest;
						ability.StartAbilityJob(target);
						Log.Message("STARTING JOB, SELECTED TARGET: " + ability.selectedTarget.Thing);
					}
				}, ability.pawn);
			});

			Find.Targeter.targetingSourceAdditionalPawns = new System.Collections.Generic.List<Pawn>();
		}
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            		base.Cast(target, ability);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_PsychicLove hediff_PsychicLove = (Hediff_PsychicLove)HediffMaker.MakeHediff(HediffDefOf.PsychicLove, pawn, pawn.health.hediffSet.GetBrain());
			hediff_PsychicLove.target = ability.selectedTarget.Thing;
			HediffComp_Disappears hediffComp_Disappears = hediff_PsychicLove.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				float num = ability.GetDurationForPawn();
				num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity);
				hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
			}
			pawn.health.AddHediff(hediff_PsychicLove);
			Log.Message("Added hediff to " + pawn + " - " + hediff_PsychicLove + " - target: " + ability.selectedTarget.Thing);
		}
		public bool ValidateTarget(LocalTargetInfo target, Ability ability, bool showMessages = true)
		{
			Pawn pawn = ability.selectedTarget.Pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn == pawn2)
			{
				return false;
			}
			if (pawn != null && pawn2 != null && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				Gender gender = pawn.gender;
				Gender gender2 = (pawn.story.traits.HasTrait(TraitDefOf.Gay) ? gender : gender.Opposite());
				if (pawn2.gender != gender2)
				{
					if (showMessages)
                    {
						Messages.Message("AbilityCantApplyWrongAttractionGender".Translate(pawn, pawn2), pawn, MessageTypeDefOf.RejectInput, historical: false);
					}
					return false;
				}
			}
			return true;
		}

        public override bool Valid(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					if (throwMessages)
                    {
						Messages.Message("AbilityCantApplyOnAsexual".Translate(ability.def.label), pawn, MessageTypeDefOf.RejectInput, historical: false);
					}
					return false;
				}
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages))
				{
					return false;
				}
			}
			return true;
        }

		public override string ExtraLabelMouseAttachment(LocalTargetInfo target, Ability ability)
		{
			if (ability.selectedTarget.IsValid)
			{
				return "PsychicLoveFor".Translate();
			}
			return "PsychicLoveInduceIn".Translate();
		}
	}
}