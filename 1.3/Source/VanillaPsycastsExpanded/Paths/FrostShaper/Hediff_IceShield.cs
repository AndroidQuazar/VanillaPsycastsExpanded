namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    public class Hediff_IceShield : Hediff_Overlay
    {
        public override string OverlayPath => "Effects/Frostshaper/FrostShield/Frostshield";
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            if (dinfo.Instigator is Pawn attacker)
            {
                if (dinfo.Weapon is null || dinfo.Weapon.race != null || dinfo.Weapon.IsMeleeWeapon)
                {
                    HealthUtility.AdjustSeverity(attacker, HediffDefOf.Hypothermia, 0.05f);
                }
            }
        }

        public override void Draw()
        {
            Vector3 pos = pawn.DrawPos;
            pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
            Matrix4x4 matrix = default(Matrix4x4);
            var drawSize = 1.5f;
            matrix.SetTRS(pos, Quaternion.identity, new Vector3(drawSize, 1f, drawSize));
            UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix, ShieldMat, 0, null, 0, MatPropertyBlock);
        }
    }
}