namespace VanillaPsycastsExpanded.Wildspeaker
{
    using System.Collections.Generic;
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
            if (!this.CheckSpawnLoc(this.Position, map)) this.Destroy();
        }

        protected virtual bool CheckSpawnLoc(IntVec3 loc, Map map)
        {
            if (loc.GetTerrain(map).fertility == 0f) return false;
            List<Thing> list = loc.GetThingList(map);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Thing thing = list[i];
                if (thing is Plant)
                {
                    if (thing.def.plant.IsTree) return false;
                    thing.Destroy();
                }

                if (thing.def.IsEdifice()) return false;
            }

            return true;
        }
    }
}