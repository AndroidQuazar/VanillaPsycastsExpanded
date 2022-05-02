using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
    [StaticConstructorOnStartup]
    public class CompSetStoneColour : ThingComp
    {

        private PawnRenderer pawn_renderer;
      


        public CompProperties_SetStoneColour Props
        {
            get
            {
                return (CompProperties_SetStoneColour)this.props;
            }
        }

        


        public void SetStoneColour(ThingDef thingDef)
        {

            
                Pawn pawn = this.parent as Pawn;
                if (this.pawn_renderer == null)
                {
                    this.pawn_renderer = pawn.Drawer.renderer;

                }

                Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize;

                
                    LongEventHandler.ExecuteWhenFinished(delegate
                    {
                        if (this.pawn_renderer != null)
                        {

                            try
                            {

                                Color color = thingDef.graphic.color;
                                Graphic_Multi nakedGraphic = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(pawn_renderer.graphics.nakedGraphic.path, ShaderDatabase.Cutout, vector, color);
                                
                                this.pawn_renderer.graphics.ResolveAllGraphics();
                                this.pawn_renderer.graphics.nakedGraphic = nakedGraphic;
                                (this.pawn_renderer.graphics.nakedGraphic.data = new GraphicData()).shadowData = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.shadowData;

                            }
                            catch (NullReferenceException) { }
                        }

                    });

                




            }




        }


    }

