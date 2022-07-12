﻿namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using RimWorld;
    using RimWorld.Planet;
    using UnityEngine;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    public class AbilityExtension_Psycast : AbilityExtension_AbilityMod
    {
        public float            entropyGain = 0f;
        public int              level;
        public int              order;
        public PsycasterPathDef path;
        public List<AbilityDef> prerequisites = new List<AbilityDef>();
        public float            psyfocusCost  = 0f;
        public bool             spaceAfter;
        public bool             showCastBubble = true;
        public bool             psychic;

        public bool PrereqsCompleted(Pawn pawn) => this.PrereqsCompleted(pawn.GetComp<CompAbilities>());

        public bool PrereqsCompleted(CompAbilities compAbilities)
        {
            return this.prerequisites == null || compAbilities.LearnedAbilities.Any(ab => this.prerequisites.Contains(ab.def));
        }

        public void UnlockWithPrereqs(CompAbilities compAbilities)
        {
            foreach (AbilityDef prerequisite in this.prerequisites)
            {
                AbilityExtension_Psycast extension = prerequisite.GetModExtension<AbilityExtension_Psycast>();
                if(extension != null)
                    extension.UnlockWithPrereqs(compAbilities);
                else
                    compAbilities.GiveAbility(prerequisite);
            }

            compAbilities.GiveAbility(this.abilityDef);
        }

        public float GetPsyfocusUsedByPawn(Pawn pawn) => this.psyfocusCost * pawn.GetStatValue(VPE_DefOf.VPE_PsyfocusCostFactor);

        public float GetEntropyUsedByPawn(Pawn pawn) => this.entropyGain;

        public override bool IsEnabledForPawn(Ability ability, out string reason)
        {
            Hediff_PsycastAbilities hediff = ability?.pawn?.Psycasts();
            if (hediff != null)
            {
                if (ability.pawn.psychicEntropy.PsychicSensitivity < float.Epsilon)
                {
                    reason = "CommandPsycastZeroPsychicSensitivity".Translate();
                    return true;
                }

                float psyfocusCost = this.GetPsyfocusUsedByPawn(ability.pawn);
                if (!hediff.SufficientPsyfocusPresent(psyfocusCost))
                {
                    reason = "CommandPsycastNotEnoughPsyfocus".Translate(
                        psyfocusCost.ToStringPercent("#.0"), ability.pawn.psychicEntropy.CurrentPsyfocus.ToStringPercent("#.0"),
                        ability.def.label.Named("PSYCASTNAME"), ability.pawn.Named("CASTERNAME"));
                    return false;
                }

                if (ability.pawn.psychicEntropy.WouldOverflowEntropy(this.GetEntropyUsedByPawn(ability.pawn)))
                {
                    reason = "CommandPsycastWouldExceedEntropy".Translate(ability.def.label);
                    return false;
                }

                if (hediff.currentlyChanneling != null)
                {
                    reason = "VPE.CurrentChanneling".Translate(hediff.currentlyChanneling.def.LabelCap);
                    return false;
                }

                reason = string.Empty;
                return true;
            }

            reason = "VPE.NotPsycaster".Translate();
            return false;
        }

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);

            Hediff_PsycastAbilities psycastHediff =
                (Hediff_PsycastAbilities) ability.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
            psycastHediff.UseAbility(this.GetPsyfocusUsedByPawn(ability.pawn), this.GetEntropyUsedByPawn(ability.pawn));
        }

        public override string GetDescription(Ability ability)
        {
            StringBuilder builder = new();

            float psyfocusCost = this.GetPsyfocusUsedByPawn(ability.pawn);
            if (psyfocusCost > float.Epsilon)
                builder.AppendInNewLine($"{"AbilityPsyfocusCost".Translate()}: {psyfocusCost.ToStringPercent()}");

            float entropy = this.GetEntropyUsedByPawn(ability.pawn);
            if (entropy > float.Epsilon)
                builder.AppendInNewLine($"{"AbilityEntropyGain".Translate()}: {entropy}");

            return builder.ToString().Colorize(Color.cyan);
        }

        public override void WarmupToil(Toil toil)
        {
            base.WarmupToil(toil);
            if (!this.showCastBubble) return;
            toil.AddPreInitAction(delegate
            {
                MoteCastBubble mote = (MoteCastBubble) ThingMaker.MakeThing(VPE_DefOf.VPE_Mote_Cast);
                mote.Setup(toil.actor, toil.actor.GetComp<CompAbilities>().currentlyCasting);
                GenSpawn.Spawn(mote, toil.actor.Position, toil.actor.Map);
            });
        }

        public override void TargetingOnGUI(LocalTargetInfo target, Ability ability)
        {
            base.TargetingOnGUI(target, ability);
            if (!this.psychic) return;
            List<GlobalTargetInfo> validTargets = ability.currentTargets.Where(t => t.IsValid && t.Map != null).ToList();
            GlobalTargetInfo[]     targets      = new GlobalTargetInfo[validTargets.Count + 1];
            validTargets.CopyTo(targets, 0);
            targets[targets.Length - 1] = target.ToGlobalTargetInfo(validTargets?.LastOrDefault().Map ?? ability.pawn.Map);
            ability.ModifyTargets(ref targets);
            foreach (GlobalTargetInfo targetInfo in targets)
                if (targetInfo.Thing is Pawn p)
                {
                    float sense = p.GetStatValue(StatDefOf.PsychicSensitivity);
                    if (sense < 1.401298E-45f)
                    {
                        Vector3 drawPos = p.DrawPos;
                        drawPos.z += 1f;
                        GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), "Ineffective".Translate(), Color.red);
                    }
                    else
                    {
                        Vector3 drawPos = p.DrawPos;
                        drawPos.z += 1f;
                        GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), StatDefOf.PsychicSensitivity.LabelCap + ": " + sense.ToStringPercent(),
                                          sense > float.Epsilon ? Color.white : Color.red);
                    }
                }
        }

        public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
            if (this.psychic && target.Pawn is { } p && p.GetStatValue(StatDefOf.PsychicSensitivity) < 1.401298E-45f)
            {
                if (throwMessages) Messages.Message("Ineffective".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            return true;
        }
    }

    public static class AbilityExtensionPsycastUtility
    {
        private static readonly Dictionary<AbilityDef, AbilityExtension_Psycast> cache = new();

        public static AbilityExtension_Psycast Psycast(this AbilityDef def)
        {
            if (cache.TryGetValue(def, out AbilityExtension_Psycast ext)) return ext;
            ext        = def.GetModExtension<AbilityExtension_Psycast>();
            cache[def] = ext;
            return ext;
        }
    }
}