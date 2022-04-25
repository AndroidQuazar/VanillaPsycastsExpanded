namespace VanillaPsycastsExpanded.Staticlord
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Flashstorm : Ability
    {
        private readonly HashSet<Faction> affectedFactionCache = new();

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Map                      map             = this.pawn.Map;
            Thing                    conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.pawn.Map);
            GameCondition_Flashstorm flashstorm      = (GameCondition_Flashstorm) GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm);
            flashstorm.centerLocation     = target.Cell.ToIntVec2;
            flashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.GetRadiusForPawn()), Mathf.RoundToInt(this.GetRadiusForPawn()));
            flashstorm.Duration           = this.GetDurationForPawn();
            flashstorm.suppressEndMessage = true;
            flashstorm.initialStrikeDelay = new IntRange(60, 180);
            flashstorm.conditionCauser    = conditionCauser;
            flashstorm.ambientSound       = true;
            map.gameConditionManager.RegisterCondition(flashstorm);
            this.ApplyGoodwillImpact(target, flashstorm.AreaRadius);
        }

        private void ApplyGoodwillImpact(LocalTargetInfo target, int radius)
        {
            if (this.pawn.Faction != Faction.OfPlayer) return;

            this.affectedFactionCache.Clear();
            foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, radius, true))
                if (thing is Pawn p                             && thing.Faction != null                              && thing.Faction != this.pawn.Faction &&
                    !thing.Faction.HostileTo(this.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) &&
                    (this.def.applyGoodwillImpactToLodgers || !p.IsQuestLodger()))
                {
                    this.affectedFactionCache.Add(thing.Faction);
                    Faction.OfPlayer.TryAffectGoodwillWith(thing.Faction, this.def.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility);
                }

            this.affectedFactionCache.Clear();
        }
    }
}