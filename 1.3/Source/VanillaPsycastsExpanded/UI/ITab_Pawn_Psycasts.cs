namespace VanillaPsycastsExpanded.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;

    [StaticConstructorOnStartup]
    public class ITab_Pawn_Psycasts : ITab
    {
        private          string                                     curTab;
        private readonly List<MeditationFocusDef>                   foci;
        private          Hediff_PsycastAbilities                    hediff;
        private readonly Dictionary<string, List<PsycasterPathDef>> pathsByTab;
        private          Pawn                                       pawn;
        private          List<TabRecord>                            tabs;

        static ITab_Pawn_Psycasts()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
                if (def.race is {Humanlike: true})
                {
                    def.inspectorTabs.Add(typeof(ITab_Pawn_Psycasts));
                    def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Psycasts)));
                }
        }

        public ITab_Pawn_Psycasts()
        {
            this.labelKey   = "VPE.Psycasts";
            this.size       = new Vector2(UI.screenWidth, UI.screenHeight * 0.75f);
            this.pathsByTab = DefDatabase<PsycasterPathDef>.AllDefs.GroupBy(def => def.tab).ToDictionary(group => group.Key, group => group.ToList());
            this.foci       = DefDatabase<MeditationFocusDef>.AllDefs.OrderByDescending(def => def.modContentPack.IsOfficialMod).ThenByDescending(def => def.label).ToList();
            this.tabs       = this.pathsByTab.Select(kv => new TabRecord(kv.Key, () => this.curTab = kv.Key, () => this.curTab == kv.Key)).ToList();
        }

        public override bool IsVisible => Find.Selector.SingleSelectedThing is Pawn pawn && pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_PsycastAbilityImplant);

        protected override void UpdateSize()
        {
            base.UpdateSize();
            this.size.y = this.PaneTopY - 30f;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            this.pawn   = (Pawn) Find.Selector.SingleSelectedThing;
            this.hediff = (Hediff_PsycastAbilities) this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
        }

        protected override void CloseTab()
        {
            base.CloseTab();
            this.pawn   = null;
            this.hediff = null;
        }

        protected override void FillTab()
        {
            GameFont         font         = Text.Font;
            TextAnchor       anchor       = Text.Anchor;
            Rect             pawnAndStats = new(20f, 20f, this.size.x / 3f                              - 5f, this.size.y - 40f);
            Rect             paths        = new(this.size.x / 3f, 10f, this.size.x * 0.67f, this.size.y - 40f);
            Listing_Standard listing      = new();
            listing.Begin(pawnAndStats);
            Text.Font = GameFont.Medium;
            listing.Label(this.pawn.Name.ToStringFull);
            listing.Label("VPE.PsyLevel".Translate(this.hediff.level));
            listing.Gap(10f);
            Rect bar = listing.GetRect(60f).ContractedBy(10f, 0f);
            Text.Anchor = TextAnchor.MiddleCenter;
            int xpForNext = Hediff_PsycastAbilities.ExperienceRequiredForLevel(this.hediff.level + 1);
            Widgets.FillableBar(bar, this.hediff.experience / xpForNext);
            Widgets.Label(bar, $"{this.hediff.experience} / {xpForNext}");
            Text.Font = GameFont.Tiny;
            listing.Label("VPE.EarnXP".Translate());
            listing.Gap(10f);
            Text.Font   = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            listing.Label("VPE.Points".Translate(this.hediff.points));
            Text.Font = GameFont.Tiny;
            listing.Label("VPE.SpendPoints".Translate());
            listing.Gap(3f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Text.Font   = GameFont.Small;
            if (listing.ButtonTextLabeled("VPE.PsycasterStats".Translate(), "VPE.Upgrade".Translate())) Messages.Message("Upgraded!", MessageTypeDefOf.PositiveEvent);
            listing.StatDisplay(TexPsycasts.IconNeuralHeatLimit,     StatDefOf.PsychicEntropyMax,          this.pawn);
            listing.StatDisplay(TexPsycasts.IconNeuralHeatRegenRate, StatDefOf.PsychicEntropyRecoveryRate, this.pawn);
            listing.StatDisplay(TexPsycasts.IconPsychicSensitivity,  StatDefOf.PsychicSensitivity,         this.pawn);
            listing.StatDisplay(TexPsycasts.IconPsyfocusGain,        StatDefOf.MeditationFocusGain,        this.pawn);
            listing.LabelWithIcon(TexPsycasts.IconFocusTypes, "VPE.FocusTypes".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            Rect  fociRect = listing.GetRect(48f);
            float x        = pawnAndStats.x;
            foreach (MeditationFocusDef def in this.foci)
            {
                Rect rect = new(x, fociRect.y, 48f, 48f);
                this.DoFocus(rect, def);
                x += 50f;
                if (x >= pawnAndStats.xMax)
                {
                    x        = pawnAndStats.x;
                    fociRect = listing.GetRect(48f);
                }
            }

            listing.End();
            Text.Font   = font;
            Text.Anchor = anchor;
        }

        private void DoFocus(Rect inRect, MeditationFocusDef def)
        {
            Widgets.DrawBox(inRect, 3, Texture2D.grayTexture);
            bool unlocked = this.hediff.unlockedMeditationFoci.Contains(def);
            GUI.color = unlocked ? Color.white : Color.gray;
            Widgets.DrawTextureFitted(inRect.ContractedBy(5f), def.Icon(), 1f);
            GUI.color = Color.white;
            if (this.hediff.points >= 1 && !unlocked)
                if (Widgets.ButtonText(new Rect(inRect.xMax - 13f, inRect.yMax - 13f, 12f, 12f), "▲"))
                {
                    this.hediff.SpentPoints();
                    this.hediff.UnlockMeditationFocus(def);
                }
        }
    }
}