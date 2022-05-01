namespace VanillaPsycastsExpanded.Chronopath
{
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_ImproveRelations : AbilityExtension_AbilityMod
    {
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            Pawn pawn = target.Pawn;
            if (pawn?.Faction is null or {IsPlayer: true}) return;
            if (pawn.Faction.RelationKindWith(ability.pawn.Faction) == FactionRelationKind.Hostile) return;
            if (pawn.guest.HostFaction                              != null) return;
            pawn.Faction.TryAffectGoodwillWith(ability.pawn.Faction, 20, reason: VPE_DefOf.VPE_Foretelling);
        }
    }
}