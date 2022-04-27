namespace VanillaPsycastsExpanded.Skipmaster
{
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Smokepop : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            GenExplosion.DoExplosion(target.Cell, this.pawn.MapHeld, this.GetRadiusForPawn(), DamageDefOf.Smoke, null, -1, -1f, null, null, null, null,
                                     ThingDefOf.Gas_Smoke, 1f);
        }
    }
}