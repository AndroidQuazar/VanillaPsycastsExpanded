namespace VanillaPsycastsExpanded
{
using Mono.Unix.Native;
    using RimWorld;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
	public class AbilityExtension_CastMalePsychicSoothe : AbilityExtension_AbilityMod
	{
        public override void Cast(LocalTargetInfo target, Ability ability)
{
			foreach (Pawn pawn in ability.pawn.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (!pawn.Dead && pawn.gender == Gender.Male && pawn.needs != null && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost));
				}
			}
		}
	}
}