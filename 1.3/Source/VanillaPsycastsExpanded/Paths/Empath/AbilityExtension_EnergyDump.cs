namespace VanillaPsycastsExpanded.Empath
{
    using Verse;
    using VFECore.Abilities;

    public class AbilityExtension_EnergyDump : AbilityExtension_AbilityMod
    {
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            target.Pawn.needs.rest.CurLevel  = 1;
            ability.pawn.needs.rest.CurLevel = 0;
        }
    }
}