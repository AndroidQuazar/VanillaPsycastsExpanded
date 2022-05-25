namespace VanillaPsycastsExpanded.Wildspeaker
{
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class WildPlantSpawner : BrambleSpawner
    {
        protected override void Spawn(Map map, IntVec3 loc)
        {
            Rand.PushState(Find.TickManager.TicksAbs);
            if (DefDatabase<ThingDef>.AllDefs.Where(td => td.plant is {Sowable: true}).TryRandomElement(out ThingDef plantDef))
                if (loc.GetTerrain(map).fertility >= plantDef.plant.fertilityMin)
                {
                    Plant plant = (Plant) ThingMaker.MakeThing(plantDef);
                    plant.Growth = Mathf.Clamp(this.GetComp<CompAbilitySpawn>().pawn.GetStatValue(StatDefOf.PsychicSensitivity) - 1f, 0.1f, 1f);
                    GenSpawn.Spawn(plant, loc, map);
                }

            Rand.PopState();
        }
    }
}