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
            __state = ___tradeables.Where(x => x.ThingDef == VPE_DefOf.VPE_Eltex).Sum(x => x.CountToTransferToDestination);
        }
        public static void Postfix(int __state, bool __result)
        {
            if (__result && TradeSession.trader.Faction != Faction.OfEmpire)
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