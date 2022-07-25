namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Verse;

    [HarmonyPatch(typeof(TradeDeal), "TryExecute", new Type[]
    {
        typeof(bool),
    }, new ArgumentType[] { ArgumentType.Out })]
    public static class TradeDeal_TryExecute_Patch
    {
        public static void Prefix(List<Tradeable> ___tradeables, out int __state)
        {
            __state = 0;
            foreach (var tradeable in ___tradeables)
            {
                __state += tradeable.ThingDef.GetEltexOrEltexMaterialCount() * tradeable.CountToTransferToDestination;
            }
        }

        public static int GetEltexOrEltexMaterialCount(this ThingDef def)
        {
            if (def != null)
            {
                if (def == VPE_DefOf.VPE_Eltex)
                {
                    return 1;
                }
                else if (def.costList != null)
                {
                    var firstCost = def.costList.FirstOrDefault(x => x.thingDef == VPE_DefOf.VPE_Eltex);
                    if (firstCost != null)
                    {
                        return firstCost.count;
                    }
                }
                else
                {
                    foreach (var recipe in DefDatabase<RecipeDef>.AllDefs)
                    {
                        if (recipe.ProducedThingDef == def)
                        {
                            var firstCost = recipe.ingredients.FirstOrDefault(x => x.IsFixedIngredient && x.FixedIngredient == VPE_DefOf.VPE_Eltex);
                            if (firstCost != null)
                            {
                                return (int)firstCost.GetBaseCount();
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public static void Postfix(int __state, bool __result)
        {
            if (__state > 0 &&__result && TradeSession.trader.Faction != Faction.OfEmpire)
            {
                if (Rand.Chance(0.5f))
                {
                    Current.Game.GetComponent<GameComponent_PsycastsManager>().goodwillImpacts.Add(new GoodwillImpactDelayed
                    {
                        factionToImpact = Faction.OfEmpire,
                        goodwillImpact = -__state,
                        historyEvent = TradeSession.giftMode ? VPE_DefOf.VPE_GiftedEltex : VPE_DefOf.VPE_SoldEltex,
                        impactInTicks = Find.TickManager.TicksGame + (int)(GenDate.TicksPerDay * Rand.Range(7f, 14f)),
                        letterLabel = "VPE.EmpireAngeredTitle".Translate(),
                        letterDesc = "VPE.EmpireAngeredDesc".Translate(TradeSession.giftMode ? "VPE.Gifting".Translate() : "VPE.Trading".Translate()),
                        relationInfoKey = "VPE.FactionRelationReducedInfo"
                    });
                }
            }
        }
    }
}