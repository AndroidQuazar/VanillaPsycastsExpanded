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
            listing.Label("VPE.PsycasterSpawnBaseChance".Translate() + ": " + (Settings.baseSpawnChance * 100) + "%");
            Settings.baseSpawnChance = listing.Slider(Settings.baseSpawnChance, 0, 1f);
            listing.Label("VPE.PsycasterSpawnAdditional".Translate() + ": " + (Settings.additionalAbilityChance * 100) + "%");
            Settings.additionalAbilityChance = listing.Slider(Settings.additionalAbilityChance, 0, 1f);
            listing.End();
        }
    }

    public class PsycastSettings : ModSettings
    {
        public float XPPerPercent = 1f;
        public float baseSpawnChance = 0.1f;
        public float additionalAbilityChance = 0.1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.XPPerPercent, "xpPerPercent", 1f);
            Scribe_Values.Look(ref this.baseSpawnChance, nameof(this.baseSpawnChance), 0);
            Scribe_Values.Look(ref this.additionalAbilityChance, nameof(this.additionalAbilityChance), 0);
        }
    }
}