namespace VanillaPsycastsExpanded.Skipmaster
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Waterskip : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Map map = this.pawn.Map;
            foreach (IntVec3 c in this.AffectedCells(target, map))
            {
                List<Thing> thingList = c.GetThingList(map);
                for (int i = thingList.Count - 1; i >= 0; i--)
                    if (thingList[i] is Fire)
                        thingList[i].Destroy();

                if (!c.Filled(map)) FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Water);

                FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles);
                dataStatic.rotationRate = Rand.Range(-30, 30);
                dataStatic.rotation     = 90 * Rand.RangeInclusive(0, 3);
                map.flecks.CreateFleck(dataStatic);
            }
        }

        private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
        {
            if (target.Cell.Filled(this.pawn.Map)) yield break;

            foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, this.GetRadiusForPawn(), true))
                if (intVec.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, intVec, map, true))
                    yield return intVec;
        }

        public override void DrawHighlight(LocalTargetInfo target)
        {
            float range = this.GetRangeForPawn();
            if (GenRadial.MaxRadialPatternRadius > range && range >= 1)
                GenDraw.DrawRadiusRing(this.pawn.Position, range, Color.cyan);
            if (target.IsValid)
            {
                GenDraw.DrawTargetHighlight(target);
                GenDraw.DrawFieldEdges(this.AffectedCells(target, this.pawn.Map).ToList(), this.ValidateTarget(target, false) ? Color.white : Color.red);
            }
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (target.Cell.Filled(this.pawn.Map))
            {
                if (showMessages)
                    Messages.Message("AbilityOccupiedCells".Translate(this.def.LabelCap), target.ToTargetInfo(this.pawn.Map), MessageTypeDefOf.RejectInput,
                                     false);

                return false;
            }

            return base.ValidateTarget(target, showMessages);
        }
    }
}