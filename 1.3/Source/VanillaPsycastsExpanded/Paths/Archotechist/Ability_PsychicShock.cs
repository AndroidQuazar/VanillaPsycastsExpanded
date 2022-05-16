namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    public class Ability_PsychicShock : Ability_HediffDuration
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (Rand.Chance(0.3f))
            {
                var pawn = targets[0].Thing as Pawn;
                FireUtility.TryAttachFire(pawn, 0.5f);
                BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
                if (brain != null)
                {
                    int num = Rand.RangeInclusive(1, 5);
                    pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, num, 0f, -1f, this.pawn, brain));
                }
            }
        }
    }
}