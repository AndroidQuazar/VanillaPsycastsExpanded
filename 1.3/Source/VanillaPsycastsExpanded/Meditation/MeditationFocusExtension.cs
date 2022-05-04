namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using RimWorld;
    using Verse;

    public class MeditationFocusExtension : DefModExtension
    {
        public string               icon;
        public List<StatPart_Focus> statParts;
    }

    public abstract class StatPart_Focus : StatPart
    {
        public MeditationFocusDef focus;

        public bool ApplyOn(StatRequest req) => req.Pawn != null && this.focus.CanPawnUse(req.Pawn) && StatPart_NearbyFoci.ShouldApply;
    }
}