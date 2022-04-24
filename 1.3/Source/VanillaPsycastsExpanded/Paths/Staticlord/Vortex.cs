namespace VanillaPsycastsExpanded.Staticlord
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;

    public class Vortex : ThingWithComps
    {
        public const float RADIUS   = 22.9f;
        public const int   DURATION = 2500;

        private Sustainer sustainer;
        private int       startTick;

        public override void Tick()
        {
            base.Tick();
            if (this.sustainer == null) this.sustainer = VPE_DefOf.VPE_Vortex_Sustainer.TrySpawnSustainer(this);
            this.sustainer.Maintain();
            for (int i = 0; i < 3; i++)
            {
                Vector3           pos  = this.DrawPos + new Vector3(Rand.Range(1f, RADIUS), 0f, 0f).RotatedBy(Rand.Range(0f, 360f));
                FleckCreationData data = FleckMaker.GetDataStatic(pos, this.Map, VPE_DefOf.VPE_VortexSpark);
                data.rotation = Rand.Range(0f, 360f);
                this.Map.flecks.CreateFleck(data);
            }

            if (Find.TickManager.TicksGame - this.startTick > DURATION) this.Destroy();
            if (this.IsHashIntervalTick(30))
                foreach (Pawn pawn in GenRadial.RadialDistinctThingsAround(this.Position, this.Map, RADIUS, true).OfType<Pawn>())
                    if (pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_Vortex) is null)
                    {
                        Hediff_Vortexed hediff = (Hediff_Vortexed) HediffMaker.MakeHediff(VPE_DefOf.VPE_Vortex, pawn);
                        hediff.Vortex = this;
                        pawn.health.AddHediff(hediff);
                    }
        }

        public override void Draw()
        {
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad) this.startTick = Find.TickManager.TicksGame;
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            this.sustainer.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.startTick, nameof(this.startTick));
        }
    }

    public class Hediff_Vortexed : Hediff
    {
        public Vortex Vortex;

        public override HediffStage CurStage => this.Vortex is null
            ? base.CurStage
            : new HediffStage
            {
                capMods = new List<PawnCapacityModifier>
                {
                    new()
                    {
                        capacity = PawnCapacityDefOf.Moving,
                        setMax   = Mathf.Lerp(0f, 0.4f, this.pawn.Position.DistanceTo(this.Vortex.Position) / Vortex.RADIUS)
                    },
                    new()
                    {
                        capacity = PawnCapacityDefOf.Manipulation,
                        setMax   = Mathf.Lerp(0f, 0.4f, this.pawn.Position.DistanceTo(this.Vortex.Position) / Vortex.RADIUS)
                    }
                }
            };

        public override bool ShouldRemove => this.Vortex.Destroyed || this.pawn.Position.DistanceTo(this.Vortex.Position) >= Vortex.RADIUS;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref this.Vortex, "vortex");
        }
    }
}