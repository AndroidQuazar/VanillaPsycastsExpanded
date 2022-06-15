﻿namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_GameCondition : AbilityExtension_AbilityMod
    {
        public GameConditionDef gameCondition;
        public FloatRange?      durationDays;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            GameCondition condition = GameConditionMaker.MakeCondition(this.gameCondition,
                                                                       this.durationDays.HasValue
                                                                           ? (int) (this.durationDays.Value.RandomInRange * GenDate.TicksPerDay)
                                                                           : ability.GetDurationForPawn());
            ability.pawn.Map.gameConditionManager.RegisterCondition(condition);
        }
    }
}