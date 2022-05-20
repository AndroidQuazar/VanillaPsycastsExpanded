namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using Skipmaster;
    using UnityEngine;
    using Verse;

    public class PsycastsMod : Mod
    {
        public static Harmony         Harm;
        public static PsycastSettings Settings;

        public PsycastsMod(ModContentPack content) : base(content)
        {
            LongEventHandler.ExecuteWhenFinished(() => { Skipdoor.Init(content); });
            Harm     = new Harmony("OskarPotocki.VanillaPsycastsExpanded");
            Settings = this.GetSettings<PsycastSettings>();
            Harm.Patch(AccessTools.Method(typeof(ThingDefGenerator_Neurotrainer), nameof(ThingDefGenerator_Neurotrainer.ImpliedThingDefs)),
                       postfix: new HarmonyMethod(typeof(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch),
                                                  nameof(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch.Postfix)));
        }

        public override string SettingsCategory() => "VanillaPsycastsExpanded".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing = new();
            listing.Begin(inRect);
            listing.Label("VPE.XPPerPercent".Translate() + ": " + Settings.XPPerPercent);
            Settings.XPPerPercent = listing.Slider(Settings.XPPerPercent, 0.1f, 10f);
            listing.End();
        }
    }

    public class PsycastSettings : ModSettings
    {
        public float XPPerPercent = 1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.XPPerPercent, "xpPerPercent", 1f);
        }
    }
}