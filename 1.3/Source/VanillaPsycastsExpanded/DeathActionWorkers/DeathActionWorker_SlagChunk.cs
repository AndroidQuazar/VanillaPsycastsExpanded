using RimWorld;
using Verse;


namespace VanillaPsycastsExpanded
{
	public class DeathActionWorker_SlagChunk : DeathActionWorker
	{
		

		public override void PawnDied(Corpse corpse)
		{
			if (corpse.Map != null)
            {
				Thing chunk = ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel);
				GenSpawn.Spawn(chunk, corpse.Position, corpse.Map);
				corpse.Destroy();
			}
		}
	}
}