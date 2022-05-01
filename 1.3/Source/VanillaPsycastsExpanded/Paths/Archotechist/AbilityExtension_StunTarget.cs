namespace VanillaPsycastsExpanded
{
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class AbilityExtension_StunTarget : AbilityExtension_AbilityMod
	{
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            target.Pawn?.stances.stunner.StunFor(ability.GetDurationForPawn(), ability.pawn, addBattleLog: false);
        }
    }
}