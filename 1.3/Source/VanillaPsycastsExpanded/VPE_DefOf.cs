namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;

    [DefOf]
    public static class VPE_DefOf
    {
        public static HediffDef VPE_PsycastAbilityImplant;
        public static HediffDef VPE_Recharge;
        public static HediffDef VPE_Vortex;
        public static HediffDef PsychicComa;
        public static HediffDef VPE_BlockBleeding;

        public static ThingDef VPE_ChainBolt;
        public static ThingDef VPE_Bolt;
        public static ThingDef VPE_Mote_FireBeam;
        public static ThingDef VPE_HurricaneMaker;
        public static ThingDef VPE_Skipdoor;

        public static SoundDef VPE_Recharge_Sustainer;
        public static SoundDef VPE_BallLightning_Zap;
        public static SoundDef VPE_Vortex_Sustainer;
        public static SoundDef VPE_Skipdoor_Sustainer;

        [DefAlias("VPE_Hurricane")] public static WeatherDef       VPE_Hurricane_Weather;
        [DefAlias("VPE_Hurricane")] public static GameConditionDef VPE_Hurricane_Condition;

        public static GameConditionDef VPE_PsychicFlashstorm;

        public static StatDef VPE_PsyfocusCostFactor;
        public static StatDef VPE_PsychicEntropyMinimum;
        public static JobDef  VPE_UseSkipdoor;

        public static FleckDef VPE_VortexSpark;
        public static ThingDef VPE_Mote_GreenMist;
        public static HediffDef VPE_Regenerating;

        public static MeditationFocusDef VPE_Group;
        public static MeditationFocusDef VPE_Archotech;
        public static MeditationFocusDef VPE_Science;
        public static MeditationFocusDef VPE_Wealth;

        public static SoundDef VPE_GuardianSkipbarrier_Sustainer;
        public static HediffDef VPE_GuardianSkipBarrier;
    }
}