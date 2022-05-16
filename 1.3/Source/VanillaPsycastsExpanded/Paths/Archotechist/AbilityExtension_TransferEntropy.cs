namespace VanillaPsycastsExpanded
{
using Mono.Unix.Native;
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
using static UnityEngine.GraphicsBuffer;
    using Ability = VFECore.Abilities.Ability;
	public class AbilityExtension_TransferEntropy : AbilityExtension_AbilityMod
	{
		public bool targetReceivesEntropy = true;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
			Pawn pawn = targets[0].Thing as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = ability.pawn;
				if (targetReceivesEntropy)
				{
					pawn.psychicEntropy.TryAddEntropy(pawn2.psychicEntropy.EntropyValue, pawn2, scale: false, overLimit: true);
				}
				pawn2.psychicEntropy.RemoveAllEntropy();
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, ability.pawn, pawn);
			}
			else
			{
				Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.");
			}
		}

        public override bool IsEnabledForPawn(Ability ability, out string reason)
		{
			if (ability.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return false;
			}
			return base.IsEnabledForPawn(ability, out reason);

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
	}
}