namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using Skipmaster;
    using Verse;

    public class PsycastsMod : Mod
    {
        public static Harmony Harm;

        public PsycastsMod(ModContentPack content) : base(content)
        {
            LongEventHandler.ExecuteWhenFinished(() => { Skipdoor.Init(content); });
            Harm = new Harmony("OskarPotocki.VanillaPsycastsExpanded");
            Harm.Patch(AccessTools.Method(typeof(ThingDefGenerator_Neurotrainer), nameof(ThingDefGenerator_Neurotrainer.ImpliedThingDefs)),
                       postfix: new HarmonyMethod(typeof(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch),
                                                  nameof(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch.Postfix)));
        }
    }
}