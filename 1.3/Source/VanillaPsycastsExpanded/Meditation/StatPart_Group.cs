namespace VanillaPsycastsExpanded
{
    using System.Linq;
    using RimWorld;
    using Verse;

    public class StatPart_Group : StatPart_Focus
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!this.ApplyOn(req)) return;

            if (req.Thing.Map != null)
                val += GenRadial.RadialDistinctThingsAround(req.Thing.Position, req.Thing.Map, 5f, true).OfType<Pawn>()
                             .Count(p => p.CurJobDef == JobDefOf.Meditate) switch
                {
                    <=1 => 0,
                    2   => 0.06f,
                    3   => 0.2f,
                    4   => 0.45f,
                    >=5 => 0.8f
                };
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (!this.ApplyOn(req)) return "";

            int num = req.Thing.Map != null ? GenRadial.RadialDistinctThingsAround(req.Thing.Position, req.Thing.Map, 5f, true).OfType<Pawn>().Count(p => p.CurJobDef == JobDefOf.Meditate) : 0;
            return "VPE.GroupFocus".Translate(num - 1) + ": " + this.parentStat.Worker.ValueToString(num switch
            {
                <=1 => 0,
                2   => 0.06f,
                3   => 0.2f,
                4   => 0.45f,
                >=5 => 0.8f
            }, true, ToStringNumberSense.Offset);
        }
    }
}