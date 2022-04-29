namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;

    public class FocusStrengthOffset_NearbyOfTechlevel : FocusStrengthOffset
    {
        private const           int                  CACHE_REFRESH = 600;
        private static readonly Dictionary<Key, int> cache         = new();
        private static          int                  lastCacheTick;

        public TechLevel techLevel;
        public float     radius = 10f;
        public float     max;

        public virtual int MaxThings => Mathf.FloorToInt(this.max / this.offset);

        public override float GetOffset(Thing parent, Pawn user = null) => this.offset * this.CountThings(parent.Position, parent.Map);

        public override string GetExplanation(Thing parent)
        {
            int num = this.CountThings(parent.Position, parent.Map);
            return "VPE.ThingsOfLevel".Translate(num, this.techLevel.ToString()) + ": " + (this.offset * num).ToStringWithSign("0%");
        }

        public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
        {
            base.PostDrawExtraSelectionOverlays(parent, user);
            GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor);
            foreach (Thing thing in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, this.radius, false)
                                             .Where(thing => thing.def.techLevel == this.techLevel).Take(this.MaxThings))
                GenDraw.DrawLineBetween(parent.TrueCenter(), thing.TrueCenter(), SimpleColor.Green);
        }

        protected virtual int CountThings(IntVec3 cell, Map map)
        {
            Key key = new() {cell = cell, techLevel = this.techLevel, mapId = map.Index};
            if (Find.TickManager.TicksGame - lastCacheTick >= CACHE_REFRESH) cache.Clear();
            if (cache.TryGetValue(key, out int num)) return num;
            lastCacheTick = Find.TickManager.TicksGame;
            num = GenRadial.RadialDistinctThingsAround(cell, map, this.radius, false).Take(this.MaxThings)
                           .Count(thing => thing.def.techLevel == this.techLevel);
            cache.Add(key, num);
            return num;
        }

        private struct Key
        {
            public int       mapId;
            public TechLevel techLevel;
            public IntVec3   cell;
        }
    }
}