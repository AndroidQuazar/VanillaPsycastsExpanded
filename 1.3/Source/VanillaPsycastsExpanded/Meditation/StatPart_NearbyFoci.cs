namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using Verse;

    public class StatPart_NearbyFoci : StatPart
    {
        private static bool shouldApply;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing == null || !shouldApply) return;
            shouldApply = false;
            val += GenRadial.RadialDistinctThingsAround(req.Thing.Position, req.Thing.Map, MeditationUtility.FocusObjectSearchRadius, true)
                            .Where(thing => thing.TryGetComp<CompMeditationFocus>() is { } comp && comp.CanPawnUse(req.Pawn))
                            .Sum(thing => thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, req.Pawn));
            shouldApply = true;
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing == null || !shouldApply) return "";
            shouldApply = false;
            List<string> lines =
                (from thing in GenRadial.RadialDistinctThingsAround(req.Thing.Position, req.Thing.Map, MeditationUtility.FocusObjectSearchRadius, true)
                 where thing.TryGetComp<CompMeditationFocus>() is { } comp && comp.CanPawnUse(req.Pawn)
                 select thing.LabelCap + ": " +
                        StatDefOf.MeditationFocusStrength.Worker.ValueToString(thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, req.Pawn), true,
                                                                               ToStringNumberSense.Offset)).ToList();
            shouldApply = true;

            return lines.Count > 0 ? "VPE.Nearby" + ":" + lines.ToLineList("", true) : "";
        }
    }
}