namespace VanillaPsycastsExpanded.Chronopath
{
    using System.Linq;
    using Harmonist;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using Ability = VFECore.Abilities.Ability;

    public class GameCondition_TimeQuake : GameCondition_TimeSnow
    {
        public  float     SafeRadius;
        public  Pawn      Pawn;
        private Sustainer sustainer;

        public override void GameConditionTick()
        {
            base.GameConditionTick();
            if (this.TicksPassed % 60 == 0)
                foreach (Map map in this.AffectedMaps)
                {
                    for (int i = 0; i < 2000; i++) map.wildPlantSpawner.WildPlantSpawnerTick();

                    foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned.Where(this.CanEffect))
                        AbilityExtension_Age.Age(pawn, 1f);

                    foreach (Plant plant in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).OfType<Plant>().Where(this.CanEffect))
                        if (plant.Growth < 1f) plant.Growth = 1f;
                        else if (plant.def.useHitPoints) plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 0.01f * plant.MaxHitPoints));
                        else plant.Age = int.MaxValue;

                    foreach (Building building in map.listerBuildings.allBuildingsColonist
                                                     .Concat(map.listerBuildings.allBuildingsNonColonist)
                                                     .Where(this.CanEffect))
                        if (building.def.useHitPoints)
                            building.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * building.MaxHitPoints));
                }

            if (this.TicksPassed % 300 == 250)
                foreach (Map map in this.AffectedMaps)
                    Ability_RandomEvent.DoRandomEvent(map);

            if (this.sustainer == null) this.sustainer = VPE_DefOf.Psycast_Neuroquake_CastLoop.TrySpawnSustainer(SoundInfo.OnCamera());
            else this.sustainer.Maintain();
        }

        public override void End()
        {
            this.sustainer.End();
            VPE_DefOf.Psycast_Neuroquake_CastEnd.PlayOneShotOnCamera();
            base.End();
        }

        private bool CanEffect(Thing thing) => !thing.Position.InHorDistOf(this.Pawn.Position, this.SafeRadius);

        public override void GameConditionDraw(Map map)
        {
            base.GameConditionDraw(map);
            if (Find.Selector.IsSelected(this.Pawn)) GenDraw.DrawRadiusRing(this.Pawn.Position, this.SafeRadius, Color.yellow);
        }
    }

    public class Ability_TimeQuake : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            GameCondition_TimeQuake condition = (GameCondition_TimeQuake) GameConditionMaker.MakeCondition(VPE_DefOf.VPE_TimeQuake, this.GetDurationForPawn());
            condition.SafeRadius = this.GetRadiusForPawn();
            condition.Pawn       = this.pawn;
            ThingWithComps skyChanger = (ThingWithComps) ThingMaker.MakeThing(VPE_DefOf.VPE_SkyChanger);
            GenSpawn.Spawn(skyChanger, this.pawn.Position, this.pawn.Map);
            skyChanger.TryGetComp<CompAffectsSky>().StartFadeInHoldFadeOut(0, this.GetDurationForPawn(), 0);
            this.pawn.Map.gameConditionManager.RegisterCondition(condition);
            foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
            {
                if (faction.CanChangeGoodwillFor(this.pawn.Faction, -10))
                    faction.TryAffectGoodwillWith(this.pawn.Faction, -10, reason: HistoryEventDefOf.UsedHarmfulAbility);

                if (faction.CanChangeGoodwillFor(this.pawn.Faction, -75) && this.pawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Count > 0)
                    faction.TryAffectGoodwillWith(this.pawn.Faction, -75, reason: HistoryEventDefOf.UsedHarmfulAbility);
            }
        }
    }

    public class ThoughtWorker_TimeQuake : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p) => p.Map.gameConditionManager.ConditionIsActive(VPE_DefOf.VPE_TimeQuake);
    }
}