namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    public class Hediff_PsycastAbilities : Hediff_Abilities
    {
        public  float experience;
        public  int   points;
        private int   statPoints;
        private float minHeat;

        public Hediff_Psylink           psylink;
        public List<PsySet>             psysets                = new();
        public List<MeditationFocusDef> unlockedMeditationFoci = new();
        public List<PsycasterPathDef>   unlockedPaths          = new();

        private HediffStage curStage;

        public override HediffStage CurStage
        {
            get
            {
                if (this.curStage == null) this.RecacheCurStage();
                return this.curStage;
            }
        }

        public void InitializeFromPsylink(Hediff_Psylink psylink)
        {
            this.psylink = psylink;
            this.level   = psylink.level;
            this.points  = this.level;
            this.RecacheCurStage();
        }

        private void RecacheCurStage()
        {
            this.curStage = new HediffStage
            {
                statOffsets = new List<StatModifier>
                {
                    new() {stat = StatDefOf.PsychicEntropyMax, value          = this.level * 5     + this.statPoints * 20},
                    new() {stat = StatDefOf.PsychicEntropyRecoveryRate, value = this.level * 0.05f + this.statPoints * 0.2f},
                    new() {stat = StatDefOf.PsychicSensitivity, value         = this.statPoints * 0.05f},
                    new() {stat = StatDefOf.MeditationFocusGain, value        = this.statPoints * 0.1f},
                    new() {stat = VPE_DefOf.VPE_PsyfocusCostFactor, value     = this.statPoints * -0.01f},
                    new() {stat = VPE_DefOf.VPE_PsychicEntropyMinimum, value  = this.minHeat}
                },
                becomeVisible = false
            };
            this.pawn.health.Notify_HediffChanged(this);
        }

        public void UseAbility(float focus, float entropy)
        {
            this.pawn.psychicEntropy.TryAddEntropy(entropy);
            this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-focus);
        }

        public override void ChangeLevel(int levelOffset)
        {
            base.ChangeLevel(levelOffset);
            this.points += levelOffset;
            this.RecacheCurStage();
        }

        public void GainExperience(float experienceGain)
        {
            this.experience += experienceGain;
            while (this.experience >= ExperienceRequiredForLevel(this.level + 1))
            {
                this.ChangeLevel(1);
                this.experience -= ExperienceRequiredForLevel(this.level);
            }
        }

        public bool SufficientPsyfocusPresent(float focusRequired) =>
            this.pawn.psychicEntropy.CurrentPsyfocus > focusRequired;

        public override bool SatisfiesConditionForAbility(AbilityDef abilityDef) =>
            base.SatisfiesConditionForAbility(abilityDef) ||
            abilityDef.requiredHediff?.minimumLevel <= this.psylink.level;

        public void OffsetMinHeat(float offset)
        {
            this.minHeat += offset;
            this.RecacheCurStage();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.experience, nameof(this.experience));
            Scribe_Values.Look(ref this.points,     nameof(this.points));
            Scribe_Values.Look(ref this.statPoints, nameof(this.statPoints));
            Scribe_Collections.Look(ref this.unlockedPaths,          nameof(this.unlockedPaths),          LookMode.Def);
            Scribe_Collections.Look(ref this.unlockedMeditationFoci, nameof(this.unlockedMeditationFoci), LookMode.Def);
            Scribe_Collections.Look(ref this.psysets,                nameof(this.psysets),                LookMode.Deep);
            Scribe_References.Look(ref this.psylink, nameof(this.psylink));
        }

        public void SpentPoints(int count = 1)
        {
            this.points -= count;
        }

        public void ImproveStats(int count = 1)
        {
            this.statPoints += count;
            this.RecacheCurStage();
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

        public override void GiveRandomAbilityAtLevel(int? forLevel = null)
        {
        }
    }
}