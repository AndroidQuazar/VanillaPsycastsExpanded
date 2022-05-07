namespace VanillaPsycastsExpanded.Technomancer
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    using VFECore.Shields;

    [StaticConstructorOnStartup]
    public class HediffComp_InfinitePower : HediffComp_Draw
    {
        private static readonly Material OVERLAY = MaterialPool.MatFrom("Effects/Technomancer/Power/InfinitePowerOverlay", ShaderDatabase.MetaOverlay);

        private Thing           target;
        private CompPowerTrader compPower;

        public void Begin(Thing t)
        {
            this.target    = t;
            this.compPower = t.TryGetComp<CompPowerTrader>();
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.compPower.PowerOn     = true;
            this.compPower.PowerOutput = 0f;
        }

        public override void DrawAt(Vector3 drawPos)
        {
            Graphics.DrawMesh(MeshPool.plane10,
                              Matrix4x4.TRS(this.target.DrawPos.Yto0() + Vector3.up * AltitudeLayer.MetaOverlays.AltitudeFor(),
                                            Quaternion.AngleAxis(0f, Vector3.up), Vector3.one), OVERLAY, 0);
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            this.compPower.SetUpPowerVars();
        }
    }

    public class Ability_Power : Ability_HediffDuration
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            this.ApplyHediffs(this.pawn);
            this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_InfinitePower)?.TryGetComp<HediffComp_InfinitePower>().Begin(target.Thing);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!base.ValidateTarget(target, showMessages)) return false;
            if (target.Thing?.TryGetComp<CompPowerTrader>() is not {PowerOutput: <0f})
            {
                if (showMessages) Messages.Message("VPE.MustConsumePower".Translate(), MessageTypeDefOf.RejectInput, false);

                return false;
            }

            return true;
        }
    }
}