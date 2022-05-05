namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Linq;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_SnapFreeze : Ability
    {
        public override void ApplyHediffs(LocalTargetInfo targetInfo)
        {
            ApplyHediff(this, targetInfo);
        }
        public static void ApplyHediff(Ability ability, LocalTargetInfo targetInfo)
        {
            var hediffExtension = ability.def.GetModExtension<AbilityExtension_Hediff>();
            if (targetInfo.Pawn == null || !(hediffExtension?.applyAuto ?? false)) return;
            BodyPartRecord bodyPart = hediffExtension.bodyPartToApply != null
                ? ability.pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault((BodyPartRecord x) => x.def == hediffExtension.bodyPartToApply)
                : null;
            var localHediff = HediffMaker.MakeHediff(hediffExtension.hediff, targetInfo.Pawn, bodyPart);
            if (hediffExtension.severity > float.Epsilon)
                localHediff.Severity = hediffExtension.severity;
            if (localHediff is Hediff_Ability hediffAbility)
            {
                hediffAbility.ability = ability;
            }
            var duration = ability.GetDurationForPawn();
            var ambientTemperature = targetInfo.Pawn.AmbientTemperature;
            if (ambientTemperature >= 0)
            {
                duration = (int)(duration * (1f - (ambientTemperature / 100f)));
            }
            if (hediffExtension.durationMultiplier != null)
            {
                duration = (int)(duration * targetInfo.Pawn.GetStatValue(hediffExtension.durationMultiplier));
            }
            if (localHediff is HediffWithComps hwc)
                foreach (var hediffComp in hwc.comps)
                    switch (hediffComp)
                    {
                        case HediffComp_Ability hca:
                            hca.ability = ability;
                            break;
                        case HediffComp_Disappears hcd:
                            hcd.ticksToDisappear = duration;
                            break;
                    }

            targetInfo.Pawn.health.AddHediff(localHediff);
        }
    }
}