namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    public class Ability_PsychicShock : Ability_HediffDuration
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            if (Rand.Chance(0.3f))
            {
                FireUtility.TryAttachFire(target.Pawn, 0.5f);
                BodyPartRecord brain = target.Pawn.health.hediffSet.GetBrain();
                if (brain != null)
                {
                    int num = Rand.RangeInclusive(1, 5);
                    target.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, num, 0f, -1f, this.pawn, brain));
                }
            }
        }
    }
}