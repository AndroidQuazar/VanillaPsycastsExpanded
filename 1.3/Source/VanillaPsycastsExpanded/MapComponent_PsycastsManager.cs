namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using Verse;
    public class MapComponent_PsycastsManager : MapComponent
    {
        public List<FixedTemperatureZone> temperatureZones = new List<FixedTemperatureZone>();

        public List<Hediff_BlizzardSource> blizzardSources = new List<Hediff_BlizzardSource>();
        public MapComponent_PsycastsManager(Map map) : base(map)
        {

        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();
            for (var i = temperatureZones.Count - 1; i >= 0; i--)
            {
                var zone = temperatureZones[i];
                if (Find.TickManager.TicksGame >= zone.expiresIn)
                {
                    temperatureZones.RemoveAt(i);
                }
                else
                {
                    zone.DoEffects(map);
                }
            }
        }
        public bool TryGetOverridenTemperatureFor(IntVec3 cell, out float result)
        {
            foreach (var coldZone in temperatureZones)
            {
                if (cell.DistanceTo(coldZone.center) <= coldZone.radius)
                {
                    result = coldZone.fixedTemperature;
                    return true;
                }
            }
            foreach (var hediff in blizzardSources)
            {
                if (cell.DistanceTo(hediff.pawn.Position) <= hediff.ability.GetRadiusForPawn())
                {
                    result = -60; // hardcoded for now, doesn't matter much
                    return true;
                }
            }
            result = -1f;
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref temperatureZones, "temperatureZones", LookMode.Deep);
            Scribe_Collections.Look(ref blizzardSources, "blizzardSources", LookMode.Reference);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                temperatureZones ??= new List<FixedTemperatureZone>();
                blizzardSources ??= new List<Hediff_BlizzardSource>();
            }
        }
    }
}