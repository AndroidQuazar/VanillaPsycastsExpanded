namespace VanillaPsycastsExpanded
{
    using UnityEngine;
    using Verse;

    [StaticConstructorOnStartup]
    public class CompSetStoneColour : ThingComp
    {
        private ThingDef                      rockDef;
        public  CompProperties_SetStoneColour Props => (CompProperties_SetStoneColour) this.props;

        public ThingDef KilledLeave => this.rockDef;

        public void SetStoneColour(ThingDef thingDef)
        {
            if (this.parent is not Pawn pawn) return;
            PawnRenderer renderer = pawn.Drawer.renderer;
            this.rockDef = thingDef;
            Color       color = thingDef.graphic.data.color;
            GraphicData data  = new();
            data.CopyFrom(pawn.ageTracker.CurKindLifeStage.bodyGraphicData);
            data.color    = color;
            data.colorTwo = color;
            if (!renderer.graphics.AllResolved) renderer.graphics.ResolveAllGraphics();
            renderer.graphics.nakedGraphic = data.Graphic;
            renderer.graphics.ClearCache();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Defs.Look(ref this.rockDef, nameof(this.rockDef));
        }
    }
}