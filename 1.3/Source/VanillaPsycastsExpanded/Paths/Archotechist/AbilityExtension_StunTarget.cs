namespace VanillaPsycastsExpanded
{
    using RimWorld.Planet;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class AbilityExtension_StunTarget : AbilityExtension_AbilityMod
	{
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (var target in targets)
            {
                var pawn = target.Thing as Pawn;
                pawn?.stances.stunner.StunFor(ability.GetDurationForPawn(), ability.pawn, addBattleLog: false);
            }
        }
    }
}