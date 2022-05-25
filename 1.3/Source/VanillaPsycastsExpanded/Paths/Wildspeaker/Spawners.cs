namespace VanillaPsycastsExpanded.Wildspeaker
{
    using System.Collections.Generic;
    using System.Linq;
    using HarmonyLib;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public abstract class PlantSpawner : TunnelHiveSpawner
    {
        private static readonly AccessTools.FieldRef<TunnelHiveSpawner, int> secondSpawnTickRef =
            AccessTools.FieldRefAccess<TunnelHiveSpawner, int>("secondarySpawnTick");

        protected override void Spawn(Map map, IntVec3 loc)
        {
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad) secondSpawnTickRef(this) = Find.TickManager.TicksGame + this.DurationTicks();
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

        protected virtual int DurationTicks() => 3f.SecondsToTicks();
    }

    public class BrambleSpawner : PlantSpawner
    {
        protected override void Spawn(Map map, IntVec3 loc)
        {
            base.Spawn(map, loc);
            Thing thing = GenSpawn.Spawn(VPE_DefOf.Plant_Brambles, loc, map);
            if (this.TryGetComp<CompDuration>() is {durationTicksLeft: var ticks})
                Current.Game.GetComponent<GameComponent_PsycastsManager>().removeAfterTicks.Add((thing, Find.TickManager.TicksGame + ticks));
        }
    }

    public class WildPlantSpawner : PlantSpawner
    {
        protected override void Spawn(Map map, IntVec3 loc)
        {
            base.Spawn(map, loc);
            Rand.PushState(Find.TickManager.TicksAbs);
            if (DefDatabase<ThingDef>.AllDefs.Where(td => td.plant is {Sowable: true}).TryRandomElement(out ThingDef plantDef))
                if (plantDef.CanEverPlantAt(loc, map, true))
                {
                    Plant plant = (Plant) ThingMaker.MakeThing(plantDef);
                    plant.Growth = Mathf.Clamp(this.GetComp<CompAbilitySpawn>().pawn.GetStatValue(StatDefOf.PsychicSensitivity) - 1f, 0.1f, 1f);
                    GenSpawn.Spawn(plant, loc, map);
                }

            Rand.PopState();
        }
    }

    public class TreeSpawner : PlantSpawner
    {
        protected override void Spawn(Map map, IntVec3 loc)
        {
            base.Spawn(map, loc);
            Rand.PushState(Find.TickManager.TicksAbs);
            if (map.Biome.AllWildPlants.Where(td => td.plant is {IsTree: true}).TryRandomElement(out ThingDef plantDef) ||
                map.Biome.AllWildPlants.TryRandomElement(out plantDef))
                if (plantDef.CanEverPlantAt(loc, map, true))
                {
                    Plant plant = (Plant) ThingMaker.MakeThing(plantDef);
                    plant.Growth = 1f;
                    GenSpawn.Spawn(plant, loc, map);
                }

            Rand.PopState();
        }

        protected override int DurationTicks() => 5f.SecondsToTicks();
    }
}