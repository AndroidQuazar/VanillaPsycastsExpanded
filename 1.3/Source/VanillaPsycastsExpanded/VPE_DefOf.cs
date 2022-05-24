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
        public static HediffDef VFEP_HypothermicSlowdown;
        public static HediffDef VPE_IceShield;
        public static HediffDef VPE_FrostRay;
        public static HediffDef VPE_IceBlock;
        public static HediffDef VPE_Blizzard;
        public static HediffDef VPE_InfinitePower;
        public static HediffDef VPE_Lucky;
        public static HediffDef VPE_UnLucky;

        public static ThingDef VPE_ChainBolt;
        public static ThingDef VPE_Bolt;
        public static ThingDef VPE_Mote_FireBeam;
        public static ThingDef VPE_HurricaneMaker;
        public static ThingDef VPE_Skipdoor;
        public static ThingDef VPE_Mote_GreenMist;
        public static ThingDef VPE_JumpingPawn;
        public static ThingDef VPE_TimeSphere;
        public static ThingDef VPE_SkyChanger;
        public static ThingDef VPE_PsycastAreaEffectMaintained;
        public static ThingDef VPE_HeatPearls;
        public static ThingDef VPE_Eltex;
        public static ThingDef VPE_EltexOre;
        public static ThingDef VPE_PsycastPsychicEffectTransfer;
        public static ThingDef VPE_Mote_Cast;
        public static ThingDef VPE_Psyring;
        public static ThingDef Plant_Brambles;

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
        public static SoundDef VPE_FrostRay_Sustainer;

        public static FleckDef    VPE_VortexSpark;
        public static FleckDef    VPE_WarlordZap;
        public static FleckDef    VPE_AggresiveHeatDump;
        public static FleckDef    PsycastAreaEffect;
        public static StatDef     VPE_PsyfocusCostFactor;
        public static StatDef     VPE_PsychicEntropyMinimum;
        public static JobDef      VPE_UseSkipdoor;
        public static JobDef      VPE_StandFreeze;
        public static EffecterDef VPE_Haywire;


        public static MeditationFocusDef VPE_Archotech;
        public static MeditationFocusDef VPE_Science;
        public static MentalStateDef     VPE_Wander_Sad;
        public static EffecterDef        Interceptor_BlockedProjectilePsychic;
        public static EffecterDef        VPE_Skip_ExitNoDelayRed;
        public static HistoryEventDef    VPE_Foretelling;
        public static HistoryEventDef    VPE_GiftedEltex;
        public static HistoryEventDef    VPE_SoldEltex;
        public static PawnKindDef        VPE_RockConstruct;
        public static PawnKindDef        VPE_SteelConstruct;

        public static GameConditionDef VPE_PsychicFlashstorm;
        public static GameConditionDef VPE_TimeQuake;

        [DefAlias("VPE_Hurricane")]      public static WeatherDef       VPE_Hurricane_Weather;
        [DefAlias("VPE_Hurricane")]      public static GameConditionDef VPE_Hurricane_Condition;
        [DefAlias("VPE_RockConstruct")]  public static ThingDef         VPE_Race_RockConstruct;
        [DefAlias("VPE_SteelConstruct")] public static ThingDef         VPE_Race_SteelConstruct;

        static VPE_DefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(VPE_DefOf));
        }
    }
}