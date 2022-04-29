namespace VanillaPsycastsExpanded.Harmonist
{
    using System.Collections.Generic;
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_TransmuteItem : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Thing   item  = target.Thing;
            float   value = item.MarketValue * item.stackCount;
            IntVec3 pos   = item.Position;

            List<ThingDef> allItems =
                (from thingDef in DefDatabase<ThingDef>.AllDefs
                 where thingDef.category == ThingCategory.Item
                 where !thingDef.IsCorpse
                 where !thingDef.MadeFromStuff
                 where !thingDef.IsEgg
                 let marketValue = thingDef.BaseMarketValue
                 let count = Mathf.FloorToInt(value / thingDef.BaseMarketValue)
                 where marketValue <= value
                 where count       <= thingDef.stackLimit
                 where count       >= 1
                 select thingDef
                ).ToList();

            float WeightSelector(ThingDef def)
            {
                float amount = value / def.BaseMarketValue;
                float weight = Mathf.Abs(amount - Mathf.FloorToInt(amount));
                return weight;
            }

            float    maxWeight = allItems.Max(WeightSelector);
            ThingDef chosen    = allItems.RandomElementByWeight(def => maxWeight - WeightSelector(def));
            item.Destroy();
            item            = ThingMaker.MakeThing(chosen);
            item.stackCount = Mathf.FloorToInt(value / chosen.BaseMarketValue);
            GenSpawn.Spawn(item, pos, this.pawn.Map);
        }

        public override bool CanHitTarget(LocalTargetInfo target) => this.targetParams.CanTarget(target.Thing, this) &&
                                                                     GenSight.LineOfSight(this.pawn.Position, target.Cell, this.pawn.Map, true);

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!base.ValidateTarget(target, showMessages)) return false;
            if (target.Thing.MarketValue < 1f)
            {
                if (showMessages) Messages.Message("VPE.TooCheap".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            return true;
        }
    }
}