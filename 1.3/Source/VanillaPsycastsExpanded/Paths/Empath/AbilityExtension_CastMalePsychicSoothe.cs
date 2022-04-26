﻿namespace VanillaPsycastsExpanded
{
using Mono.Unix.Native;
    using RimWorld;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
	public class AbilityExtension_CastPsychicSoothe : AbilityExtension_AbilityMod
	{
		public Gender gender;
        public override void Cast(LocalTargetInfo target, Ability ability)
{
			foreach (Pawn pawn in ability.pawn.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (!pawn.Dead && pawn.gender == gender && pawn.needs != null && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost));
				}
			}
		}
	}
}