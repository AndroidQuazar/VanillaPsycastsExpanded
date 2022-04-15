namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    public class AbilityExtension_Psycast : AbilityExtension_AbilityMod
    {
        public float entropyGain = 0f;
        public int   level;
        public int   order;

        public PsycasterPathDef path;
        public List<AbilityDef> prerequisites;
        public float            psyfocusCost = 0f;

        public float GetPsyfocusUsedByPawn(Pawn pawn) =>
            this.psyfocusCost * pawn.GetStatValue(StatDefOf.Ability_PsyfocusCost);

        public float GetEntropyUsedByPawn(Pawn pawn) =>
            this.entropyGain * pawn.GetStatValue(StatDefOf.Ability_EntropyGain);

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
                    if (ability.pawn.GetComp<CompAbilities>().LearnedAbilities.Any(ab => this.prerequisites.Contains(ab.def)))
                    {
                        reason = "None of the prerequisites learned";
                        return false;
                    }

                float psyfocusCost = this.GetPsyfocusUsedByPawn(ability.pawn);
                if (!((Hediff_PsycastAbilities) ability.Hediff).SufficientPsyfocusPresent(psyfocusCost))
                {
                    reason = "CommandPsycastNotEnoughPsyfocus".Translate(psyfocusCost, (ability.pawn.psychicEntropy.CurrentPsyfocus - psyfocusCost).ToStringPercent("0.#"),
                                                                         ability.def.label.Named("PSYCASTNAME"), ability.pawn.Named("CASTERNAME"));
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

    public static class AbilityExtensionPsycastUtility
    {
        private static readonly Dictionary<AbilityDef, AbilityExtension_Psycast> cache = new();

        public static AbilityExtension_Psycast Psycast(this AbilityDef def)
        {
            if (cache.TryGetValue(def, out AbilityExtension_Psycast ext)) return ext;
            ext = def.GetModExtension<AbilityExtension_Psycast>();
            cache[def] = ext;
            return ext;
        }
    }
}