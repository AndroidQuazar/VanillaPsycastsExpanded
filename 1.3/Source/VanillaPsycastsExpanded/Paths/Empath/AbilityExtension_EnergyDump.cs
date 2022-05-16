namespace VanillaPsycastsExpanded.Empath
{
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;

    public class AbilityExtension_EnergyDump : AbilityExtension_AbilityMod
    {
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            (targets[0].Thing as Pawn).needs.rest.CurLevel = 1;
            ability.pawn.needs.rest.CurLevel = 0;
        }
    }
}