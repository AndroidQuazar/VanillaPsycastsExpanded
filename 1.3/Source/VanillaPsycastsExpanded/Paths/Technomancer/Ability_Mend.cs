namespace VanillaPsycastsExpanded.Technomancer
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Mend : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            target.Thing.HitPoints = Mathf.Clamp(target.Thing.HitPoints + (int) ((this.pawn.GetStatValue(StatDefOf.PsychicSensitivity) - 0.9f) * 100f),
                                                 target.Thing.HitPoints, target.Thing.MaxHitPoints);
        }

        public override bool CanHitTarget(LocalTargetInfo target) => base.CanHitTarget(target) && target.Thing is {def: {useHitPoints: true}};
    }
}