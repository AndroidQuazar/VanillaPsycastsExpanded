namespace VanillaPsycastsExpanded.Chronopath
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using Ability = VFECore.Abilities.Ability;

    [StaticConstructorOnStartup]
    public class TimeSphere : Thing
    {
        private static readonly Material  DistortionMat = MaterialPool.MatFrom("Things/Mote/Black", ShaderDatabase.MoteGlowDistortBG);
        private static readonly Texture2D DistortionTex = ContentFinder<Texture2D>.Get("Things/Mote/PsycastDistortionMask");
        public                  float     Radius;
        public                  int       Duration;

        private int       startTick;
        private Sustainer sustainer;
        private float     scale       = 5f;
        private float     scaleChange = 1f;

        static TimeSphere()
        {
            DistortionMat.SetTexture("_DistortionTex", DistortionTex);
            DistortionMat.SetFloat("_distortionIntensity",  0.1f);
            DistortionMat.SetFloat("_brightnessMultiplier", 1.5f);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad) this.startTick = Find.TickManager.TicksGame;
        }

        public override void Tick()
        {
            if (this.IsHashIntervalTick(60))
                foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.Position, this.Map, this.Radius, true))
                {
                    if (thing is Pawn pawn) AbilityExtension_Age.Age(pawn, 1f);

                    if (thing is Plant plant)
                    {
                        if (plant.Growth < 1f) plant.Growth = 1f;
                        else if (plant.def.useHitPoints) thing.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * thing.MaxHitPoints));
                        else plant.Age = int.MaxValue;
                    }

                    if (thing is Building && thing.def.useHitPoints) thing.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * thing.MaxHitPoints));
                }

            if (this.sustainer == null) this.sustainer = VPE_DefOf.VPE_TimeSphere_Sustainer.TrySpawnSustainer(this);
            else this.sustainer.Maintain();

            if (this.scale >= this.Radius || this.scale <= 1f) this.scaleChange *= -1;

            this.scale =  Mathf.Clamp(this.scale, 1f, this.Radius);
            this.scale += this.scaleChange;

            if (Find.TickManager.TicksGame >= this.startTick + this.Duration) this.Destroy();
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            this.sustainer.End();
            base.Destroy(mode);
        }

        public override void Draw()
        {
            Graphics.DrawMesh(MeshPool.plane10,
                              Matrix4x4.TRS(this.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverheadLow), Quaternion.AngleAxis(0f, Vector3.up),
                                            Vector3.one * this.Radius * 2f), DistortionMat, 0);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.Radius,    "radius");
            Scribe_Values.Look(ref this.Duration,  "duration");
            Scribe_Values.Look(ref this.startTick, nameof(this.startTick));
        }
    }

    public class Ability_TimeSphere : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            TimeSphere sphere = (TimeSphere) ThingMaker.MakeThing(VPE_DefOf.VPE_TimeSphere);
            sphere.Duration = this.GetDurationForPawn();
            sphere.Radius   = this.GetRadiusForPawn();
            GenSpawn.Spawn(sphere, target.Cell, this.pawn.Map);
        }
    }
}