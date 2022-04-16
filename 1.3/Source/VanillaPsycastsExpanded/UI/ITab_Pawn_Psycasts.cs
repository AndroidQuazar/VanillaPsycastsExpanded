namespace VanillaPsycastsExpanded.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using VFECore.UItils;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    [StaticConstructorOnStartup]
    public class ITab_Pawn_Psycasts : ITab
    {
        private static readonly float[][] abilityTreeXOffsets =
        {
            new[] {-18f},
            new[] {-11f - 36f, 11f},
            new[] {-18f - 15f - 36f, -18f, 18f + 15f}
        };

        private readonly List<MeditationFocusDef>                   foci;
        private readonly Dictionary<string, List<PsycasterPathDef>> pathsByTab;
        private readonly List<TabRecord>                            tabs;
        private readonly Dictionary<AbilityDef, Vector2>            abilityPos = new();

        private CompAbilities           compAbilities;
        private string                  curTab;
        private Hediff_PsycastAbilities hediff;
        private float                   lastPathsHeight;
        private float                   lastPsysetsHeight;
        private int                     pathsPerRow;
        private Vector2                 pathsScrollPos;
        private Pawn                    pawn;
        private Vector2                 psysetsScrollPos;

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
            this.foci = DefDatabase<MeditationFocusDef>.AllDefs.OrderByDescending(def => def.modContentPack.IsOfficialMod).ThenByDescending(def => def.label)
                                                       .ToList();
            this.tabs   = this.pathsByTab.Select(kv => new TabRecord(kv.Key, () => this.curTab = kv.Key, () => this.curTab == kv.Key)).ToList();
            this.curTab = this.pathsByTab.Keys.FirstOrDefault();
        }

        public override bool IsVisible =>
            Find.Selector.SingleSelectedThing is Pawn pawn && pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_PsycastAbilityImplant);

        protected override void UpdateSize()
        {
            base.UpdateSize();
            this.size.y      = this.PaneTopY - 30f;
            this.pathsPerRow = Mathf.FloorToInt(this.size.x * 0.67f / 200f);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            this.pawn          = (Pawn) Find.Selector.SingleSelectedThing;
            this.hediff        = (Hediff_PsycastAbilities) this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant);
            this.compAbilities = this.pawn.GetComp<CompAbilities>();
        }

        protected override void CloseTab()
        {
            base.CloseTab();
            this.pawn          = null;
            this.hediff        = null;
            this.compAbilities = null;
        }

        protected override void FillTab()
        {
            GameFont         font         = Text.Font;
            TextAnchor       anchor       = Text.Anchor;
            Rect             tabRect      = new(Vector2.one                  * 20f, this.size - Vector2.one * 40f);
            Rect             pawnAndStats = tabRect.TakeLeftPart(this.size.x * 0.3f);
            Rect             paths        = tabRect.ContractedBy(5f);
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
            if (listing.ButtonTextLabeled("VPE.PsycasterStats".Translate(), "VPE.Upgrade".Translate()))
                Messages.Message("Upgraded!", MessageTypeDefOf.PositiveEvent);
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

            listing.Gap(10f);
            listing.Label("VPE.PsysetCustomize".Translate());
            Text.Font = GameFont.Tiny;
            listing.Label("VPE.PsysetDesc".Translate());
            Rect psysets = listing.GetRect(240f);
            Widgets.DrawMenuSection(psysets);
            Rect viewRect = new(0, 0, psysets.width - 20f, this.lastPsysetsHeight);
            Widgets.BeginScrollView(psysets, ref this.psysetsScrollPos, viewRect);
            this.DoPsysets(viewRect);
            Widgets.EndScrollView();
            listing.End();
            if (this.pathsByTab.NullOrEmpty())
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font   = GameFont.Medium;
                Widgets.DrawMenuSection(paths);
                Widgets.Label(paths, "No Paths");
            }
            else
            {
                TabDrawer.DrawTabs(new Rect(paths.x, paths.y + 40f, paths.width, paths.height), this.tabs);
                paths.yMin += 40f;
                Widgets.DrawMenuSection(paths);
                viewRect = new Rect(0, 0, paths.width - 20f, this.lastPathsHeight);
                Widgets.BeginScrollView(paths, ref this.pathsScrollPos, viewRect);
                this.DoPaths(viewRect);
                Widgets.EndScrollView();
            }

            Text.Font   = font;
            Text.Anchor = anchor;
        }

        private void DoFocus(Rect inRect, MeditationFocusDef def)
        {
            Widgets.DrawBox(inRect, 3, Texture2D.grayTexture);
            bool unlocked = this.hediff.unlockedMeditationFoci.Contains(def);
            GUI.color = unlocked ? Color.white : Color.gray;
            GUI.DrawTexture(inRect.ContractedBy(5f), def.Icon());
            GUI.color = Color.white;
            if (this.hediff.points >= 1 && !unlocked)
                if (Widgets.ButtonText(new Rect(inRect.xMax - 13f, inRect.yMax - 13f, 12f, 12f), "▲"))
                {
                    this.hediff.SpentPoints();
                    this.hediff.UnlockMeditationFocus(def);
                }
        }

        private void DoPsysets(Rect inRect)
        {
            Listing_Standard listing = new();
            listing.Begin(inRect);
            if (Widgets.ButtonText(listing.GetRect(70f).LeftHalf().ContractedBy(10f), "VPE.CreatePsyset".Translate()))
                Messages.Message("Created!", MessageTypeDefOf.PositiveEvent);
            listing.End();
            this.lastPsysetsHeight = listing.CurHeight;
        }

        private void DoPaths(Rect inRect)
        {
            Vector2 curPos       = inRect.position + Vector2.one * 10f;
            float   widthPerPath = (inRect.width - (this.pathsPerRow + 1) * 10f) / this.pathsPerRow;
            float   maxHeight    = 0f;
            int     paths        = this.pathsPerRow;
            foreach (PsycasterPathDef def in this.pathsByTab[this.curTab].OrderByDescending(path => this.hediff.unlockedPaths.Contains(path))
                                                 .ThenBy(path => path.order).ThenBy(path => path.label))
            {
                float height = widthPerPath / def.backgroundImage.width * def.backgroundImage.height + 30f;
                Rect  rect   = new(curPos, new Vector2(widthPerPath, height));
                GUI.color = new ColorInt(97, 108, 122).ToColor;
                Widgets.DrawBox(rect.ExpandedBy(2f), 1, Texture2D.whiteTexture);
                GUI.color = Color.white;
                Rect labelRect = rect.TakeBottomPart(30f);
                Widgets.DrawRectFast(labelRect, Widgets.WindowBGFillColor);
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(labelRect, def.LabelCap);
                GUI.DrawTexture(rect, def.backgroundImage);
                Text.Anchor = TextAnchor.UpperLeft;
                if (this.hediff.unlockedPaths.Contains(def))
                {
                    if (def.HasAbilities) this.DoPathAbilities(rect, def);
                }
                else
                {
                    Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.7f));
                    if (this.hediff.points >= 1 && Widgets.ButtonText(rect.CenterRect(new Vector2(140f, 30f)), "VPE.Unlock".Translate()))
                    {
                        this.hediff.SpentPoints();
                        this.hediff.UnlockPath(def);
                    }
                }

                maxHeight =  Mathf.Max(maxHeight, height + 10f);
                curPos.x  += widthPerPath + 10f;
                paths--;
                if (paths == 0)
                {
                    curPos.x  =  inRect.x + 10f;
                    curPos.y  += maxHeight;
                    paths     =  this.pathsPerRow;
                    maxHeight =  0f;
                }
            }

            this.lastPathsHeight = curPos.y + maxHeight;
        }

        private void DoPathAbilities(Rect inRect, PsycasterPathDef path)
        {
            foreach (AbilityDef def in path.abilities)
                if (def.Psycast()?.prerequisites is { } prerequisites && this.abilityPos.ContainsKey(def))
                    foreach (AbilityDef abilityDef in prerequisites.Where(abilityDef => this.abilityPos.ContainsKey(abilityDef)))
                    {
                        Color color = Color.grey;
                        if (this.compAbilities.HasAbility(abilityDef))
                            color = Color.white;

                        Widgets.DrawLine(this.abilityPos[def], this.abilityPos[abilityDef], color, 2f);
                    }

            for (int level = 0; level < path.abilityLevelsInOrder.Length; level++)
            {
                Rect levelRect = new(inRect.x, inRect.y + (path.MaxLevel - 1 - level) * inRect.height / path.MaxLevel + 10f, inRect.width, inRect.height / 5f);
                AbilityDef[] abilities = path.abilityLevelsInOrder[level];
                for (int pos = 0; pos < abilities.Length; pos++)
                {
                    Rect       rect = new(levelRect.x + levelRect.width / 2 + abilityTreeXOffsets[abilities.Length - 1][pos], levelRect.y, 36f, 36f);
                    AbilityDef def  = abilities[pos];
                    if (def.defName == PsycasterPathDef.BlankLabel) continue;
                    this.abilityPos[def] = rect.center;
                    this.DoAbility(rect, def);
                }
            }
        }

        private void DoAbility(Rect inRect, AbilityDef ability)
        {
            bool unlockable = false;
            bool locked     = false;
            if (!this.compAbilities.HasAbility(ability))
            {
                if (ability.Psycast().PrereqsCompleted(this.pawn) && this.hediff.points >= 1)
                    unlockable = true;
                else locked    = true;
            }

            Color color = Mouse.IsOver(inRect) ? GenUI.MouseoverColor : Color.white;
            MouseoverSounds.DoRegion(inRect, SoundDefOf.Mouseover_Command);
            if (unlockable) QuickSearchWidget.DrawStrongHighlight(inRect.ExpandedBy(12f));
            GUI.color = color;
            GUI.DrawTexture(inRect, Command.BGTexShrunk);
            GUI.color = Color.white;
            GUI.DrawTexture(inRect, ability.icon);
            if (locked) Widgets.DrawRectFast(inRect, new Color(0f, 0f, 0f, 0.6f));

            TooltipHandler.TipRegion(inRect, () => $"{ability.LabelCap}\n\n{ability.description}{(unlockable ? "\n\n" + "VPE.ClickToUnlock".Translate() : "")}",
                                     ability.GetHashCode());

            if (unlockable && Widgets.ButtonInvisible(inRect))
            {
                this.hediff.SpentPoints();
                ability.abilityClass ??= typeof(Ability_Blank); // TODO: Remove this (it's for debugging)
                this.compAbilities.GiveAbility(ability);
            }
        }
    }
}