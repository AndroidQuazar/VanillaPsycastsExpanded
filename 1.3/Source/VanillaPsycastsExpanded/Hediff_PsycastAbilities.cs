namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    public class Hediff_PsycastAbilities : Hediff_Abilities
    {
        public float                  experience;
        public int                    points;
        public List<PsycasterPathDef> unlockedPaths;

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            //this.bioticEnergy = this.pawn.GetStatValue(RE_DefOf.RE_BioticEnergyMax);
        }

        public void UseAbility(float focus, float entropy)
        {
            this.pawn.psychicEntropy.TryAddEntropy(entropy);
            this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-focus);
        }

        public bool SufficientPsyfocusPresent(float focusRequired) =>
            this.pawn.psychicEntropy.CurrentPsyfocus > focusRequired;

        public override void Tick()
        {
            base.Tick();
            //this.bioticEnergy += this.pawn.GetStatValue(RE_DefOf.RE_BioticEnergyRecoveryRate) / GenTicks.TicksPerRealSecond;
            //this.bioticEnergy = Mathf.Min(this.bioticEnergy, this.pawn.GetStatValue(RE_DefOf.RE_BioticEnergyMax));
        }

        /*
        public override IEnumerable<Gizmo> DrawGizmos()
        {
            //Gizmo_BioticEnergyStatus gizmoBioticEnergy = new Gizmo_BioticEnergyStatus { bioticHediff = this };
            //yield return gizmoBioticEnergy;
        }
        */

        public override bool SatisfiesConditionForAbility(AbilityDef abilityDef) =>
            base.SatisfiesConditionForAbility(abilityDef) ||
            (abilityDef.requiredHediff?.minimumLevel <= this.pawn.GetPsylinkLevel());

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.experience, nameof(this.experience));
            Scribe_Values.Look(ref this.points, nameof(this.points));
            Scribe_Collections.Look(ref this.unlockedPaths, nameof(this.unlockedPaths), LookMode.Def);
        }
    }

    public class AbilityExtension_Psycast : AbilityExtension_AbilityMod
    {
        public float GetPsyfocusUsedByPawn(Pawn pawn) => 
            this.psyfocusCost * pawn.GetStatValue(StatDefOf.Ability_PsyfocusCost);

        public float GetEntropyUsedByPawn(Pawn pawn) =>
            this.entropyGain * pawn.GetStatValue(StatDefOf.Ability_EntropyGain);

        public PsycasterPathDef path;
        public List<AbilityDef>       prerequisites;

        public float            entropyGain  = 0f;
        public float            psyfocusCost = 0f;

        public override bool IsEnabledForPawn(Ability ability, out string reason)
        {
            if (ability.Hediff != null)
            {

                if (ability.pawn.psychicEntropy.PsychicSensitivity < float.Epsilon)
                {
                    reason = "CommandPsycastZeroPsychicSensitivity".Translate();
                    return true;
                }

                if (this.prerequisites != null)
                {
                    if (ability.pawn.GetComp<CompAbilities>().LearnedAbilities.Any(ab => this.prerequisites.Contains(ab.def)))
                    {
                        reason = "Not all prerequisites learned";
                        return false;
                    }
                }

                float psyfocusCost = this.GetPsyfocusUsedByPawn(ability.pawn);
                if (!((Hediff_PsycastAbilities)ability.Hediff).SufficientPsyfocusPresent(psyfocusCost))
                {
                    reason = "CommandPsycastNotEnoughPsyfocus".Translate(psyfocusCost, (ability.pawn.psychicEntropy.CurrentPsyfocus - psyfocusCost).ToStringPercent("0.#"), ability.def.label.Named("PSYCASTNAME"), ability.pawn.Named("CASTERNAME"));
                    return false;
                }

                if (ability.pawn.GetPsylinkLevel() > ability.def.requiredHediff.minimumLevel)
                {
                    reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(ability.def.requiredHediff.minimumLevel);
                    return true;
                }

                if (ability.pawn.psychicEntropy.WouldOverflowEntropy(this.GetEntropyUsedByPawn(ability.pawn)))
                {
                    reason = "CommandPsycastWouldExceedEntropy".Translate(ability.def.label);
                    return true;
                }

                reason = string.Empty;
                return true;
            }

            reason = string.Empty;
            return false;
        }

        public override void Cast(Ability ability)
        {
            base.Cast(ability);

            Hediff_PsycastAbilities psycastHediff = (Hediff_PsycastAbilities) ability.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
            psycastHediff.UseAbility(this.GetPsyfocusUsedByPawn(ability.pawn), this.GetEntropyUsedByPawn(ability.pawn));
        }

        public override string GetDescription(Ability ability) =>
            $"{this.GetPsyfocusUsedByPawn(ability.pawn)} {"PsyfocusLetter".Translate()}".Colorize(Color.cyan);
    }
}
