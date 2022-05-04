namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_GiveInspiration : AbilityExtension_AbilityMod
    {
        public bool onlyPlayer;

        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            Pawn pawn = target.Pawn;
            if (pawn != null && (!this.onlyPlayer || pawn.Faction is {IsPlayer: true}))
            {
                InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
                if (randomAvailableInspirationDef != null)
                    pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef,
                                                                          "LetterPsychicInspiration".Translate(
                                                                              pawn.Named("PAWN"), ability.pawn.Named("CASTER")));
            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, Ability ability, bool throwMessages = false) => this.Valid(target, ability);

        public override bool Valid(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
            Pawn pawn = target.Pawn;
            if (pawn != null && (!this.onlyPlayer || pawn.Faction is {IsPlayer: true}))
            {
                if (!AbilityUtility.ValidateNoInspiration(pawn, throwMessages)) return false;
                if (!AbilityUtility.ValidateCanGetInspiration(pawn, throwMessages)) return false;
            }

            return base.Valid(target, ability, throwMessages);
        }
    }
}