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
        public static HediffDef VPE_ControlledFrenzy;
        public static HediffDef VPE_Regenerating;
        public static HediffDef VPE_GuardianSkipBarrier;
        public static HediffDef HeartAttack;

        public static ThingDef VPE_ChainBolt;
        public static ThingDef VPE_Bolt;
        public static ThingDef VPE_Mote_FireBeam;
        public static ThingDef VPE_HurricaneMaker;
        public static ThingDef VPE_Skipdoor;
        public static ThingDef VPE_Mote_GreenMist;
        public static ThingDef VPE_JumpingPawn;
        public static ThingDef VPE_TimeSphere;
        public static ThingDef VPE_SkyChanger;

        public static SoundDef VPE_Recharge_Sustainer;
        public static SoundDef VPE_BallLightning_Zap;
        public static SoundDef VPE_Vortex_Sustainer;
        public static SoundDef VPE_Skipdoor_Sustainer;
        public static SoundDef VPE_RaidPause_Sustainer;
        public static SoundDef VPE_GuardianSkipbarrier_Sustainer;
        public static SoundDef VPE_PowerLeap_Land;
        public static SoundDef VPE_Killskip_Jump_01a;
        public static SoundDef VPE_Killskip_Jump_01b;
        public static SoundDef VPE_Killskip_Jump_01c;
        public static SoundDef VPE_TimeSphere_Sustainer;
        public static SoundDef Psycast_Neuroquake_CastLoop;
        public static SoundDef Psycast_Neuroquake_CastEnd;

        public static StatDef VPE_PsyfocusCostFactor;
        public static StatDef VPE_PsychicEntropyMinimum;
        public static JobDef  VPE_UseSkipdoor;

        public static FleckDef VPE_VortexSpark;
        public static FleckDef VPE_WarlordZap;
        public static FleckDef VPE_AggresiveHeatDump;

        public static MeditationFocusDef VPE_Group;
        public static MeditationFocusDef VPE_Archotech;
        public static MeditationFocusDef VPE_Science;
        public static MeditationFocusDef VPE_Wealth;

        public static EffecterDef      Interceptor_BlockedProjectilePsychic;
        public static EffecterDef      VPE_Skip_ExitNoDelayRed;
        public static HistoryEventDef  VPE_Foretelling;
        public static GameConditionDef VPE_PsychicFlashstorm;
        public static GameConditionDef VPE_TimeQuake;

        [DefAlias("VPE_Hurricane")] public static WeatherDef       VPE_Hurricane_Weather;
        [DefAlias("VPE_Hurricane")] public static GameConditionDef VPE_Hurricane_Condition;
    }
}