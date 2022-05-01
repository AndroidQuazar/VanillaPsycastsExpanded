namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Networking;
    using Verse;
    using VFECore.Abilities;

    [StaticConstructorOnStartup]
    public class Hediff_Overshield : HediffWithComps
    {
        private int lastInterceptTicks = -999999;
        private float lastInterceptAngle;
        private bool drawInterceptCone;

        public static float idlePulseSpeed = 3;
        public static float minIdleAlpha = 0.05f;
        public static float minAlpha = 0.2f;
        public virtual float ShieldSize => 1f;
        public virtual Color ShieldColor => Color.yellow;

        private Material shieldMat;
        public Material ForceFieldMat
        {
            get
            {
                if (shieldMat == null)
                {
                    shieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow, ShieldColor);
                }
                return shieldMat;
            }
        }

        private static Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

        private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();
        public override void Tick()
        {
            base.Tick();
			foreach (var thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, ShieldSize + 1, true))
            {
                if (thing is Projectile projectile)
                {
                    if (CanDestroyProjectile(projectile))
                    {
                        DestroyProjectile(projectile);
                    }
                }
            }
        }
        protected virtual void DestroyProjectile(Projectile projectile)
        {
            Effecter effecter = new Effecter(VPE_DefOf.Interceptor_BlockedProjectilePsychic);
            effecter.Trigger(new TargetInfo(projectile.Position, pawn.Map), TargetInfo.Invalid);
            effecter.Cleanup();
            lastInterceptAngle = projectile.ExactPosition.AngleToFlat(pawn.TrueCenter());
            lastInterceptTicks = Find.TickManager.TicksGame;
            drawInterceptCone = true;
            projectile.Destroy();
        }
        public virtual bool CanDestroyProjectile(Projectile projectile)
        {
            var cell = ((Vector3)VFECore.NonPublicFields.Projectile_origin.GetValue(projectile)).Yto0().ToIntVec3();
            return Vector3.Distance(projectile.ExactPosition.Yto0(), pawn.DrawPos.Yto0()) <= ShieldSize &&
                !GenRadial.RadialCellsAround(pawn.Position, ShieldSize, true).ToList().Contains(cell);
        }
        public void Draw()
        {
            Vector3 pos = pawn.DrawPos;
            pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
            float currentAlpha = GetCurrentAlpha();
            if (currentAlpha > 0f)
            {
                Color value = ShieldColor;
                value.a *= currentAlpha;
                MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(pos, Quaternion.identity, new Vector3(ShieldSize * 2f * 1.16015625f, 1f, ShieldSize * 2f * 1.16015625f));
                UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix, ForceFieldMat, 0, null, 0, MatPropertyBlock);
            }
            float currentConeAlpha_RecentlyIntercepted = GetCurrentConeAlpha_RecentlyIntercepted();
            if (currentConeAlpha_RecentlyIntercepted > 0f)
            {
                Color color = ShieldColor;
                color.a *= currentConeAlpha_RecentlyIntercepted;
                MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
                Matrix4x4 matrix2 = default(Matrix4x4);
                matrix2.SetTRS(pos, Quaternion.Euler(0f, lastInterceptAngle - 90f, 0f), new Vector3(ShieldSize * 2f * 1.16015625f, 1f, ShieldSize * 2f * 1.16015625f));
                UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix2, ForceFieldConeMat, 0, null, 0, MatPropertyBlock);
            }
        }

        private float GetCurrentAlpha()
        {
            return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(GetCurrentAlpha_Idle(), GetCurrentAlpha_Selected()), GetCurrentAlpha_RecentlyIntercepted()), GetCurrentAlpha_RecentlyActivated()), minAlpha);
        }

        private float GetCurrentAlpha_Idle()
        {
            if (Find.Selector.IsSelected(pawn))
            {
                return 0f;
            }
            return Mathf.Lerp(minIdleAlpha, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(pawn.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * idlePulseSpeed) + 1f) / 2f);
        }

        private float GetCurrentAlpha_Selected()
        {
            float num = Mathf.Max(2f, idlePulseSpeed);
            if (!Find.Selector.IsSelected(pawn))
            {
                return 0f;
            }
            return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(pawn.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * num) + 1f) / 2f);
        }

        private float GetCurrentAlpha_RecentlyIntercepted()
        {
            int num = Find.TickManager.TicksGame - lastInterceptTicks;
            return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
        }

        private float GetCurrentAlpha_RecentlyActivated()
        {
            int num = Find.TickManager.TicksGame - (lastInterceptTicks);
            return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
        }

        private float GetCurrentConeAlpha_RecentlyIntercepted()
        {
            if (!drawInterceptCone)
            {
                return 0f;
            }
            int num = Find.TickManager.TicksGame - lastInterceptTicks;
            return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastInterceptTicks, "lastInterceptTicks");
            Scribe_Values.Look(ref lastInterceptAngle, "lastInterceptTicks");
            Scribe_Values.Look(ref drawInterceptCone, "drawInterceptCone");
        }
    }
}