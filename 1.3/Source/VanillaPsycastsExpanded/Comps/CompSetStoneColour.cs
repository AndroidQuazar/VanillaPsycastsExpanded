namespace VanillaPsycastsExpanded
{
    using System;
    using UnityEngine;
    using Verse;

    [StaticConstructorOnStartup]
    public class CompSetStoneColour : ThingComp
    {
        private PawnRenderer pawn_renderer;


        public CompProperties_SetStoneColour Props => (CompProperties_SetStoneColour) this.props;


        public void SetStoneColour(ThingDef thingDef)
        {
            Pawn pawn = this.parent as Pawn;

            if (this.pawn_renderer == null) this.pawn_renderer = pawn.Drawer.renderer;

            Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize;

            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (this.pawn_renderer != null)
                    try
                    {
                        Color color = thingDef.graphic.color;
                        Graphic_Multi nakedGraphic =
                            (Graphic_Multi) GraphicDatabase.Get<Graphic_Multi>(this.pawn_renderer.graphics.nakedGraphic.path,
                                                                               this.pawn_renderer.graphics.nakedGraphic.Shader,
                                                                               vector, color);

                        this.pawn_renderer.graphics.ResolveAllGraphics();
                        this.pawn_renderer.graphics.nakedGraphic = nakedGraphic;
                        (this.pawn_renderer.graphics.nakedGraphic.data = new GraphicData()).shadowData =
                            pawn.ageTracker.CurKindLifeStage.bodyGraphicData.shadowData;
                    }
                    catch (NullReferenceException)
                    {
                    }
            });
        }
    }
}