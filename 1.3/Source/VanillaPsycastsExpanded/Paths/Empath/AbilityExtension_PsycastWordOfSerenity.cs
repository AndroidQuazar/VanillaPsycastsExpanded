namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_PsycastWordOfSerenity : AbilityExtension_Psycast
	{
		public float psyfocusCostForMinor = -1f;

		public float psyfocusCostForMajor = -1f;

		public float psyfocusCostForExtreme = -1f;

		public List<MentalStateDef> exceptions;
		public override void Cast(LocalTargetInfo target, Ability ability)
		{
			Log.Message("CAsting: " + PsyfocusCostForTarget(target) + " - " + target);
			Hediff_PsycastAbilities psycastHediff =
				(Hediff_PsycastAbilities)ability.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
			psycastHediff.UseAbility(PsyfocusCostForTarget(target), this.GetEntropyUsedByPawn(ability.pawn));
		}

		public float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			return TargetMentalBreakIntensity(target) switch
{
				MentalBreakIntensity.Minor => psyfocusCostForMinor,
				MentalBreakIntensity.Major => psyfocusCostForMajor,
				MentalBreakIntensity.Extreme => psyfocusCostForExtreme,
				_ => 0f,
			};
		}
		public MentalBreakIntensity TargetMentalBreakIntensity(LocalTargetInfo target)
		{
			MentalStateDef mentalStateDef = target.Pawn?.MentalStateDef;
			if (mentalStateDef != null)
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].mentalState == mentalStateDef)
					{
						return allDefsListForReading[i].intensity;
					}
				}
			}
			return MentalBreakIntensity.Minor;
		}

        public override bool Valid(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateHasMentalState(pawn, throwMessages))
				{
					return false;
				}
				if (exceptions.Contains(pawn.MentalStateDef))
				{
					if (throwMessages)
					{
						Messages.Message("AbilityDoesntWorkOnMentalState".Translate(ability.def.label, pawn.MentalStateDef.label), pawn, MessageTypeDefOf.RejectInput, historical: false);
					}
					return false;
				}
				float num = PsyfocusCostForTarget(target);
				if (num > ability.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
				{
					Pawn pawn2 = ability.pawn;
					if (throwMessages)
					{
						TaggedString taggedString = ("MentalBreakIntensity" + TargetMentalBreakIntensity(target)).Translate();
						Messages.Message("CommandPsycastNotEnoughPsyfocusForMentalBreak".Translate(num.ToStringPercent(), taggedString, pawn2.psychicEntropy.CurrentPsyfocus.ToStringPercent("0.#"), ability.def.label.Named("PSYCASTNAME"), pawn2.Named("CASTERNAME")), pawn, MessageTypeDefOf.RejectInput, historical: false);
					}
					return false;
				}
			}
			return true;
		}
    }
}