namespace VanillaPsycastsExpanded.Chronopath
{
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_ReduceResistance : AbilityExtension_AbilityMod
    {
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            Pawn pawn = target.Pawn;
            if (pawn?.HostFaction == null) return;
            if (pawn.GuestStatus  != GuestStatus.Prisoner) return;
            if (pawn.HostFaction  != ability.pawn.Faction) return;
            pawn.guest.resistance -= 20f;
        }
    }
}