namespace VanillaPsycastsExpanded.Technomancer
{
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_IncreaseQuality : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            CompQuality comp = target.Thing.TryGetComp<CompQuality>();
            if (comp is not {Quality: < QualityCategory.Excellent}) return;
            comp.SetQuality(comp.Quality + 1, ArtGenerationContext.Colony);
            for (int i = 0; i < 16; i++) FleckMaker.ThrowMicroSparks(target.CenterVector3, this.pawn.Map);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!base.ValidateTarget(target, showMessages)) return false;

            CompQuality comp;
            if ((comp = target.Thing.TryGetComp<CompQuality>()) == null)
            {
                if (showMessages) Messages.Message("VPE.MustHaveQuality".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            if (comp.Quality >= QualityCategory.Excellent)
            {
                if (showMessages) Messages.Message("VPE.QualityTooHigh".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            return true;
        }
    }
}