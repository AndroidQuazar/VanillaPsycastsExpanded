namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    using AbilityDef = VFECore.Abilities.AbilityDef;
    
    public class Hediff_PsycastAbilities : Hediff_Abilities
    {
        public float                    experience;
        public int                      points;
        public List<PsycasterPathDef>   unlockedPaths;
        public List<MeditationFocusDef> unlockedMeditationFoci;
        public List<PsySet>             psysets;

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
            Scribe_Collections.Look(ref this.unlockedMeditationFoci, nameof(this.unlockedMeditationFoci), LookMode.Def);
            Scribe_Collections.Look(ref this.psysets, nameof(this.psysets), LookMode.Deep);
        }

        public void SpentPoints(int count = 1)
        {
            this.points -= count;
        }

        public void UnlockPath(PsycasterPathDef path)
        {
            this.unlockedPaths.Add(path);
        }

        public void UnlockMeditationFocus(MeditationFocusDef focus)
        {
            this.unlockedMeditationFoci.Add(focus);
        }

        public static int ExperienceRequiredForLevel(int level)
        {
            if (level <= 1)
                return 100;
            if (level <= 20)
                return Mathf.RoundToInt(ExperienceRequiredForLevel(level - 1) * 1.15f);
            if (level <= 30)
                return Mathf.RoundToInt(ExperienceRequiredForLevel(level - 1) * 1.10f);
            return Mathf.RoundToInt(ExperienceRequiredForLevel(level - 1) * 1.05f);
        }

        public static int ExperienceRequiredForNextLevel(int curLevel)
        {
            if (curLevel <= 0)
                return 100;
            if (curLevel < 20)
                return Mathf.RoundToInt(ExperienceRequiredForLevel(curLevel) * 0.15f);
            if (curLevel < 30)
                return Mathf.RoundToInt(ExperienceRequiredForLevel(curLevel) * 0.10f);
            return Mathf.RoundToInt(ExperienceRequiredForLevel(curLevel) * 0.05f);
        }
    }

    public class AbilityExtension_Psycast : AbilityExtension_AbilityMod
    {
        public float GetPsyfocusUsedByPawn(Pawn pawn) => 
            this.psyfocusCost * pawn.GetStatValue(StatDefOf.Ability_PsyfocusCost);

        public float GetEntropyUsedByPawn(Pawn pawn) =>
            this.entropyGain * pawn.GetStatValue(StatDefOf.Ability_EntropyGain);

        public PsycasterPathDef path;
        public List<AbilityDef> prerequisites;
        public int              order;

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
                        reason = "None of the prerequisites learned";
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
