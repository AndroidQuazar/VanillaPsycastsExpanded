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
        public static HediffDef VPE_Hurricane;

        public static ThingDef VPE_ChainBolt;
        public static ThingDef VPE_Bolt;
        public static ThingDef VPE_Mote_FireBeam;
        public static ThingDef VPE_HurricaneMaker;

        public static SoundDef VPE_Recharge_Sustainer;
        public static SoundDef VPE_BallLightning_Zap;
        public static SoundDef VPE_Vortex_Sustainer;

        public static FleckDef VPE_VortexSpark;

        [DefAlias("VPE_Hurricane")] public static WeatherDef       VPE_Hurricane_Weather;
        [DefAlias("VPE_Hurricane")] public static GameConditionDef VPE_Hurricane_Condition;
    }
}