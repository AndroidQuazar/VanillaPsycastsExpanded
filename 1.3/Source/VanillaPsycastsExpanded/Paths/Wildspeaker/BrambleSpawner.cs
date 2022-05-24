namespace VanillaPsycastsExpanded.Wildspeaker
{
    using HarmonyLib;
    using RimWorld;
    using Verse;
    using VFECore.Abilities;

    public class BrambleSpawner : TunnelHiveSpawner
    {
        private static readonly AccessTools.FieldRef<TunnelHiveSpawner, int> secondSpawnTickRef =
            AccessTools.FieldRefAccess<TunnelHiveSpawner, int>("secondarySpawnTick");

        protected override void Spawn(Map map, IntVec3 loc)
        {
            Thing thing = GenSpawn.Spawn(VPE_DefOf.Plant_Brambles, loc, map);
            if (this.TryGetComp<CompDuration>() is {durationTicksLeft: var ticks})
                Current.Game.GetComponent<GameComponent_PsycastsManager>().removeAfterTicks.Add((thing, Find.TickManager.TicksGame + ticks));
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad) secondSpawnTickRef(this) = Find.TickManager.TicksGame + 3f.SecondsToTicks();
        }
    }
}