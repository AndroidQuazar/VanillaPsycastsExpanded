namespace VanillaPsycastsExpanded.Chronopath
{
    using System.Collections.Generic;
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_Age : AbilityExtension_AbilityMod
    {
        public float? targetYears = null;
        public float? casterYears = null;

        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            if (this.targetYears.HasValue) Age(target.Pawn,  this.targetYears.Value);
            if (this.casterYears.HasValue) Age(ability.pawn, this.casterYears.Value);
        }

        private static void Age(Pawn pawn, float years)
        {
            pawn.ageTracker.AgeTickMothballed((int) (years * GenDate.TicksPerYear));
            if (years < 0)
            {
                List<HediffGiverSetDef> giverSets = pawn.def.race.hediffGiverSets;
                if (giverSets == null) return;
                float lifeFraction = pawn.ageTracker.AgeBiologicalYears / pawn.def.race.lifeExpectancy;
                foreach (HediffGiverSetDef giverSet in giverSets)
                foreach (HediffGiver giver in giverSet.hediffGivers)
                    if (giver is HediffGiver_Birthday giverBirthday)
                        if (giverBirthday.ageFractionChanceCurve.Evaluate(lifeFraction) <= 0f)
                        {
                            Hediff hediff;
                            while ((hediff = pawn.health.hediffSet.GetFirstHediffOfDef(giverBirthday.hediff)) != null) pawn.health.RemoveHediff(hediff);
                        }
            }

            if (pawn.ageTracker.AgeBiologicalYears < 16)
                pawn.ageTracker.AgeTickMothballed(GenDate.TicksPerYear * 16 - (int) pawn.ageTracker.AgeBiologicalTicks);
        }
    }
}