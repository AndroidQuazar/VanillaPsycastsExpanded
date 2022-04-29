namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;

    public class FocusStrengthOffset_Value : FocusStrengthOffset
    {
        public override float GetOffset(Thing parent, Pawn user = null) => this.offset * parent.MarketValue;

        public override string GetExplanation(Thing parent) => "MarketValueTip".Translate() + ": " + (this.offset * parent.MarketValue).ToStringWithSign("0%");

        public override string GetExplanationAbstract(ThingDef def = null)
        {
            if (def == null) return "";
            return "MarketValueTip".Translate() + ": " + (this.offset * def.BaseMarketValue).ToStringWithSign("0%");
        }
    }
}