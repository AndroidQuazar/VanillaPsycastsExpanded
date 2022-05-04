namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using Verse;
    public class MapComponent_PsycastsManager : MapComponent
    {
        public List<FixedTemperatureZone> temperatureZones = new List<FixedTemperatureZone>();
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
            result = -1f;
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref temperatureZones, "temperatureZones", LookMode.Deep);
        }
    }
}