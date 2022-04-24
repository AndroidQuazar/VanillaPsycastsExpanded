﻿namespace VanillaPsycastsExpanded.Staticlord
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using VFECore.Shields;
    using Ability = VFECore.Abilities.Ability;

    [StaticConstructorOnStartup]
    public class HediffComp_Recharge : HediffComp_Draw
    {
        private static readonly SoundDef  sustainerDef = DefDatabase<SoundDef>.GetNamed("VPE_Recharge_Sustainer");
        private                 Thing     battery;
        private                 int       startTick;
        private                 Sustainer sustainer;

        public void Init(Thing bat)
        {
            this.battery   = bat;
            this.startTick = Find.TickManager.TicksGame;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (this.sustainer == null) this.sustainer = sustainerDef.TrySpawnSustainer(this.Pawn);
            this.sustainer.Maintain();
            if ((Find.TickManager.TicksGame - this.startTick) % 60 == 0) this.battery.TryGetComp<CompPowerBattery>().AddEnergy(100f);
        }

        public override void CompPostPostRemoved()
        {
            this.sustainer.End();
            base.CompPostPostRemoved();
        }

        public override void DrawAt(Vector3 drawPos)
        {
            Vector3   b      = this.battery.TrueCenter();
            Vector3   s      = new(this.Graphic.drawSize.x, 1f, (b - drawPos).magnitude);
            Matrix4x4 matrix = Matrix4x4.TRS(drawPos + (b - drawPos) / 2, Quaternion.LookRotation(b - drawPos), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, this.Graphic.MatSingle, 0);
        }
    }

    [StaticConstructorOnStartup]
    public class Ability_Recharge : Ability
    {
        private static readonly HediffDef hediffDef = DefDatabase<HediffDef>.GetNamed("VPE_Recharge");

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, this.pawn);
            hediff.TryGetComp<HediffComp_Recharge>().Init(target.Thing);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = this.GetDurationForPawn();
            this.pawn.health.AddHediff(hediff);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            Thing thing = target.Thing;
            if (thing is null) return false;

            CompPowerBattery comp = thing.TryGetComp<CompPowerBattery>();
            if (comp is not null) return true;
            if (showMessages) Messages.Message("VPE.MustTargetBattery".Translate(), MessageTypeDefOf.RejectInput, false);
            return false;
        }
    }
}