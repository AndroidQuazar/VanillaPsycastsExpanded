namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_FrostRay : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Projectile projectile = GenSpawn.Spawn(this.def.GetModExtension<AbilityExtension_Projectile>().projectile, this.pawn.Position, this.pawn.Map) as Projectile;
            if (projectile is AbilityProjectile abilityProjectile)
            {
                abilityProjectile.ability = this;
            }
            projectile?.Launch(this.pawn, this.pawn.DrawPos, target, target, ProjectileHitFlags.IntendedTarget);
            this.pawn.stances.SetStance(new Stance_Stand(this.GetDurationForPawn(), target, verb));
        }

        public override void ApplyHediffs(LocalTargetInfo targetInfo)
        {
            Ability_HediffDuration.ApplyHediff(this, this.pawn);
        }
    }
}