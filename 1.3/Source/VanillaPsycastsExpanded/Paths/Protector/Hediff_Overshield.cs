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
        public const float ShieldSizeModifier = 0.5f;
        public virtual float ShieldSize => 2.5f;
        public virtual Color ShieldColor => Color.yellow;

        private Material shieldMat;
        public Material BubbleMat
        {
            get
            {
                if (shieldMat == null)
                {
                    shieldMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent, ShieldColor);
                }
                return shieldMat;
            }
        }
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
            projectile.Destroy();
        }

        public virtual bool CanDestroyProjectile(Projectile projectile)
        {
            var cell = ((Vector3)VFECore.NonPublicFields.Projectile_origin.GetValue(projectile)).Yto0().ToIntVec3();
            return Vector3.Distance(projectile.ExactPosition.Yto0(), pawn.DrawPos.Yto0()) <= (ShieldSize * ShieldSizeModifier) &&
                !GenRadial.RadialCellsAround(pawn.Position, ShieldSize * ShieldSizeModifier, true).ToList().Contains(cell);
        }
        public void Draw()
        {
			float num = ShieldSize;
			Vector3 drawPos = pawn.Drawer.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float angle = Rand.Range(0, 360);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
			UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
		}
    }
}