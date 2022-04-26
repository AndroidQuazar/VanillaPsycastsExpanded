namespace VanillaPsycastsExpanded.UI
{
    using System.Collections.Generic;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using VFECore.UItils;

    [StaticConstructorOnStartup]
    public static class PsycastsUIUtility
    {
        private static readonly Dictionary<MeditationFocusDef, Texture2D> meditationIcons;

        static PsycastsUIUtility()
        {
            meditationIcons = new Dictionary<MeditationFocusDef, Texture2D>();
            foreach (MeditationFocusDef def in DefDatabase<MeditationFocusDef>.AllDefs)
            {
                MeditationFocusExtension_Icon ext = def.GetModExtension<MeditationFocusExtension_Icon>();
                if (ext is null)
                {
                    Log.Error(
                        $"MeditationFocusDef {def} does not have a MeditationFocusExtension_Icon, meaning {def.modContentPack?.Name} is incompatible with Vanilla Psycasts Expanded");
                    meditationIcons.Add(def, BaseContent.WhiteTex);
                }
                else
                    meditationIcons.Add(def, ContentFinder<Texture2D>.Get(ext.icon));
            }
        }

        public static void LabelWithIcon(this Listing_Standard listing, Texture2D icon, string label)
        {
            float height = Text.CalcHeight(label, listing.ColumnWidth);
            Rect  rect   = listing.GetRect(height);
            float width  = icon.width * (height / icon.height);
            GUI.DrawTexture(rect.TakeLeftPart(width), icon);
            rect.xMin += 3f;
            Widgets.Label(rect, label);
            listing.Gap(3f);
        }

        public static void StatDisplay(this Listing_Standard listing, Texture2D icon, StatDef stat, Thing thing)
        {
            listing.LabelWithIcon(
                icon,
                stat.LabelCap + ": " + stat.Worker.GetStatDrawEntryLabel(stat, thing.GetStatValue(stat), stat.toStringNumberSense, StatRequest.For(thing)));
        }

        public static Rect CenterRect(this Rect rect, Vector2 size) => new(rect.center - size / 2f, size);

        public static Texture2D Icon(this MeditationFocusDef def) => meditationIcons[def];
    }
}