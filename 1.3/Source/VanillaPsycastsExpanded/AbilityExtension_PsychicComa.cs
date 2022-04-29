namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class AbilityExtension_PsychicComa : AbilityExtension_AbilityMod
    {
        public float     hours;
        public HediffDef coma;
        public StatDef   multiplier;
        public int       ticks;

        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            float duration = this.hours * GenDate.TicksPerHour + this.ticks;
            float mult     = ability.pawn.GetStatValue(this.multiplier ?? StatDefOf.PsychicSensitivity);

            if (mult != 1f) duration *= 1 / (mult - 1f);

            Hediff hediff = HediffMaker.MakeHediff(this.coma ?? VPE_DefOf.PsychicComa, ability.pawn);
            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(duration);
            ability.pawn.health.AddHediff(hediff);
        }
    }
}