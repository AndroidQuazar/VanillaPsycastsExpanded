namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    [StaticConstructorOnStartup]
    public class Hediff_PsychicDrone : Hediff_Ability
    {
        private float curAngle;

        private Material material;
        public Material FieldMat
        {
            get
            {
                if (material == null)
                {
                    material = MaterialPool.MatFrom("Effects/Archotechist/PsychicDrone/PsychicDroneEnergyField", ShaderDatabase.MoteGlow);
                }
                return material;
            }
        }

        private List<Pawn> affectedPawns = new List<Pawn>();

        private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
        public override void Tick()
        {
            base.Tick();
            curAngle += 0.01f;
            if (curAngle > 360)
            {
                curAngle = 0;
            }
            if (Find.TickManager.TicksGame % 180 == 0)
            {
                foreach (var pawn in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, this.ability.GetRadiusForPawn(), true)
                    .OfType<Pawn>().Where(x => !affectedPawns.Contains(x) && !x.InMentalState && x.HostileTo(pawn) && x.RaceProps.IsFlesh))
                {
                    var mentalStateDef = Rand.Bool ? VPE_DefOf.VPE_Wander_Sad : MentalStateDefOf.Berserk;
                    if (pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateDef, causedByPsycast: true))
                    {
                        affectedPawns.Add(pawn);
                        Log.Message("Affecting " + pawn + " with " + mentalStateDef);
                    }
                    else
                    {
                        Log.Message("Failed to affect " + pawn + " with " + mentalStateDef);
                    }
                }
            }
        }

        public void Draw()
        {
            Vector3 pos = pawn.DrawPos;
            pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(pos, Quaternion.AngleAxis(curAngle, Vector3.up), new Vector3(this.ability.GetRadiusForPawn(), 1f, this.ability.GetRadiusForPawn()));
            UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix, FieldMat, 0, null, 0, MatPropertyBlock);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref affectedPawns, "affectedPawns", LookMode.Reference);
        }
    }
}